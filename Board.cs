#pragma warning disable IDE1006 // Private members naming style
using static HiveCore.Utils;
using System.Diagnostics;

namespace HiveCore
{
    public class Board
    {
        public Dictionary<(int, int), Stack<Piece>> Pieces;
        public Dictionary<string, Piece> WhitePieces { get; set; }
        public Dictionary<string, Piece> BlackPieces { get; set; }
        public HashSet<string> WhitePiecesKeys { get; set; }
        public HashSet<string> BlackPiecesKeys { get; set; }

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            Pieces = new Dictionary<(int, int), Stack<Piece>>();
            Pieces.EnsureCapacity(22);

            WhitePieces = new Dictionary<string, Piece>()
            {
                {"wQ1", new Piece("wQ1")},
                {"wA1", new Piece("wA1")},
                {"wA2", new Piece("wA2")},
                {"wA3", new Piece("wA3")},
                {"wB1", new Piece("wB1")},
                {"wB2", new Piece("wB2")},
                {"wG1", new Piece("wG1")},
                {"wG2", new Piece("wG2")},
                {"wG3", new Piece("wG3")},
                {"wS1", new Piece("wS1")},
                {"wS2", new Piece("wS2")},
            };

            WhitePiecesKeys = new HashSet<string>()
            {
                "wQ1",
                "wA1",
                "wA2",
                "wA3",
                "wB1",
                "wB2",
                "wG1",
                "wG2",
                "wG3",
                "wS1",
                "wS2",
            };

            BlackPieces = new Dictionary<string, Piece>()
            {
                {"bQ1", new Piece("bQ1")},
                {"bA1", new Piece("bA1")},
                {"bA2", new Piece("bA2")},
                {"bA3", new Piece("bA3")},
                {"bB1", new Piece("bB1")},
                {"bB2", new Piece("bB2")},
                {"bG1", new Piece("bG1")},
                {"bG2", new Piece("bG2")},
                {"bG3", new Piece("bG3")},
                {"bS1", new Piece("bS1")},
                {"bS2", new Piece("bS2")},
            };

            BlackPiecesKeys = new HashSet<string>()
            {
                "bQ1",
                "bA1",
                "bA2",
                "bA3",
                "bB1",
                "bB2",
                "bG1",
                "bG2",
                "bG3",
                "bS1",
                "bS2",
            };
        }

        public Board Clone()
        {
            Dictionary<string, Piece> clonedWhitePieces = this.WhitePieces.ToDictionary(first => first.Key, second => second.Value.Clone());
            Dictionary<string, Piece> clonedBlackPieces = this.BlackPieces.ToDictionary(first => first.Key, second => second.Value.Clone());
            Dictionary<(int, int), Stack<Piece>> clonedPieces = new();

            foreach ((int, int) point in Pieces.Keys)
            {
                clonedPieces[point] = new Stack<Piece>();
                foreach (Piece piece in Pieces[point])
                {
                    clonedPieces[point].Push(piece.Color == Color.Black ? clonedBlackPieces[piece.ToString()] : clonedWhitePieces[piece.ToString()]);
                }
            }

            return new Board()
            {
                WhitePieces = clonedWhitePieces,
                BlackPieces = clonedBlackPieces,
                Pieces = clonedPieces,
            };
        }

        /*************************************************************************
        **************************************************************************
                                            AI
        *************************************************************************
        *************************************************************************/
        public bool AIMove(Color color)
        {
            int min = 1000000;
            int max = -min;
            (int eval, (Piece piece, (int, int) to)) = _Search(color, max, min, _MAX_DEPTH_TREE_SEARCH);
            MovePiece(piece, to);
            return true;
        }

        // Based on the original pseudocode and this guy's: https://www.youtube.com/watch?v=U4ogK0MIzqk&t=1005s
        private (int eval, (Piece piece, (int, int) to) move) _Search(Color whoseTurn, int alpha, int beta, int depth)
        {
            (Piece, (int, int)) myMove = default;
            if (depth == 0)
            {
                return (new Random().Next(-1000010, 1000010), myMove);
            }

            HashSet<(Piece, (int, int))> moves = _GenerateMovesFor(whoseTurn);

            if (moves.Count == 0)
            {
                if (IsGameOver())
                {
                    return (-1000000, myMove);
                }
                return (0, myMove);
            }

            foreach ((Piece curPiece, (int, int) to) in moves)
            {
                (int, int) oldPoint = curPiece.Point;
                Piece piece = curPiece.Color == Color.Black ? BlackPieces[curPiece.ToString()] : WhitePieces[curPiece.ToString()];
                // make move
                MovePiece(piece, to);
                (int evaluation, (Piece, (int, int)) move) = _Search(whoseTurn == Color.Black ? Color.White : Color.Black, -beta, -alpha, depth - 1);
                evaluation = -evaluation;
                // put it back
                MovePiece(piece, oldPoint);

                if (evaluation >= beta)
                {
                    return (beta, myMove);
                }
                alpha = Math.Max(alpha, evaluation);
                myMove = (piece, to);
            }
            return (alpha, myMove);
        }

#region AI Method Helpers
        private HashSet<(Piece, (int, int))> _GenerateMovesFor(Color curPlayer)
        {
            HashSet<(Piece, (int, int))> moves = new();

            // Force the player to play their queen on their 4th turn
            string playersQueen = $"{char.ToLower(curPlayer.ToString()[0])}Q1";
            if (_GetManyPiecesPlayedBy(curPlayer) == 3 && !_IsPieceOnBoard(playersQueen))
            {
                foreach ((int, int) spot in GetPlacingSpotsFor(curPlayer))
                {
                    moves.Add((curPlayer == Color.Black ? BlackPieces[playersQueen].Clone() : WhitePieces[playersQueen].Clone(), spot));
                }
                return moves;
            }

            // Idea for forcing the queen to not be played on the first turn:
            // Maybe remove it temporarily, and put it back until the `manyPiecesPlayedByCurPlayer > 1`?
            foreach (Piece piece in curPlayer == Color.Black ? BlackPieces.Values : WhitePieces.Values)
            {
                if (!piece.IsSurrounded)
                {
                    if (piece.IsOnBoard)
                    {
                        foreach ((int, int) spot in GetMovingSpotsFor(piece))
                        {
                            moves.Add((piece.Clone(), spot));
                        }
                    }
                    else
                    {
                        foreach ((int, int) spot in GetPlacingSpotsFor(piece.Color))
                        {
                            moves.Add((piece.Clone(), spot));
                        }
                    }
                }
            }
            return moves;
        }

        public void SetState(Board board)
        {
            Pieces = board.Pieces;
            WhitePieces = board.WhitePieces;
            BlackPieces = board.BlackPieces;
        }

        public bool IsGameOver()
        {
            return BlackPieces["bQ1"].IsSurrounded || WhitePieces["wQ1"].IsSurrounded;
        }
#endregion

        public void MovePiece(Piece piece, (int, int) to)
        {
            if (piece == null)
            {
                return;
            }

            if (piece.IsOnBoard)
            {
                _RemovePiece(ref piece);
            }

            // (1, 2) is a signal of "putting back"
            // meaning that it does not belong to the board
            if (to != (1, 2))
            {
                _PlacePiece(ref piece, to);
            }
        }
        public HashSet<(int, int)> GetPlacingSpotsFor(Color color)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            HashSet<(int, int)> positions = new();

            // If nothing has been played
            if (Pieces.Count == 0)
            {
                // Origin is the only first valid placing spot
                return new HashSet<(int, int)>(){(0, 0)};
            }
            // If there is only one piece on the board
            else if (Pieces.Count == 1)
            {
                // the available placing spots will be the such piece's surrounding spots
                return Pieces[(0, 0)].Peek().Sides;
            }
            // Make sure the game is still going (add isGameOver or some form of validation)
            else if (!IsGameOver())
            {
                // from here on out, only the spots that do not neighbor an opponent are valid  

                // iterate through the current player's pieces on the board
                foreach (string key in color == Color.Black ? BlackPiecesKeys : WhitePiecesKeys)
                {
                    Piece piece = color == Color.Black ? BlackPieces[key] : WhitePieces[key];
                    if (piece.IsOnBoard)
                    {
                        // iterate through this piece's available spots
                        foreach ((int, int) spot in piece.OpenSpotsAround)
                        {
                            //      Not been visited        It is not neighboring an opponent
                            if (!positions.Contains(spot) && !_HasOpponentNeighbor(spot, color))
                            {
                                positions.Add(spot);
                            }
                        }
                    }
                }
            }
            stopwatch.Stop();
            PrintRed($"Generating Available Placing Spots for {color} Player took: {stopwatch.Elapsed.TotalMilliseconds} ms");
            return positions;
        }

        public HashSet<(int, int)> GetMovingSpotsFor(Piece piece)
        {
            Color color = piece.Color;
            string queen = $"{char.ToLower(color.ToString()[0])}Q1";
            // If the piece about to be played is not the queen
            if (!piece.ToString().Equals(queen)
            // And the queen has not been played
            && !_IsPieceOnBoard(queen))
            {
                // this piece cannot move
                return new HashSet<(int, int)>();
            }

            // If the piece about to be played is the fourth one
            else if (_GetManyPiecesPlayedBy(piece.Color) == 3
            // AND this piece is not the queen
            && !piece.ToString().Equals(queen)
            // AND the queen has not been played
            && !_IsPieceOnBoard(queen)
            )
            {
                Console.WriteLine("returning only placing spots for this piece (but not necessarily for the queen)");
                // then you must place a piece,
                // and such piece MUST be the queen (this may need to be enforced by GameManager)
                return GetPlacingSpotsFor(piece.Color);
            }
            // if it is not the top piece
            else if (!Pieces[piece.Point].Peek().ToString().Equals(piece.ToString()))
            {
                // This piece cannot move
                return new HashSet<(int, int)>();
            }
            else
            {
                Console.WriteLine($"returning the appropriate moves for {piece}");
                // Return this piece's valid moving spots
                return piece.Insect switch
                {
                    Insect.Ant => _GetAntMovingSpots(ref piece),
                    Insect.Beetle => _GetBeetleMovingSpots(ref piece),
                    Insect.Grasshopper => _GetGrasshopperMovingSpots(ref piece),
                    Insect.Spider => _GetSpiderMovingSpots(ref piece),
                    Insect.QueenBee => _GetQueenMovingSpots(ref piece),
                    _ => throw new ArgumentException($"{piece} is an invalid piece."),
                };
            }
        }

#region Placing/Moving on the Board helper methods

        private void _PlacePiece(ref Piece piece, (int, int) to)
        {
            if (piece.Insect == Insect.Beetle && Pieces.ContainsKey(to))
            {
                Pieces[to].Push(piece);
            }
            else
            {
                Pieces[to] = new Stack<Piece>();
                Pieces[to].Push(piece);
            }
            piece.Point = to;
            piece.IsOnBoard = true;
            _UpdateNeighborsAt(to);
        }

        private void _RemovePiece(ref Piece piece)
        {
            (int, int) removingSpot = piece.Point;
            Pieces[removingSpot].Pop();
            if (Pieces[removingSpot].Count == 0)
            {
                Pieces.Remove(removingSpot);
            }
            piece.SetToDefault();
            _UpdateNeighborsAt(removingSpot);
        }

        private void _PopulateNeighborsFor(Piece piece)
        {
            piece.Neighbors.Clear();
            foreach ((int, int) sidePoint in piece.Sides)
            {
                // bool IsNeighbor = (point % side == (0, 0));
                // (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighborExists = Pieces.ContainsKey(sidePoint);
                if (neighborExists)
                {
                    piece.Neighbors.Add(sidePoint);
                }
            }
        }
        private void _UpdateNeighborsAt((int x, int y) point)
        {
            // if this is a busy spot
            if (Pieces.ContainsKey(point))
            {
                // for each piece at this spot
                foreach (Piece piece in Pieces[point])
                {
                    // re-populate its neighbors
                    _PopulateNeighborsFor(piece);

                    // go around its new neighbouring spots
                    foreach ((int, int) neighborSpot in piece.Neighbors)
                    {
                        // and let the pieces at this spot know 
                        foreach (Piece neighborPiece in Pieces[neighborSpot])
                        {
                            // that `piece` is their new neighbor
                            if (!neighborPiece.Neighbors.Contains(piece.Point))
                            {
                                neighborPiece.Neighbors.Add(piece.Point);
                            }
                            piece.IsSurrounded = piece.Neighbors.Count == MANY_SIDES;
                            // or update piece.IsPinned as soon as a piece is added/removed?
                        }
                    }
                }
            }
            // this is an open spot
            else
            {
                // so for every neighboring point
                for (int i = 0; i < MANY_SIDES; ++i)
                {
                    (int, int) neighborPoint = (point.x + SIDE_OFFSETS_ARRAY[i].x, point.y + SIDE_OFFSETS_ARRAY[i].y);

                    // if there are pieces to update
                    if (Pieces.ContainsKey(neighborPoint))
                    {
                        // for each piece at this spot
                        foreach (Piece piece in Pieces[neighborPoint])
                        {
                            // let them know that `point` is now an open spot
                            piece.Neighbors.Remove(point);
                            piece.IsSurrounded = piece.Neighbors.Count == MANY_SIDES;
                        }
                    }
                }
            }
        }

        private bool _IsPieceOnBoard(string piece)
        {
            return piece[0] == 'b' ? BlackPieces[piece].IsOnBoard : WhitePieces[piece].IsOnBoard;
        }

        private int _GetManyPiecesPlayedBy(Color color)
        {
            return color == Color.Black
                ? BlackPieces.Values.Count(piece => piece.IsOnBoard)
                : WhitePieces.Values.Count(piece => piece.IsOnBoard);
        }
#endregion

#region Each Moving Spot Getter for `Piece`
        private HashSet<(int, int)> _GetAntMovingSpots(ref Piece piece)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            HashSet<(int x, int y)> positions = new();
            (int x, int y) oldAntPosition = piece.Point;
            _AntDFS(ref piece, ref positions, piece.Point);

            // Because the last point it found is where this piece is now positioned
            // Move it back to where it was
            MovePiece(piece, oldAntPosition);

            // itself should not be included
            positions.Remove(oldAntPosition);

            stopwatch.Stop();
            PrintRed("Generating Ant Moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
            return positions;
        }

        private void _AntDFS(ref Piece piece, ref HashSet<(int x, int y)> positions, (int x, int y) curSpot)
        {
            for (int i = 0; i < MANY_SIDES; ++i)
            {
                (int x, int y) nextSpot = (curSpot.x + SIDE_OFFSETS_ARRAY[i].x, curSpot.y + SIDE_OFFSETS_ARRAY[i].y);
                // If it has not been visited    AND Is a valid move
                if (!positions.Contains(nextSpot) && _IsValidMove(ref piece, curSpot, nextSpot))
                {
                    positions.Add(nextSpot);
                    // This move is important because it needs to update its neighbors
                    // so that it can later be appropriately validated by the _IsOneHive
                    MovePiece(piece, nextSpot);
                    _AntDFS(ref piece, ref positions, nextSpot);
                }
            }
        }

        private HashSet<(int, int)> _GetBeetleMovingSpots(ref Piece piece)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            HashSet<(int, int)> validMoves = new();
            foreach ((int, int) side in piece.Sides)
            {
                // Because the beetle can only go around, and on top of other pieces
                // just check if it can move from its current point -> side 
                if (_IsValidMove(ref piece, piece.Point, side, false, true))
                {
                    validMoves.Add(side);
                }
            }
            stopwatch.Stop();
            PrintRed("Generating Beetle moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
            return validMoves;
        }

        private HashSet<(int, int)> _GetGrasshopperMovingSpots(ref Piece piece)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            HashSet<(int x, int y)> positions = new();
            for (int s = 0; s < MANY_SIDES; ++s)
            {
                (int x, int y) nextSpot = (piece.Point.x + SIDE_OFFSETS_ARRAY[s].x, piece.Point.y + SIDE_OFFSETS_ARRAY[s].y);
                bool firstIsValid = false;

                // Keep hopping over pieces
                while (Pieces.ContainsKey(nextSpot))
                {
                    // until you find a spot
                    nextSpot = (nextSpot.x + SIDE_OFFSETS_ARRAY[s].x, nextSpot.y + SIDE_OFFSETS_ARRAY[s].y);
                    firstIsValid = true;
                }

                if (firstIsValid && _IsOneHive(ref piece, nextSpot, true))
                {
                    positions.Add(nextSpot);
                }
            }
            stopwatch.Stop();
            PrintRed("Generating grasshoper moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
            return positions;
        }

        private HashSet<(int, int)> _GetSpiderMovingSpots(ref Piece piece)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            HashSet<(int x, int y)> positions = new();
            HashSet<(int x, int y)> visited = new();
            (int, int) oldSpiderPosition = piece.Point;
            _SpiderDFS(ref piece, ref positions, ref visited, piece.Point, 0, _SPIDER_MAX_STEP_COUNT);

            // Because the last point it found is where this piece is now positioned
            // Move it back to where it was
            MovePiece(piece, oldSpiderPosition);
            stopwatch.Stop();
            PrintRed("Generating spider moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
            return positions;
        }

        private void _SpiderDFS(ref Piece piece, ref HashSet<(int x, int y)> positions, ref HashSet<(int x, int y)> visited, (int x, int y) curSpot, int curDepth, int maxDepth)
        {
            if (curDepth == maxDepth)
            {
                visited.Add(curSpot);
                positions.Add(curSpot);
                return;
            }
            visited.Add(curSpot);
            for (int i = 0; i < MANY_SIDES; ++i)
            {
                (int x, int y) nextSpot = (curSpot.x + SIDE_OFFSETS_ARRAY[i].x, curSpot.y + SIDE_OFFSETS_ARRAY[i].y);

                // TO BENCHMARK: Because DFS may run into itself at one point, maybe this should be validated
                // if (!visited.Contains(nextSpot) && (piece.Point != nextSpot) && _IsValidMove(ref piece, curSpot, nextSpot))

                // It has not been visited      AND Is a valid move
                if (!visited.Contains(nextSpot) && _IsValidMove(ref piece, curSpot, nextSpot))
                {
                    // This move is important because it needs to update its neighbors
                    // so that it can later be appropriately validated by the _IsOneHive
                    MovePiece(piece, nextSpot);
                    _SpiderDFS(ref piece, ref positions, ref visited, nextSpot, curDepth + 1, maxDepth);
                }
                MovePiece(piece, curSpot);
            }
        }

        private HashSet<(int, int)> _GetQueenMovingSpots(ref Piece piece)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            HashSet<(int, int)> spots = new();
            foreach ((int, int) openSpot in piece.OpenSpotsAround)
            {
                // Since the queen can only go around its open spots,
                // only keep such valid open spots around it
                if (_IsValidMove(ref piece, piece.Point, openSpot))
                {
                    spots.Add(openSpot);
                }
            }
            stopwatch.Stop();
            PrintRed("Elapsed time: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
            return spots;
        }
#endregion

#region Helper Methods For Finding Valid Moves
        private bool _HasOpponentNeighbor((int x, int y) point, Color color)
        {
            // foreach ((int, int) side in SIDE_OFFSETS.Values)
            for (int i = 0; i < MANY_SIDES; ++i)
            {
                (int, int) potentialOpponentNeighborPosition = (point.x + SIDE_OFFSETS_ARRAY[i].x, point.y + SIDE_OFFSETS_ARRAY[i].y);
                // if (Pieces.ContainsKey(potentialOpponentNeighborPosition) && Pieces[potentialOpponentNeighborPosition].TryPeek(out Piece topPiece) && topPiece.Color != color)
                // If piece is on the board                                     And Is not the same color as the piece that is about to be placed
                if (Pieces.ContainsKey(potentialOpponentNeighborPosition) && Pieces[potentialOpponentNeighborPosition].Peek().Color != color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }
            // Checked each side, and no opponent's pieces were found
            return false;
        }

        private bool _IsValidMove(ref Piece piece, (int x, int y) from, (int x, int y) to, bool isGrasshopper = false, bool isBeetle = false)
        {
            //      Only beetle can crawl on top of                 One Hive Rule                       Physically Fits
            return (isBeetle || !Pieces.ContainsKey(to)) && _IsOneHive(ref piece, to, isGrasshopper, isBeetle) && _IsFreedomOfMovement(ref piece, from, to, isBeetle);
        }

        private bool _IsFreedomOfMovement(ref Piece piece, (int x, int y) from, (int x, int y) to, bool isBeetle = false)
        {
            (int offsetX, int offsetY) offset = (to.x - from.x, to.y - from.y);

            int index = Array.IndexOf(SIDE_OFFSETS_ARRAY, offset); // direction we're going

            (int x, int y) leftOffset = index == 0 ? SIDE_OFFSETS_ARRAY[5] : SIDE_OFFSETS_ARRAY[index - 1];
            (int x, int y) rightOffset = index == 5 ? SIDE_OFFSETS_ARRAY[0] : SIDE_OFFSETS_ARRAY[index + 1];

            (int x, int y) peripheralLeftSpot = (from.x + leftOffset.x, from.y + leftOffset.y);
            (int x, int y) peripheralRightSpot = (from.x + rightOffset.x, from.y + rightOffset.y);

            bool peripheralLeftIsNotItself = peripheralLeftSpot.x != piece.Point.x || peripheralLeftSpot.y != piece.Point.y;
            bool peripheralRightIsNotItself = peripheralRightSpot.x != piece.Point.x || peripheralRightSpot.y != piece.Point.y;

            if(isBeetle){
                int LeftStackCount = Pieces.ContainsKey(peripheralLeftSpot) ? Pieces[peripheralLeftSpot].Count : 0;
                int RightStackCount = Pieces.ContainsKey(peripheralRightSpot) ? Pieces[peripheralRightSpot].Count : 0;
                int FromStackCount = Pieces.ContainsKey(from) ? Pieces[from].Count : 0;
                int ToStackCount = Pieces.ContainsKey(to) ? Pieces[to].Count : 0;
                return FromStackCount >= (ToStackCount + 1)
                ? !(LeftStackCount >= FromStackCount && RightStackCount >= FromStackCount)
                : !(LeftStackCount >= ToStackCount && RightStackCount >= ToStackCount);
            }

             bool noneOfThePeripheralsIsItself = peripheralLeftIsNotItself && peripheralRightIsNotItself;
             bool onlyOneSpotIsOpen = Pieces.ContainsKey(peripheralLeftSpot) ^ Pieces.ContainsKey(peripheralRightSpot);
             //                                              Right is itself             Someone MUST be there on the left             OR    Left is itself                    Someone MUST be there on the right 
             bool checkThatTheOppositePeripheralExists = (!peripheralRightIsNotItself && Pieces.ContainsKey(peripheralLeftSpot)) || (!peripheralLeftIsNotItself && Pieces.ContainsKey(peripheralRightSpot));

            return noneOfThePeripheralsIsItself
                    // If it is a beetle, check it can crawl on   OR get off of piece at to/from point   OR Treat it as a normal piece
                    // ? ((isBeetle && (board.Pieces.ContainsKey(to) || board.Pieces.ContainsKey(from))) || onlyOneSpotIsOpen)
                    ? onlyOneSpotIsOpen
                    : checkThatTheOppositePeripheralExists;
        }

        private bool _IsOneHive(ref Piece piece, (int x, int y) to, bool isGrasshopper = false, bool isBeetle = false)
        {
            (int, int) oldPoint = piece.Point;
            HashSet<(int x, int y)> oldNeighbors = new(piece.Neighbors);

            _RemovePiece(ref piece);
            if (!IsAllConnected())
            {
                // place it back
                _PlacePiece(ref piece, oldPoint);
                // this move breaks the hive
                return false;
            }
            else
            {
                // Temporarily place this piece to the `to` point
                _PlacePiece(ref piece, to);

                // If it is a grasshopper, just check if it is all connected  
                if ((isGrasshopper
                // if it is a beetle, and it is on top of another one, just check that it is all connected
                || (isBeetle && Pieces[to].Count > 1)
                // for any other piece, make sure it was always connected, and that it is still connected
                || piece.Neighbors.Overlaps(oldNeighbors))
                && IsAllConnected())
                {
                    // move it back
                    MovePiece(piece, oldPoint);
                    // this is a valid move
                    return true;
                }
                else
                {
                    // move it back
                    MovePiece(piece, oldPoint);
                    // this is an invalid move
                    return false;
                }
            }
        }

        public bool IsAllConnected()
        {
            var visited = new Dictionary<(int, int), bool>();
            if (Pieces.Count > 0)
            {
                (int, int) start = Pieces.Keys.First();
                _BoardDFS(ref visited, start);
                return visited.Count == Pieces.Count;
            }
            else
            {
                return true;
            }
        }

        private void _BoardDFS(ref Dictionary<(int, int), bool> visited, (int, int) piecePoint)
        {
            visited[piecePoint] = true;
            if (Pieces[piecePoint].Count == 0)
            {
                return;
            }

            // for each piece in the stack
            foreach (Piece curPiece in Pieces[piecePoint])
            {
                foreach ((int, int) neighbor in curPiece.Neighbors)
                {
                    if ((!visited.ContainsKey(neighbor) || (visited.ContainsKey(neighbor) && !visited[neighbor])) && Pieces.ContainsKey(neighbor))
                        _BoardDFS(ref visited, neighbor);
                }
            }
        }

        // This should be used for the pieces that have an expensive move generation–i.e., for now, only the Ant
        // TODO: Test whether adding this validation actually improves performance on other pieces
        // (which probably does not because it would be validating its surroundings twice) 
        public bool IsPinned(Piece piece, bool isGrasshopper, bool isBeetle)
        {
            // should be set up as soon as neighbors are added
            if (piece.IsSurrounded)
                return true;

            foreach((int, int) side in piece.Sides)
            {
                // If there is at least one valid path
                if (_IsValidMove(ref piece, piece.Point, side, isGrasshopper, isBeetle))
                {
                    // It is not pinned
                    return false;
                }
            }
            // None of its side were a valid path,
            // So this piece is pinned
            return true;
        }
#endregion
    }
}