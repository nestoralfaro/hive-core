#pragma warning disable IDE1006 // Private members naming style
using static HiveCore.Utils;
using System.Diagnostics;
namespace HiveCore
{
    public class Board
    {
        private const int _MAX_DEPTH_TREE_SEARCH = 5;
        public Dictionary<(int, int), Stack<Piece>> Pieces;
        private Dictionary<string, (int, int)> _piece_point;
        private Dictionary<Color, HashSet<(int, int)>> _color_pieces;
        public HashSet<Piece> WhitePieces { get; set; }
        public HashSet<Piece> BlackPieces { get; set; }

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            Pieces = new Dictionary<(int, int), Stack<Piece>>();
            Pieces.EnsureCapacity(22);

            _piece_point = new Dictionary<string, (int, int)>();
            _piece_point.EnsureCapacity(22);

            _color_pieces = new Dictionary< Color, HashSet<(int, int)> >()
            {
                {Color.Black, new HashSet<(int, int)>()},
                {Color.White, new HashSet<(int, int)>()},
            };
            _color_pieces.EnsureCapacity(2);

            WhitePieces = new HashSet<Piece>()
            {
               new Piece("wQ1"),
               new Piece("wA1"),
               new Piece("wA2"),
               new Piece("wA3"),
               new Piece("wB1"),
               new Piece("wB2"),
               new Piece("wG1"),
               new Piece("wG2"),
               new Piece("wG3"),
               new Piece("wS1"),
               new Piece("wS2"),
            };

            BlackPieces = new HashSet<Piece>()
            {
               new Piece("bQ1"),
               new Piece("bA1"),
               new Piece("bA2"),
               new Piece("bA3"),
               new Piece("bB1"),
               new Piece("bB2"),
               new Piece("bG1"),
               new Piece("bG2"),
               new Piece("bG3"),
               new Piece("bS1"),
               new Piece("bS2"),
            };
        }

        public Board Clone()
        {
            return new Board()
            {
                WhitePieces = this.WhitePieces.Select(p => p.Clone()).ToHashSet(),
                BlackPieces = this.BlackPieces.Select(p => p.Clone()).ToHashSet(),
                Pieces = this.Pieces.ToDictionary(point => point.Key, stack => new Stack<Piece>(stack.Value.Select(piece => piece.Clone()))),
                _color_pieces = this._color_pieces.ToDictionary(color => color.Key, pieces => new HashSet<(int, int)>(pieces.Value)),
                _piece_point = new Dictionary<string, (int, int)>(this._piece_point),
            };
        }

        /*************************************************************************
        **************************************************************************
                                            AI
        *************************************************************************
        *************************************************************************/
        public bool AIMove(Color color)
        {
            //    if AI's color is black, then Opponent is White, otherwise black
            Color opponent = color == Color.Black ? Color.White : Color.Black;
            (int maxVal, AIAction? move) = alpha_beta(ref color, ref opponent, color);
            if (move != null)
                _AIMakeMove(move.Action, move.Piece, move.To);
            // if everything went well
            return true;
        }

#region AI Method Helpers
        private void _AIMakeMove (ActionType action, Piece piece, (int, int) to)
        {
            if (action == ActionType.Moving)
            {
                MovePiece(piece, to);
            }

            if (action == ActionType.Placing)
            {
                PlacePiece(piece, to);
            }
        }
        private (int, AIAction?) alpha_beta(ref Color AI, ref Color opponent, Color whoseTurn, int alpha = int.MinValue, int beta = int.MaxValue, int depth = 1)
        {
            AIAction? myMove = null;
            if (depth > _MAX_DEPTH_TREE_SEARCH || IsAQueenSurrounded())
            {
                // Calculate score with heuristic
                // Return score and currentMove
                return (new Random().Next(1, 50), myMove);
            }

            // If AI turn
            if (whoseTurn == AI)
            {
                int maxValue = -1000000;
                // pieces hashset should be in sync with board but they are not
                HashSet<(ActionType action, (int, int) to)> availableMoves = new();

                // get AI's available placing spots
                foreach ((int, int) to in GetPlacingSpotsFor(AI))
                {
                    availableMoves.Add((ActionType.Placing, to));
                }

                // For every AI piece
                foreach (Piece piece in AI == Color.Black ? BlackPieces : WhitePieces)
                {
                    Piece curPiece = piece;

                    // if this piece is on the board
                    if (piece.IsOnBoard)
                    {
                        foreach ((int, int) to in GetMovingSpotsFor(ref curPiece))
                        {
                            availableMoves.Add((ActionType.Moving, to));
                        }
                    }

                    // for every available move for current piece
                    foreach ((ActionType action, (int, int) to) availableMove in availableMoves)
                    {
                        Board prev = this.Clone();
                        // make move
                        if (piece.IsOnBoard && availableMove.action == ActionType.Moving)
                        {
                            // crashes on the second skip
                            // first move is successful
                            // by the second time it gets here, the piece in the original blackpieces set do not match the
                            // current state–i.e., points dont match

                            // Break point here and start with `wG1`! The depth should be of 5
                            MovePiece(piece, availableMove.to);
                        }
                        else if (!piece.IsOnBoard && availableMove.action == ActionType.Placing)
                        {
                            PlacePiece(piece, availableMove.to);
                        }
                                                                                    // switch whose turn
                        (int min, AIAction? move) = alpha_beta(ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

                        if (min > maxValue)
                        {
                            maxValue = min;
                            myMove = new AIAction(piece, availableMove);
                        }

                        // set game state back to what is was before making move
                        SetState(prev);

                        // If alpha catches up to beta, kill kids
                        if (maxValue >= beta)
                            return (maxValue, myMove);

                        // new value is greater than alpha? update alpha
                        if (maxValue > alpha)
                            alpha = maxValue;
                    }
                }
                return (maxValue, myMove);
            }

            // If Opponent turn
            // if (whoseTurn != this.Color)
            else
            {
                int minValue = 1000000;

                HashSet<(ActionType action, (int, int) to)> availableMoves = new();

                // get opponent's available placing spots
                foreach (var to in GetPlacingSpotsFor(opponent))
                {
                    availableMoves.Add((ActionType.Placing, to));
                }

                // For every opponent's piece
                foreach (Piece piece in opponent == Color.Black ? BlackPieces : WhitePieces)
                {
                    Piece curPiece = piece;

                    // if this piece is on the board
                    if (piece.IsOnBoard)
                    {
                        // get its moving spots
                        foreach ((int, int) to in GetMovingSpotsFor(ref curPiece))
                        {
                            availableMoves.Add((ActionType.Moving, to));
                        }
                    }

                    // for every available move for current piece
                    foreach ((ActionType action, (int, int) to) availableMove in availableMoves)
                    {
                        Board prev = this.Clone();

                        // make move
                        if (piece.IsOnBoard && availableMove.action == ActionType.Moving)
                        {
                            MovePiece(piece, availableMove.to);
                        }
                        else if (!piece.IsOnBoard && availableMove.action == ActionType.Placing)
                        {
                            PlacePiece(piece, availableMove.to);
                        }
                                                                                    // switch whose turn
                        (int max, AIAction? move) = alpha_beta(ref AI, ref opponent, whoseTurn == Color.White ? Color.Black : Color.White, alpha, beta, depth + 1);

                        if (max < minValue)
                        {
                            minValue = max;
                            myMove = new AIAction(piece, availableMove);
                        }

                        // set game state back to what is was before making move
                        SetState(prev);

                        // If alpha catches up to beta, kill kids
                        if (minValue <= beta)
                            return (minValue, myMove);

                        // new value is greater than alpha? update alpha
                        if (minValue < alpha)
                            alpha = minValue;
                    }
                }
                return (minValue, myMove);
            }
        }
#endregion


        /*************************************************************************
        **************************************************************************
                                        Pieces' Management
        *************************************************************************
        *************************************************************************/
#region Placing/Moving on the Board
        public void MovePiece(Piece piece, (int, int) to)
        {
            // remove piece
            _RemovePiece(piece);
            // place it at the new point
            PlacePiece(piece, to);
        }

        public void PlacePiece(Piece piece, (int, int) to)
        {
            if (to.Item1 == 2 && to.Item2 == 0 && piece.ToString().Equals("bQ1"))
            {
                // third skip crashes
                Console.Write("bP!");
            }

            // if there are pieces at such point              AND it is a beetle
            if (Pieces.ContainsKey(to) && Pieces[to].Count > 0 && piece.Insect == Insect.Beetle)
            {
                // let it crawl on top
                Pieces[to].Push(piece);
                _piece_point[piece.ToString()] = to;
                _color_pieces[piece.Color].Remove(piece.Point);
                _color_pieces[piece.Color].Add(to);
            }
            else
            {
                // create a new point -> stack ref
                Pieces.Add(to, new Stack<Piece>());
                // push the reference to this piece
                Pieces[to].Push(piece);
                // update helper dictionaries
                _piece_point.Add(piece.ToString(), to);
                _color_pieces[piece.Color].Add(to);
            }
            piece.Point = to;
            piece.IsOnBoard = true;
            _UpdateNeighborsAt(to);
        }

        public void SetState(Board board)
        {
            Pieces = board.Pieces;
            _color_pieces = board._color_pieces;
            _piece_point = board._piece_point;
            WhitePieces = board.WhitePieces;
            BlackPieces = board.BlackPieces;
        }

        private void _RemovePiece(Piece piece)
        {
            (int, int) removingSpot = piece.Point;

            if (Pieces.ContainsKey(removingSpot))
            {
                if (Pieces[removingSpot].Count > 0)
                {
                    // remove the top
                    Pieces[removingSpot].Pop();
                }

                // if the stack is empty
                if (Pieces[removingSpot].Count == 0)
                {
                    // Delete the reference too, as it is now an open spot
                    Pieces.Remove(removingSpot);
                }

                // Also remove the references from the helper dictionaries
                _piece_point.Remove(piece.ToString());
                _color_pieces[piece.Color].Remove(removingSpot);
                piece.IsOnBoard = false;
                _UpdateNeighborsAt(removingSpot);
            }
        }
#endregion

#region Placing/Moving on the Board helper methods
        public bool IsAQueenSurrounded() => (GetRefTopPieceByStringName("wQ1")?.IsSurrounded) == true || (GetRefTopPieceByStringName("bQ1")?.IsSurrounded) == true;
        public bool IsOnBoard(string piece) => _piece_point.ContainsKey(piece);
        public (int x, int y) GetPointByString(string piece) => _piece_point[piece];
        public bool IsEmpty() => Pieces.Count == 0 && _piece_point.Count == 0;
        private void _PopulateNeighborsFor(Piece piece)
        {
            piece.Neighbors.Clear();
            foreach ((int, int) sidePoint in piece.Sides)
            {
                // bool IsNeighbor = (point % side == (0, 0));
                // (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighborExists = Pieces.ContainsKey(sidePoint);
                if (neighborExists && !piece.Neighbors.Contains(sidePoint))
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
#endregion

#region Piece getters
        public Piece? GetCloneTopPieceByStringName(string piece) => _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p.Clone() : null) : null;
        public Piece? GetRefTopPieceByStringName(string piece) => _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p : null) : null;
        public Piece GetCloneTopPieceByPoint((int, int) point) => Pieces[point].Peek().Clone();
        public Piece GetRefTopPieceByPoint((int, int) point) => Pieces[point].Peek();
        public Piece GetClonePieceByStringName(string piece) => Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece)).Clone();
        public Piece GetRefPieceByStringName(string piece) => Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece));
        public Piece GetClonePieceByPoint((int x, int y) point) => Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y).Clone();
        public Piece GetRefPieceByPoint((int x, int y) point) => Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y);
        public HashSet<Piece> GetClonePiecesByColor(Color color)
        {
            HashSet<Piece> res = new();
            foreach((int, int) point in _color_pieces[color])
            {
                if (Pieces[point].TryPeek(out Piece topPiece))
                {
                    res.Add(topPiece.Clone());
                }
            }
            return res;
        }

        public HashSet<Piece> GetRefPiecesByColor(Color color)
        {
            HashSet<Piece> res = new();
            foreach(var entry in _color_pieces[color])
            {
                if (Pieces[entry].TryPeek(out Piece topPiece))
                {
                    res.Add(topPiece);
                }
            }
            return res;
        }

        public int GetManyPiecesPlayedBy(Color color)
        {
            int counter = 0;
            foreach ((int, int) point in _color_pieces[color])
            {
                foreach (Piece piece in Pieces[point])
                {
                    if (piece.Color == color)
                    {
                        ++counter;
                    }
                }
            }
            return counter;
        }
#endregion
        /*************************************************************************
        **************************************************************************
                                        Move getters
        *************************************************************************
        *************************************************************************/
        public HashSet<(int, int)> GetPlacingSpotsFor(Color color)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            HashSet<(int, int)> positions = new();

            // If nothing has been played
            if (IsEmpty())
            {
                // Origin is the only first valid placing spot
                return new HashSet<(int, int)>(){(0, 0)};
            }
            // If there is only one piece on the board
            else if (Pieces.Count == 1)
            {
                // the available placing spots will be the such piece's surrounding spots
                return Pieces[(0, 0)].Peek().OpenSpotsAround;
            }
            // Make sure the game is still going
            else if (!IsAQueenSurrounded())
            {
                // from here on out, only the spots that do not neighbor an opponent are valid  

                // iterate through the current player's pieces on the board
                foreach (Piece? piece in GetRefPiecesByColor(color))
                {
                    if (piece != null)
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
            PrintRed($"Generating Available Spots for {color} Player took: {stopwatch.Elapsed.TotalMilliseconds} ms");
            return positions;
        }

        public HashSet<(int, int)> GetMovingSpotsFor(ref Piece piece)
        {
            string queen = $"{char.ToLower(piece.Color.ToString()[0])}Q1";
            // NOTE: the order of these statements DO matter. We can take advantage of each short-circuit operator–i.e., and, or
            // Maybe checking whether the piece is on top should be the first one,
            // because that would be a more common statment as the game progresses?

            // if the piece about to be moved is not a queen            
            if (!piece.ToString().Equals(queen)
            // AND the queen of this piece's color has not been played            
            && !IsOnBoard(queen)
            // AND it is not the top piece
            && !Pieces[piece.Point].Peek().Equals(this))
            {
                // This piece cannot move
                return new HashSet<(int, int)>();
            }

            // If the piece about to be placed is the fourth one
            else if (GetManyPiecesPlayedBy(piece.Color) == 3
            // AND this piece is not the queen
            && !piece.ToString().Equals(queen)
            // AND the queen has not been played
            && !IsOnBoard(queen))
            {
                // then you must place a piece,
                // and such piece MUST be the queen (this may need to be enforced by GameManager)
                return GetPlacingSpotsFor(piece.Color);
            }

            else
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
        }

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
                // If piece is on the board                                     And Is not the same color as the piece that is about to be placed
                // if (pieces.ContainsKey(potentialOpponentNeighborPosition) && pieces[potentialOpponentNeighborPosition].Peek().Color != this.Color)
                if (Pieces.ContainsKey(potentialOpponentNeighborPosition) && Pieces[potentialOpponentNeighborPosition].TryPeek(out Piece topPiece) && topPiece.Color != color)
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
            // if it is a beetle, make sure the oldNeighbors also include the pieces below 
            (int, int) oldPoint = piece.Point;
            HashSet<(int x, int y)> oldNeighbors = new(piece.Neighbors);

            _RemovePiece(piece);
            if (!IsAllConnected())
            {
                // place it back
                PlacePiece(piece, oldPoint);
                // this move breaks the hive
                return false;
            }
            else
            {
                // Temporarily place this piece to the `to` point
                PlacePiece(piece, to);

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