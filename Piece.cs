using System.Diagnostics;
using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Piece
    {
        private const int _MANY_SIDES = 6;
        private const int _SPIDER_MAX_STEP_COUNT = 3;
        private readonly string _piece;
        private Board _board { get; set; }
        private Dictionary<(int, int), Stack<Piece>> _point_stack { get { return _board.pieces; } }

        public Color Color { get; set; }
        public int Number { get; set; }
        public Insect Insect { get; set; }
        public (int x, int y) Point { get; set; }
        public Dictionary<string, (int, int)> Sides { get {
            return new Dictionary<string, (int, int)>()
                {
                    // These values may not need to be hardcoded.
                    // However, hardcoding them may make them more efficient
                    { "NT", (Point.x + 1, Point.y + 1) },         // [0] North
                    { "NW", (Point.x + (-1), Point.y + 1) },      // [1] Northwest
                    { "SW", (Point.x + (-2), Point.y + 0) },      // [2] Southwest
                    { "ST", (Point.x + (-1), Point.y + (-1)) },   // [3] South
                    { "SE", (Point.x + 1, Point.y + (-1)) },      // [4] Southeast
                    { "NE", (Point.x + 2, Point.y + 0) },         // [5] Northeast
                };
        } }
        public Dictionary<string, (int, int)> Neighbors { get; set; }
        public List<(int, int)> SpotsAround { get { return Sides.Except(Neighbors).Select(spot => spot.Value).ToList(); } }
        public int ManyNeighbors { get{ return Neighbors.Count; } }
        public override string ToString() { return _piece; }
        public bool IsSurrounded() { return Neighbors.Count == _MANY_SIDES; }

        public Piece(string piece, (int, int) piecePoint)
        {
            // Necessary for checking whether the one-hive rule has been violated  
            _board = new Board();
            _piece = piece;
            Color = piece[0] == 'b' ? Color.Black : Color.White;
            Insect = piece[1] == 'Q'
            ? Insect.QueenBee
            : piece[1] == 'B'
            ? Insect.Beetle
            : piece[1] == 'G'
            ? Insect.Grasshopper
            : piece[1] == 'S'
            ? Insect.Spider
            // piece[1] == 'A'
            : Insect.Ant;
            Number = piece[2] - '0';
            Neighbors = new Dictionary<string, (int, int)>();
            Point = piecePoint;
        }

        public List<(int, int)> GetMovingSpots(Board board)
        {
            _board = board;
            return Insect switch
            {
                Insect.Ant => _GetAntMovingSpots(),
                Insect.Beetle => _GetBeetleMovingSpots(),
                Insect.Grasshopper => _GetGrasshopperMovingSpots(),
                Insect.Spider => _GetSpiderMovingSpots(),
                Insect.QueenBee => _GetQueenMovingSpots(),
                _ => new List<(int, int)>() { (1, 2) },// this is an invalid position
            };
        }

        // This should be used for the pieces that have an expensive move generation–i.e., For now, only the Ant
        // TODO: Test whether adding this validation actually improves performance on other pieces
        // (which probably does not because I would be validating its surroundings twice) 
        public bool IsPinned(bool isBeetle = false)
        {
            foreach((int, int) side in Sides.Values)
            {
                // If there is at least one valid path
                if (_IsValidPath(Point, side, isBeetle))
                {
                    // It is not pinned
                    return false;
                }
            }

            // None of its side were a valid path,
            // So this piece is pinned
            return true;
        }


        private List<(int, int)> _GetAntMovingSpots()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int, int)> results = new();

            // Before getting all the open spots (which is an expensive computation)
            // Make sure this piece is not pinned
            if (!IsPinned())
            {
                List<(int, int)> spots = new();

                // Get all the open spots
                foreach (KeyValuePair<(int, int), Stack<Piece>> p in _point_stack)
                {
                    foreach ((int, int) spot in p.Value.Peek().SpotsAround)
                    {
                        if (!spots.Contains(spot))
                        {
                            spots.Add(spot);
                        }
                    }
                }

                // Validate each moving from `spot` to `adjacentSpot`
                foreach((int x, int y) spot in spots)
                {
                    foreach((int x, int y) sideOffset in SIDE_OFFSETS_LIST)
                    {
                        (int, int) adjacentSpot = (spot.x + sideOffset.x, spot.y + sideOffset.y);

                        // for testing only!
                        if (Color == Color.White && Point.x == 4 && Point.y == 2 && adjacentSpot.Item1 == 5 && adjacentSpot.Item2 == 1 && Neighbors.Count == 1 && Neighbors.Contains(new KeyValuePair<string, (int, int)>("bA1", (3, 1))))
                        {
                            // break point!
                            Console.WriteLine("break point!");
                        }

                        if (!results.Contains(adjacentSpot) && spots.Contains(adjacentSpot) && _IsValidPath(spot, adjacentSpot))
                        {
                            results.Add(adjacentSpot);
                        }
                    }
                }
            }


            stopwatch.Stop();
            PrintRed("Generating Ant Moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return results;
        }

        private List<(int, int)> _GetBeetleMovingSpots()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int, int)> validMoves = new();

            foreach ((int, int) side in Sides.Values)
            {
                // Filter out the sides that would break the hive
                if (_IsValidPath(this.Point, side, true))
                {
                    validMoves.Add(side);
                }
            }

            stopwatch.Stop();
            PrintRed("Generating Beetle moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return validMoves;
        }

        private List<(int, int)> _GetGrasshopperMovingSpots()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int x, int y)> positions = new();
            foreach ((int x, int y) sideOffset in SIDE_OFFSETS.Values)
            {
                (int x, int y) nextSpot = (this.Point.x + sideOffset.x, this.Point.y + sideOffset.y);
                bool firstIsValid = false;

                // Keep hopping over pieces
                while (_point_stack.ContainsKey(nextSpot))
                {
                    nextSpot = (nextSpot.x + sideOffset.x, nextSpot.y + sideOffset.y);
                    firstIsValid = true;
                }

                if (firstIsValid)
                {
                    // until you find a spot
                    if (_DoesNotBreakHive(nextSpot))
                    {
                        positions.Add(nextSpot);
                    }
                }
            }

            stopwatch.Stop();
            PrintRed("Generating grasshoper moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return positions;
        }

        private List<(int, int)> _GetSpiderMovingSpots()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int x, int y)> positions = new();
            Dictionary<(int x, int y), bool> visited = new();
            foreach ((int x, int y) side in SpotsAround)
            {
                bool hasNotBeenVisited = !visited.ContainsKey(side) || (visited.ContainsKey(side) && !visited[side]);
                if (hasNotBeenVisited && _IsValidPath(Point, side))
                {
                    _SpiderDFS(ref positions, ref visited, side, 1, _SPIDER_MAX_STEP_COUNT);
                }
            }

            stopwatch.Stop();
            PrintRed("Generating spider moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return positions;
        }

        private List<(int, int)> _GetQueenMovingSpots()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int, int)> spots = new();
            foreach ((int, int) spot in SpotsAround)
            {
                // It is not busy and is valid
                if (_IsValidPath(this.Point, spot))
                {
                    spots.Add(spot);
                }
            }

            stopwatch.Stop();
            PrintRed("Elapsed time: " + stopwatch.Elapsed.Milliseconds + "ms");

            return spots;
        }

        #region Helper Methods For Finding Valid Spots
        private bool _IsFreedomOfMovement((int x, int y) from, (int x, int y) to, bool isBeetle = false)
        {
            (int x, int y) offset = (to.x - from.x, to.y - from.y);
            int index = SIDE_OFFSETS_LIST.IndexOf(offset); // direction we're going

            (int x, int y) leftOffset = index == 0 ? SIDE_OFFSETS_LIST[5] : SIDE_OFFSETS_LIST[index - 1];
            (int x, int y) rightOffset = index == 5 ? SIDE_OFFSETS_LIST[0] : SIDE_OFFSETS_LIST[index + 1];

            (int x, int y) peripheralLeftSpot = (from.x + leftOffset.x, from.y + leftOffset.y);
            (int x, int y) peripheralRightSpot = (from.x + rightOffset.x, from.y + rightOffset.y);

            bool peripheralLeftIsNotItself = peripheralLeftSpot.x != Point.x || peripheralLeftSpot.y != Point.y;
            bool peripheralRightIsNotItself = peripheralRightSpot.x != Point.x || peripheralRightSpot.y != Point.y;

            bool noneOfThePeripheralsIsItself = peripheralLeftIsNotItself && peripheralRightIsNotItself;
            bool onlyOneSpotIsOpen = _point_stack.ContainsKey(peripheralLeftSpot) ^ _point_stack.ContainsKey(peripheralRightSpot);
            bool checkThatTheOppositePeripheralExists = (!peripheralRightIsNotItself && _point_stack.ContainsKey(peripheralLeftSpot)) || (!peripheralLeftIsNotItself && _point_stack.ContainsKey(peripheralRightSpot));

            return noneOfThePeripheralsIsItself
                    // If it is a beetle, check it can crawl     OR Treat it as a normal piece
                    ? ((isBeetle && _point_stack.ContainsKey(to)) || onlyOneSpotIsOpen)
                    : checkThatTheOppositePeripheralExists;
        }

        private bool _DoesNotBreakHive((int x, int y) to)
        {
            Piece oldPieceSpot = new(ToString(), Point);
            Piece newPieceSpot = new(ToString(), to);
            _board._RemovePiece(this);
            _board._AddPiece(to, newPieceSpot);

            if (!_board.IsAllConnected())
            {
                // Put it back
                _board._RemovePiece(newPieceSpot);
                _board._AddPiece(oldPieceSpot.Point, oldPieceSpot);

                // it breaks the hive
                return false;
            }

            // Put it back
            _board._RemovePiece(newPieceSpot);
            _board._AddPiece(oldPieceSpot.Point, oldPieceSpot);
            return true;
        }

        private bool _IsValidPath((int x, int y) from, (int x, int y) to, bool isBeetle = false)
        {
            //      Only beetle can crawl on top of                 One Hive Rule                       Physically Fits
            return (isBeetle || !_point_stack.ContainsKey(to)) && _DoesNotBreakHive(to) && _IsFreedomOfMovement(from, to, isBeetle);
        }

        private void _SpiderDFS(ref List<(int x, int y)> positions, ref Dictionary<(int x, int y), bool> visited, (int x, int y) curSpot, int curDepth, int maxDepth)
        {
            if (curDepth == maxDepth)
            {
                // No one is at that position
                if (!_point_stack.ContainsKey(curSpot))
                {
                    visited[curSpot] = true;
                    positions.Add(curSpot);
                }
                return;
            }

            visited[curSpot] = true;

            foreach ((int x, int y) offset in SIDE_OFFSETS.Values)
            {
                (int x, int y) nextSpot = (curSpot.x + offset.x, curSpot.y + offset.y);
                bool hasNotBeenVisited = !visited.ContainsKey(nextSpot) || (visited.ContainsKey(nextSpot) && !visited[nextSpot]);
                if (hasNotBeenVisited && _IsValidPath(curSpot, nextSpot))
                {
                    _SpiderDFS(ref positions, ref visited, nextSpot, curDepth + 1, maxDepth);
                }
            }
        }

        #endregion
    }

    public enum Insect
    {
        QueenBee,
        Beetle,
        Grasshopper,
        Spider,
        Ant,
    }
}