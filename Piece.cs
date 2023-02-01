using System.Diagnostics;
using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public class Piece
    {
        private const int _SPIDER_MAX_STEP_COUNT = 3;
        private readonly string _piece;
        public Color Color { get; set; }
        public int Number { get; set; }
        public bool IsOnBoard { get; set; }
        public Insect Insect { get; set; }
        public (int x, int y) Point { get; set; }
        public Dictionary<(int, int), string> Sides { get {
            return new Dictionary<(int, int), string>()
                {
                    { (Point.x + SIDE_OFFSETS_ARRAY[0].x, Point.y + SIDE_OFFSETS_ARRAY[0].y), "NT" },  // [0] North
                    { (Point.x + SIDE_OFFSETS_ARRAY[1].x, Point.y + SIDE_OFFSETS_ARRAY[1].y), "NW" },  // [1] Northwest
                    { (Point.x + SIDE_OFFSETS_ARRAY[2].x, Point.y + SIDE_OFFSETS_ARRAY[2].y), "SW" },  // [2] Southwest
                    { (Point.x + SIDE_OFFSETS_ARRAY[3].x, Point.y + SIDE_OFFSETS_ARRAY[3].y), "ST" },  // [3] South
                    { (Point.x + SIDE_OFFSETS_ARRAY[4].x, Point.y + SIDE_OFFSETS_ARRAY[4].y), "SE" },  // [4] Southeast
                    { (Point.x + SIDE_OFFSETS_ARRAY[5].x, Point.y + SIDE_OFFSETS_ARRAY[5].y), "NE" },  // [5] Northeast
                };
        } }

        public List<(int, int)> Neighbors { get; set; }
        public List<(int, int)> SpotsAround { get { return Sides.Where(side => !Neighbors.Contains(side.Key)).Select(s => s.Key).ToList(); } }
        public int ManyNeighbors { get{ return Neighbors.Count; } }
        public override string ToString() { return _piece; }
        public bool IsSurrounded() { return Neighbors.Count == MANY_SIDES; }
        public Piece(string piece)
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
            Neighbors = new List<(int, int)>();
            Neighbors.EnsureCapacity(MANY_SIDES);
            Point = (0, 0);
            IsOnBoard = false;
        }

        public override bool Equals(object? obj)
        {
            return obj is Piece p && _piece == p._piece;
        }

        public List<(int, int)> GetMovingSpots(ref Board board)
        {
            // // If the queen has not been played
            if (!board.IsOnBoard($"{char.ToLower(Color.ToString()[0])}Q1"))
            {
                // This piece cannot move
                return new List<(int, int)>();
            }
            // If the piece about to be placed is the fourth one, and the queen has not been played
            else if (board.GetRefPiecesByColor(Color).Count == 3 && !board.IsOnBoard($"{char.ToLower(Color.ToString()[0])}Q1"))
            {
                // then the queen has to be played
                return _GetQueenMovingSpots(ref board);
            }
            else
            {
            // Return this piece's valid moving spots
                return Insect switch
                {
                    Insect.Ant => _GetAntMovingSpots(ref board),
                    Insect.Beetle => _GetBeetleMovingSpots(ref board),
                    Insect.Grasshopper => _GetGrasshopperMovingSpots(ref board),
                    Insect.Spider => _GetSpiderMovingSpots(ref board),
                    Insect.QueenBee => _GetQueenMovingSpots(ref board),
                    _ => throw new ArgumentException("This piece is not valid."),
                };
            }
        }

        // Maybe this should become a static method in the `Board` class with a `Color` parameter
        // That way the `Board` tells `Manager` where `Color` can play?
        public List<(int, int)> GetPlacingSpots(ref Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            List<(int, int)> positions = new();

            // If nothing has been played
            if (board.IsEmpty())
            {
                // Origin is the only first valid placing spot
                return new List<(int, int)>(){(0, 0)};
            }
            // If there is only one piece on the board
            else if (board.Pieces.Count == 1)
            {
                // the available placing spots will be the such piece's surrounding spots
                return board.Pieces[(0, 0)].Peek().SpotsAround;
            }
            // Make sure the game is still going
            else if (!board.IsAQueenSurrounded())
            {
                // from here on out, only the spots that do not neighbor an opponent are valid  

                // iterate through the current player's pieces on the board
                foreach (Piece? piece in board.GetRefPiecesByColor(this.Color))
                {
                    if (piece != null)
                    {
                        // iterate through this piece's available spots
                        foreach ((int, int) spot in piece.SpotsAround)
                        {
                            //      Not been visited        It is not neighboring an opponent
                            if (!positions.Contains(spot) && !_HasOpponentNeighbor(spot, board.Pieces))
                            {
                                positions.Add(spot);
                            }
                        }
                    }
                }
            }

            stopwatch.Stop();
            PrintRed($"Generating Available Spots for Player {Color} took: {stopwatch.Elapsed.Milliseconds} ms");

            return positions;
        }

        private bool _HasOpponentNeighbor((int x, int y) point, Dictionary<(int, int), Stack<Piece>> pieces)
        {
            // foreach ((int, int) side in SIDE_OFFSETS.Values)
            for (int i = 0; i < MANY_SIDES; ++i)
            {
                (int, int) potentialOpponentNeighborPosition = (point.x + SIDE_OFFSETS_ARRAY[i].x, point.y + SIDE_OFFSETS_ARRAY[i].y);
                // If piece is on the board                                     And Is not the same color as the piece that is about to be placed
                // if (pieces.ContainsKey(potentialOpponentNeighborPosition) && pieces[potentialOpponentNeighborPosition].Peek().Color != this.Color)
                if (pieces.ContainsKey(potentialOpponentNeighborPosition) && pieces[potentialOpponentNeighborPosition].TryPeek(out Piece topPiece) && topPiece.Color != this.Color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // Checked each side, and no opponent's pieces were found
            return false;
        }



        // This should be used for the pieces that have an expensive move generation–i.e., for now, only the Ant
        // TODO: Test whether adding this validation actually improves performance on other pieces
        // (which probably does not because it would be validating its surroundings twice) 
        public bool IsPinned(Board board, bool isBeetle = false)
        {
            if (IsSurrounded())
                return true;

            foreach((int, int) side in Sides.Keys)
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

            // Before getting all the open spots (which could be an expensive computation)
            // Make sure this piece is not pinned
            // Benchmark whether this actually speeds up performance or not
            if (!IsPinned(board))
            {
                List<(int x, int y)> spots = new();

                // Get all the open spots
                foreach (KeyValuePair<(int, int), Stack<Piece>> p in board.Pieces)
                {
                    if (p.Value.TryPeek(out Piece topPiece))
                    {
                        foreach ((int, int) spot in topPiece.SpotsAround)
                        {
                            if (!spots.Contains(spot))
                            {
                                spots.Add(spot);
                            }
                        }
                    }
                }

                // Validate each moving from `spot` to `adjacentSpot`
                for(int s = 0; s < spots.Count; ++s)
                {
                    for(int i = 0; i < MANY_SIDES; ++i)
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

            foreach ((int, int) side in Sides.Keys)
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
            for (int s = 0; s < MANY_SIDES; ++s)
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
            (int offsetX, int offsetY) offset= (to.x - from.x, to.y - from.y);

            int index = Array.IndexOf(SIDE_OFFSETS_ARRAY, offset); // direction we're going

            // int index = 0;
            // for (; index < MANY_SIDES; ++index)
            // {
            //     if (SIDE_OFFSETS_ARRAY[index].x == offsetX && SIDE_OFFSETS_ARRAY[index].y == offsetY)
            //     {
            //         // direction we are going
            //         break;
            //     }
            // }

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
                    // If it is a beetle, check it can crawl on   OR get off of piece at to/from point   OR Treat it as a normal piece
                    ? ((isBeetle && (board.Pieces.ContainsKey(to) || board.Pieces.ContainsKey(from))) || onlyOneSpotIsOpen)
                    : checkThatTheOppositePeripheralExists;
        }

        private bool _DoesNotBreakHive(ref Board board, (int x, int y) to)
        {
            Piece oldPieceSpot = new(ToString())
            {
                Point = Point
            };

            Piece newPieceSpot = new(ToString())
            {
                Point = to
            };

            board._RemovePiece(this);

            if (!board.IsAllConnected())
            {
                // place it back
                board.AddPiece(oldPieceSpot, oldPieceSpot.Point);

                // this move breaks the hive
                return false;
            }
            else
            {
                // Temporarily place this piece to the `to` point
                board.AddPiece(newPieceSpot, to);
                if (!board.IsAllConnected())
                {
                    board._RemovePiece(newPieceSpot);
                    // place it back
                    board.AddPiece(oldPieceSpot, oldPieceSpot.Point);

                    // this move breaks the hive
                    return false;
                }
                board._RemovePiece(newPieceSpot);
            }

            // Place it back
            board.AddPiece(oldPieceSpot, oldPieceSpot.Point);

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

            for (int i = 0; i < MANY_SIDES; ++i)
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