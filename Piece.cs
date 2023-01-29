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
        public Color Color { get; set; }
        public int Number { get; set; }
        public Insect Insect { get; set; }
        public (int x, int y) Point { get; set; }
        public Dictionary<string, (int, int)> Sides { get {
            return new Dictionary<string, (int, int)>()
                {
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

        public List<(int, int)> GetMovingSpots(ref Board board)
        {
            return !board.IsAQueenSurrounded()
            ? Insect switch
            {
                Insect.Ant => _GetAntMovingSpots(ref board),
                Insect.Beetle => _GetBeetleMovingSpots(ref board),
                Insect.Grasshopper => _GetGrasshopperMovingSpots(ref board),
                Insect.Spider => _GetSpiderMovingSpots(ref board),
                Insect.QueenBee => _GetQueenMovingSpots(ref board),
                _ => throw new ArgumentException("This piece is not valid."),
            }
            // No moving spots because the game is over
            : new List<(int, int)>();
        }

        // This should be used for the pieces that have an expensive move generation–i.e., for now, only the Ant
        // TODO: Test whether adding this validation actually improves performance on other pieces
        // (which probably does not because it would be validating its surroundings twice) 
        public bool IsPinned(Board board, bool isBeetle = false)
        {
            foreach((int, int) side in Sides.Values)
            {
                // If there is at least one valid path
                if (_IsValidPath(ref board, Point, side, isBeetle))
                {
                    // It is not pinned
                    return false;
                }
            }

            // None of its side were a valid path,
            // So this piece is pinned
            return true;
        }


        private List<(int, int)> _GetAntMovingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int x, int y)> results = new();

            // Before getting all the open spots (which is an expensive computation)
            // Make sure this piece is not pinned
            if (!IsPinned(board))
            {
                List<(int x, int y)> spots = new();

                // Get all the open spots
                foreach (KeyValuePair<(int, int), Stack<Piece>> p in board.Pieces)
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
                for(int s = 0; s < spots.Count; ++s)
                {
                    for(int i = 0; i < _MANY_SIDES; ++i)
                    {
                        (int x, int y) adjacentSpot = (spots[s].x + SIDE_OFFSETS_ARRAY[i].x, spots[s].y + SIDE_OFFSETS_ARRAY[i].y);
                        if (!results.Contains(adjacentSpot) && spots.Contains(adjacentSpot) && _IsValidPath(ref board, spots[s], adjacentSpot))
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

        private List<(int, int)> _GetBeetleMovingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int, int)> validMoves = new();

            foreach ((int, int) side in Sides.Values)
            {
                // Keep the valid "paths" (from point -> to side)
                if (_IsValidPath(ref board, this.Point, side, true))
                {
                    validMoves.Add(side);
                }
            }

            stopwatch.Stop();
            PrintRed("Generating Beetle moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return validMoves;
        }

        private List<(int, int)> _GetGrasshopperMovingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int x, int y)> positions = new();
            // foreach ((int x, int y) sideOffset in SIDE_OFFSETS.Values)
            for (int s = 0; s < _MANY_SIDES; ++s)
            {
                (int x, int y) nextSpot = (this.Point.x + SIDE_OFFSETS_ARRAY[s].x, this.Point.y + SIDE_OFFSETS_ARRAY[s].y);
                bool firstIsValid = false;

                // Keep hopping over pieces
                while (board.Pieces.ContainsKey(nextSpot))
                {
                    // until you find a spot
                    nextSpot = (nextSpot.x + SIDE_OFFSETS_ARRAY[s].x, nextSpot.y + SIDE_OFFSETS_ARRAY[s].y);
                    firstIsValid = true;
                }

                if (firstIsValid)
                {
                    if (_DoesNotBreakHive(ref board, nextSpot))
                    {
                        positions.Add(nextSpot);
                    }
                }
            }

            stopwatch.Stop();
            PrintRed("Generating grasshoper moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return positions;
        }

        private List<(int, int)> _GetSpiderMovingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int x, int y)> positions = new();
            Dictionary<(int x, int y), bool> visited = new();
            // foreach ((int x, int y) side in SpotsAround)
            for (int s = 0; s < SpotsAround.Count; ++s)
            {
                bool hasNotBeenVisited = !visited.ContainsKey(SpotsAround[s]) || (visited.ContainsKey(SpotsAround[s]) && !visited[SpotsAround[s]]);
                if (hasNotBeenVisited && _IsValidPath(ref board, Point, SpotsAround[s]))
                {
                    _SpiderDFS(ref board, ref positions, ref visited, SpotsAround[s], 1, _SPIDER_MAX_STEP_COUNT);
                }
            }

            stopwatch.Stop();
            PrintRed("Generating spider moves took: " + stopwatch.Elapsed.Milliseconds + "ms");

            return positions;
        }

        private List<(int, int)> _GetQueenMovingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<(int, int)> spots = new();
            // foreach ((int, int) spot in SpotsAround)
            for (int s = 0; s < SpotsAround.Count; ++s)
            {
                // It is not busy and is valid
                if (_IsValidPath(ref board, this.Point, SpotsAround[s]))
                {
                    spots.Add(SpotsAround[s]);
                }
            }

            stopwatch.Stop();
            PrintRed("Elapsed time: " + stopwatch.Elapsed.Milliseconds + "ms");

            return spots;
        }

        #region Helper Methods For Finding Valid Spots
        private bool _IsFreedomOfMovement(ref Board board, (int x, int y) from, (int x, int y) to, bool isBeetle = false)
        {
            (int offsetX, int offsetY) = (to.x - from.x, to.y - from.y);
            // int index = SIDE_OFFSETS_LIST.IndexOf(offset); // direction we're going

            int index = 0;
            for (; index < 6; ++index)
            {
                if (SIDE_OFFSETS_ARRAY[index].x == offsetX && SIDE_OFFSETS_ARRAY[index].y == offsetY)
                {
                    // direction we are going
                    break;
                }
            }

            (int x, int y) leftOffset = index == 0 ? SIDE_OFFSETS_ARRAY[5] : SIDE_OFFSETS_ARRAY[index - 1];
            (int x, int y) rightOffset = index == 5 ? SIDE_OFFSETS_ARRAY[0] : SIDE_OFFSETS_ARRAY[index + 1];

            (int x, int y) peripheralLeftSpot = (from.x + leftOffset.x, from.y + leftOffset.y);
            (int x, int y) peripheralRightSpot = (from.x + rightOffset.x, from.y + rightOffset.y);

            bool peripheralLeftIsNotItself = peripheralLeftSpot.x != Point.x || peripheralLeftSpot.y != Point.y;
            bool peripheralRightIsNotItself = peripheralRightSpot.x != Point.x || peripheralRightSpot.y != Point.y;

            bool noneOfThePeripheralsIsItself = peripheralLeftIsNotItself && peripheralRightIsNotItself;
            bool onlyOneSpotIsOpen = board.Pieces.ContainsKey(peripheralLeftSpot) ^ board.Pieces.ContainsKey(peripheralRightSpot);
            //                                              Right is itself             Someone MUST be there on the left             OR    Left is itself                    Someone MUST be there on the right 
            bool checkThatTheOppositePeripheralExists = (!peripheralRightIsNotItself && board.Pieces.ContainsKey(peripheralLeftSpot)) || (!peripheralLeftIsNotItself && board.Pieces.ContainsKey(peripheralRightSpot));

            return noneOfThePeripheralsIsItself
                    // If it is a beetle, check it can crawl     OR Treat it as a normal piece
                    ? ((isBeetle && board.Pieces.ContainsKey(to)) || onlyOneSpotIsOpen)
                    : checkThatTheOppositePeripheralExists;
        }

        private bool _DoesNotBreakHive(ref Board board, (int x, int y) to)
        {
            Piece oldPieceSpot = new(ToString(), Point);
            Piece newPieceSpot = new(ToString(), to);
            board._RemovePiece(this);

            if (!board.IsAllConnected())
            {
                // put it back
                board._AddPiece(oldPieceSpot.Point, oldPieceSpot);

                // this move breaks the hive
                return false;
            }
            else
            {
                // Temporarily move it the new spot
                board._AddPiece(to, newPieceSpot);
                if (!board.IsAllConnected())
                {
                    // Put it back
                    board._RemovePiece(newPieceSpot);
                    board._AddPiece(oldPieceSpot.Point, oldPieceSpot);

                    // this move breaks the hive
                    return false;
                }
                board._RemovePiece(newPieceSpot);
            }

            // Put it back
            board._AddPiece(oldPieceSpot.Point, oldPieceSpot);

            // this move does not break the hive
            return true;
        }

        private bool _IsValidPath(ref Board board, (int x, int y) from, (int x, int y) to, bool isBeetle = false)
        {
            //      Only beetle can crawl on top of                 One Hive Rule                       Physically Fits
            return (isBeetle || !board.Pieces.ContainsKey(to)) && _DoesNotBreakHive(ref board, to) && _IsFreedomOfMovement(ref board, from, to, isBeetle);
        }

        private void _SpiderDFS(ref Board board, ref List<(int x, int y)> positions, ref Dictionary<(int x, int y), bool> visited, (int x, int y) curSpot, int curDepth, int maxDepth)
        {
            if (curDepth == maxDepth)
            {
                // No one is at that position
                if (!board.Pieces.ContainsKey(curSpot))
                {
                    visited[curSpot] = true;
                    positions.Add(curSpot);
                }
                return;
            }

            visited[curSpot] = true;

            for (int i = 0; i < 6; ++i)
            {
                (int x, int y) nextSpot = (curSpot.x + SIDE_OFFSETS_ARRAY[i].x, curSpot.y + SIDE_OFFSETS_ARRAY[i].y);
                bool hasNotBeenVisited = !visited.ContainsKey(nextSpot) || (visited.ContainsKey(nextSpot) && !visited[nextSpot]);
                if (hasNotBeenVisited && _IsValidPath(ref board, curSpot, nextSpot))
                {
                    _SpiderDFS(ref board, ref positions, ref visited, nextSpot, curDepth + 1, maxDepth);
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