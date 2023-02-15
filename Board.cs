#pragma warning disable IDE1006 // Private members naming style
using static HiveCore.Utils;
using System.Diagnostics;
using System.Security.Cryptography;

namespace HiveCore
{
    public class Board
    {
        // For benchmarking only
        private int moveCount;
        private int curMove = 0;
        private int manyCalculations = 0;
        private double totalTimeSoFar = 0;
        private const int MIN = 1000000;
        private const int MAX = -1000000;

        public Dictionary<(int, int), Stack<Piece>> Pieces;
        public Piece[] WhitePieces =
        {
            new Piece(wA1),
            new Piece(wA2),
            new Piece(wA3),
            new Piece(wG1),
            new Piece(wG2),
            new Piece(wG3),
            new Piece(wB1),
            new Piece(wB2),
            new Piece(wS1),
            new Piece(wS2),
            new Piece(wQ1)
        };
        public Piece[] BlackPieces =
        {
            new Piece(bA1),
            new Piece(bA2),
            new Piece(bA3),
            new Piece(bG1),
            new Piece(bG2),
            new Piece(bG3),
            new Piece(bB1),
            new Piece(bB2),
            new Piece(bS1),
            new Piece(bS2),
            new Piece(bQ1)
        };

        // private Dictionary<int, HashSet<int>> _Zobrist_Keys { get; set; }
        private Dictionary<int, string> _Zobrist_Keys { get; set; }

        public Board()
        {
            Pieces = new Dictionary<(int, int), Stack<Piece>>();
            Pieces.EnsureCapacity(22);
            _Zobrist_Keys = new();
            _Zobrist_Keys.EnsureCapacity(200);
        }

        /*************************************************************************
        **************************************************************************
                                            AI
        *************************************************************************
        *************************************************************************/
        public bool AIMove(Color color)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            moveCount = 0;

            (int eval, (Piece piece, (int, int) to)) = _Search(color, MAX, MIN, 0);

            PrintRed($"Moving {piece} to {to}. Eval was: {eval}");
            MovePiece(piece, to);
            ++curMove;

            stopwatch.Stop();
            PrintGreen($"#{curMove} - {color}'s AI calculated {moveCount} moves with depth {_MAX_DEPTH_TREE_SEARCH} in {stopwatch.Elapsed.TotalSeconds} s.");
            return true;
        }

        private int _Evaluate(Color curPlayer)
        {
            // *** GOOD STATES ***

            // Enemy Queen Surrounded -> ∞
            //int manyPiecesAroundOpponentsQueen = curPlayer == Color.Black ? WhitePieces["wQ1"].Neighbors.Count : BlackPieces["bQ1"].Neighbors.Count;

            //if (manyPiecesAroundOpponentsQueen == 6)
                //return (1000000, curMove);

            // Enemy ant pinned (no available moves for ant) * how many pinned
            // int antMoves;
            // TODO: Implement function to check immediate moves as if it were a queen

            // Enemy Queen pinned
            // int enemyQueenMoves;

            // Opponent has no available moves on the Queen

            // Queen able to move?

            // Beetle on top of enemy queen?

            // Potential spawn points on enemy queen w/ pieces in reserve?

            // More moves available the better

            // Enemy can't spawn by your queen

            // Pieces that have available moves and give the pieces a weight

            // Defenders on the Queen?



            // *** BAD STATES ***

            // Queens are adjacent (leads to more ties)

            // Queen is pinned, no available moves

            // Ant(s) are pinned

            // No possible moves or little possible moves

            // Enemy beetle on queen

            // Enemy beetle on queen w/ pieces in reserve for spawning

            // Enemy queen can move



            //dummy heuristic that encourages to play more pieces around opponents queen
            int manyPiecesAroundMyQueen = curPlayer == Color.Black ? BlackPieces[Q1].Neighbors.Count : WhitePieces[Q1].Neighbors.Count;
            int manyPiecesAroundOpponentsQueen = curPlayer == Color.Black ? WhitePieces[Q1].Neighbors.Count : BlackPieces[Q1].Neighbors.Count;

           return manyPiecesAroundOpponentsQueen - manyPiecesAroundMyQueen;

            // // maybe the pieces around queen should have a weight?
            // if (manyPiecesAroundMyQueen > manyPiecesAroundOpponentsQueen)
            // {
            //     // return (new Random().Next(1, 1000010), curMove);
            //     return (-manyPiecesAroundMyQueen, curMove);
            // }
            // else
            // {
            //     return (new Random().Next(-1000010, -1), curMove);
            // }
        }

        // Based on the original pseudocode and this guy's: https://www.youtube.com/watch?v=U4ogK0MIzqk&t=1005s
        private (int eval, (Piece piece, (int, int) to) move) _Search(Color curPlayer, int alpha, int beta, int depth, (Piece, (int, int)) curMove = default)
        {
            if (depth == _MAX_DEPTH_TREE_SEARCH)
            {
                // Evaluate the curMove taking into account Pieces (curState) from your opponent's perspective
                // Write `Evaluate` or heuristics function
                // return (new Random().Next(-1000010, 1000010), curMove);

                /**
                Keep in mind the following when evaluating:
                It is curPlayer's turn, therefore you can:
                    Check the pieces curPlayer has played, and that score should be the opposite sign-i.e.,g -score
                    because those would count against the parent (opponent)
                
                It is curPlayer's turn, therefore you can:
                    Check the opponent's pieces, and that score should be the same sign–i.e., score
                    because those would count for parent (opponent)

                Evaluation should be based on how much myMove has affected my opponent, and pick the one that has
                hurt them the most, or benefited them the least.
                **/

                // curMove was made by !curPlayer–i.e., curPlayer's opponent
                // We are evaluating curPlayer's board state, and seeing how much did curMove affected curPlayer
                return (_Evaluate(curPlayer), curMove);
            }

            // Generate opponents moves
            HashSet<(Piece, (int, int))> moves = GenerateMovesFor(curPlayer).ToHashSet();
            // var random = new Random();
            // HashSet<(Piece, (int, int))> moves = GenerateMovesFor(curPlayer).ToList().OrderBy(x => random.Next()).ToHashSet();
            moveCount += moves.Count;

            // has no more moves
            if (moves.Count == 0)
            {
                return (_Evaluate(curPlayer), curMove);
            }

            foreach ((Piece curPiece, (int, int) to) in moves)
            {
                (int, int) oldPoint = curPiece.Point;
                Piece piece = curPiece.Color == Color.Black ? BlackPieces[curPiece.Index] : WhitePieces[curPiece.Index];

                // make move
                MovePiece(piece, to);

                (int evaluation, (Piece, (int, int)) attemptedMove) = _Search(curPlayer == Color.Black ? Color.White : Color.Black, -beta, -alpha, depth + 1, (piece, to));
                // opponent's good evaluation = bad for currentPlayer, and viceversa
                // i.e., opponent's bad evaluation = good for currentPlayer.
                evaluation = -evaluation;

                // put it back (undo)
                MovePiece(piece, oldPoint);

                if (evaluation >= beta)
                {
                    // Move was too good, opponent will avoid this position
                    return (beta, attemptedMove); // Prun! (kill the kids)
                }

                // pick whichever is the most terrible for opponent,
                // which would be the best for currentPlayer
                if (evaluation > alpha)
                {
                    alpha = evaluation;
                    curMove = (piece, to);
                }
            }
            return (alpha, curMove);
        }

#region AI Method Helpers
        public HashSet<(Piece piece, (int, int) to)> GenerateMovesFor(Color curPlayer)
        {
            HashSet<(Piece, (int, int))> moves = new();
            if (!IsGameOver())
            {
                Piece playersQueenPiece = curPlayer == Color.Black ? BlackPieces[Q1] : WhitePieces[Q1];
                int manyPiecesPlayedByCurPlayer = _GetManyPiecesPlayedBy(curPlayer);
                foreach (Piece piece in curPlayer == Color.Black ? BlackPieces : WhitePieces)
                {
                    if (Pieces.Count > 0)
                    {
                        if (piece.IsOnBoard)
                        {
                            if (piece.IsTop && playersQueenPiece.IsOnBoard)
                            {
                                foreach ((int, int) spot in GetMovingSpotsFor(piece))
                                {
                                    piece.IsPinned = false;
                                    moves.Add((piece.Clone(), spot));
                                }
                            }
                            else
                            {
                                piece.IsPinned = true;
                            }
                        }
                        else
                        {
                            if (manyPiecesPlayedByCurPlayer == 0 && !piece.Equals(playersQueenPiece))
                            {
                                foreach ((int, int) spot in Pieces[(0, 0)].Peek().Sides)
                                {
                                    piece.IsPinned = false;
                                    moves.Add((piece.Clone(), spot));
                                }
                            }
                            else if (manyPiecesPlayedByCurPlayer == 3 && !playersQueenPiece.IsOnBoard)
                            {
                                foreach ((int, int) spot in GetPlacingSpotsFor(curPlayer))
                                {
                                    piece.IsPinned = false;
                                    // Must play queen on their 4th turn
                                    moves.Add((playersQueenPiece.Clone(), spot));
                                }
                            }
                            else
                            {
                                foreach ((int, int) spot in GetPlacingSpotsFor(curPlayer))
                                {
                                    piece.IsPinned = false;
                                    moves.Add((piece.Clone(), spot));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!piece.Equals(playersQueenPiece))
                        {
                            piece.IsPinned = false;
                            moves.Add((piece.Clone(), (0, 0)));
                        }
                    }
                }
            }
            return moves;
        }

        public bool IsGameOver()
        {
            return BlackPieces[Q1].IsSurrounded || WhitePieces[Q1].IsSurrounded;
        }

        // this is attempting to generate the hashing on the fly,
        // but it really should just retrieve the value from the table
        // however, would this really help?
        private int GetCurrentHash()
        {
            int hash = 17;
            foreach (KeyValuePair<(int, int), Stack<Piece>> stack in Pieces)
            {
                foreach (Piece piece in stack.Value)
                {
                    hash ^= _Hash(piece._bin_piece, piece.Point.x, piece.Point.y);
                }
            }

            // if (!_Zobrist_Keys.ContainsKey(hash))
            // {
            //     _Zobrist_Keys.Add(hash, "idk");
            // }
            return hash;
        }

        private static int _Hash(int a, int b, int c)
        {
            int hash = 17;
            hash ^= (31 * (hash << 7)) + a.GetHashCode();
            hash ^= (31 * (hash << 7)) + b.GetHashCode();
            hash ^= (31 * (hash << 7)) + c.GetHashCode();
            return hash;
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
        public HashSet<(int, int)> GetPlacingSpotsFor(Color curPlayer)
        {
            // Stopwatch stopwatch = new();
            // stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            HashSet<(int, int)> positions = new();
            foreach (Piece piece in curPlayer == Color.Black ? BlackPieces : WhitePieces)
            {
                if (piece.IsOnBoard)
                {
                    // iterate through this piece's available spots
                    foreach ((int, int) spot in piece.OpenSpotsAround)
                    {
                        //      Not been visited        It is not neighboring an opponent
                        if (!positions.Contains(spot) && !_HasOpponentNeighbor(spot, curPlayer))
                        {
                            positions.Add(spot);
                        }
                    }
                }
            }
            // stopwatch.Stop();
            // PrintRed($"Generating Available Placing Spots for {color} Player took: {stopwatch.Elapsed.TotalMilliseconds} ms");
            return positions;
        }

        public HashSet<(int, int)> GetMovingSpotsFor(Piece piece)
        {
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

#region Placing/Moving on the Board helper methods
        private void _PlacePiece(ref Piece piece, (int, int) to)
        {
            if (piece.Insect == Insect.Beetle && Pieces.ContainsKey(to))
            {
                Pieces[to].Peek().IsTop = false;
                Pieces[to].Push(piece);
            }
            else
            {
                Pieces[to] = new Stack<Piece>();
                Pieces[to].Push(piece);
            }
            piece.Point = to;
            piece.IsTop = true;
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
            else
            {
                Pieces[removingSpot].Peek().IsTop = true;
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

        private int _GetManyPiecesPlayedBy(Color color)
        {
            int counter = 0;
            if (color == Color.Black)
            {
                for (int p = 0; p < 11; ++p)
                {
                    if (BlackPieces[p].IsOnBoard)
                        ++counter;
                }
            }
            else
            {
                for (int p = 0; p < 11; ++p)
                {
                    if (WhitePieces[p].IsOnBoard)
                        ++counter;
                }
            }
            return counter;
        }
#endregion

#region Each Moving Spot Getter for `Piece`
        private HashSet<(int, int)> _GetAntMovingSpots(ref Piece piece)
        {
            // Stopwatch stopwatch = new();
            // stopwatch.Start();
            HashSet<(int x, int y)> positions = new();
            (int x, int y) oldAntPosition = piece.Point;
            _AntDFS(ref piece, ref positions, piece.Point);

            // Because the last point it found is where this piece is now positioned
            // Move it back to where it was
            MovePiece(piece, oldAntPosition);

            // itself should not be included
            positions.Remove(oldAntPosition);

            // stopwatch.Stop();
            // PrintRed("Generating Ant Moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
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
            // Stopwatch stopwatch = new();
            // stopwatch.Start();
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
            // stopwatch.Stop();
            // PrintRed("Generating Beetle moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
            return validMoves;
        }

        private HashSet<(int, int)> _GetGrasshopperMovingSpots(ref Piece piece)
        {
            // Stopwatch stopwatch = new();
            // stopwatch.Start();
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
            // stopwatch.Stop();
            // PrintRed("Generating grasshoper moves took: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
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

                // It has not been visited      AND Is a valid move
                if (!visited.Contains(nextSpot) && (piece.Point != nextSpot) && _IsValidMove(ref piece, curSpot, nextSpot))
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
            // Stopwatch stopwatch = new();
            // stopwatch.Start();
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
            // stopwatch.Stop();
            // PrintRed("Elapsed time: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
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
            //  Only beetle can crawl on top of point (to)
            return (isBeetle || !Pieces.ContainsKey(to))
            //  Checking freedom of movement first is about 40 calculations/ms faster
            && _IsFreedomOfMovement(ref piece, from, to, isBeetle)
            && _IsOneHive(ref piece, to, isGrasshopper, isBeetle);
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
            var visited = new HashSet<(int, int)>();
            if (Pieces.Count > 0)
            {
                (int, int) start = Pieces.Keys.First();
                visited.Add(start);
                return _BoardDFS(ref visited, start);
            }
            else
            {
                return true;
            }
        }

        private bool _BoardDFS(ref HashSet<(int, int)> visited, (int, int) curPoint)
        {
            foreach ((int, int) neighbor in Pieces[curPoint].Peek().Neighbors)
            {
                if (!visited.Contains(neighbor)) {
                    visited.Add(neighbor);
                    if (Pieces.Count == visited.Count)
                    {
                        return true;
                    }
                    _BoardDFS(ref visited, neighbor);
                }
            }
            return Pieces.Count == visited.Count;
        }

        // Has not been tested
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