#pragma warning disable IDE1006 // Private members naming style
#nullable enable
namespace GameCore
{
    public class Board
    {
        private Dictionary<string, (int, int)> _piece_point;
        public Dictionary<(int, int), Stack<Piece>> Pieces;
        private Dictionary<Color, List<(int, int)> > _color_pieces;

        private Dictionary<string, (int, int)> _last_piece_point;
        private Dictionary<(int, int), Stack<Piece>> _last_Pieces;
        private Dictionary<Color, List<(int, int)> > _last_color_pieces;

        // public readonly Dictionary<Player, List<Piece>> _player_pieces;

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            Pieces = new Dictionary<(int, int), Stack<Piece>>();
            Pieces.EnsureCapacity(22);
            _last_Pieces = new Dictionary<(int, int), Stack<Piece>>();
            _last_Pieces.EnsureCapacity(22);

            _piece_point = new Dictionary<string, (int, int)>();
            _piece_point.EnsureCapacity(22);
            _last_piece_point = new Dictionary<string, (int, int)>();
            _last_piece_point.EnsureCapacity(22);

            _color_pieces = new Dictionary< Color, List<(int, int)> >()
            {
                {Color.Black, new List<(int, int)>()},
                {Color.White, new List<(int, int)>()},
            };
            _color_pieces.EnsureCapacity(2);
            _last_color_pieces = new Dictionary< Color, List<(int, int)> >()
            {
                {Color.Black, new List<(int, int)>()},
                {Color.White, new List<(int, int)>()},
            };
            _last_color_pieces.EnsureCapacity(2);
        }

        public void ReplaceWithState(Board board)
        {
            Pieces.Clear();
            foreach(KeyValuePair<(int, int), Stack<Piece>> kvp in board.Pieces)
            {
                Pieces.Add(kvp.Key, kvp.Value);
            }

            _piece_point.Clear();
            foreach(KeyValuePair<string, (int, int)> kvp in board._piece_point)
            {
                _piece_point.Add(kvp.Key, kvp.Value);
            }

            _color_pieces[Color.Black].Clear();
            _color_pieces[Color.White].Clear();
            foreach (KeyValuePair<Color, List<(int, int)>> kvp in board._color_pieces)
            {
                _color_pieces[kvp.Key].AddRange(kvp.Value);
            }

            _last_Pieces.Clear();
            foreach (KeyValuePair<(int, int), Stack<Piece>> kvp in board._last_Pieces)
            {
                _last_Pieces.Add(kvp.Key, kvp.Value);
            }


            _last_piece_point.Clear();
            foreach (KeyValuePair<string, (int, int)> kvp in board._last_piece_point)
            {
                _last_piece_point.Add(kvp.Key, kvp.Value);
            }

            _last_color_pieces[Color.Black].Clear();
            _last_color_pieces[Color.White].Clear();
            foreach (KeyValuePair<Color, List<(int, int)>> kvp in board._last_color_pieces)
            {
                _last_color_pieces[kvp.Key].AddRange(kvp.Value);
            }
        }

        public void UpdateAllNeighbors()
        {
            foreach (KeyValuePair<(int, int), Stack<Piece>> stack in Pieces)
            {
                foreach (Piece piece in stack.Value)
                {
                    PopulateNeighborsFor(piece);
                }
            }
        }

        public Piece? GetTopPieceByStringName(string piece)
        {
            return _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p : null) : null;
        }

        public bool IsAQueenSurrounded()
        {
            return (GetTopPieceByStringName("wQ1")?.IsSurrounded() == true) || (GetTopPieceByStringName("bQ1")?.IsSurrounded() == true);
        }

        public Piece GetTopPieceByPoint((int, int) point)
        {
            return Pieces[point].Peek();
        }

        public Piece GetPieceByStringName(string piece)
        {
            return Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece));
        }

        public Piece GetPieceByPoint((int x, int y) point)
        {
            return Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y);
        }

        public List<Piece> GetPiecesByColor(Color color)
        {
            List<Piece> res = new();
            foreach(var entry in _color_pieces[color])
            {
                if (Pieces[entry].TryPeek(out Piece topPiece))
                {
                    res.Add(topPiece);
                }
            }

            return res;
            // return _color_pieces[color].ConvertAll(piecePoint => Pieces[piecePoint].Peek());
            // return _color_pieces[color].ConvertAll(piecePoint => Pieces[piecePoint].TryPeek(out Piece piece) ? piece : null);
        }

        public bool IsOnBoard(string piece)
        {
            return _piece_point.ContainsKey(piece);
        }

        public (int x, int y) GetPointByString(string piece)
        {
            return _piece_point[piece];
        }

        public bool IsEmpty()
        {
            return Pieces.Count == 0 && _piece_point.Count == 0;
        }

        private void PopulateNeighborsFor(Piece piece)
        {
            piece.Neighbors.Clear();
            foreach (KeyValuePair<string, (int, int)> side in piece.Sides)
            {
                // bool IsNeighbour = (point % side == (0, 0));
                // (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighbourExists = Pieces.ContainsKey(side.Value);
                if (neighbourExists)
                {
                    piece.Neighbors[side.Key] = side.Value;
                }
            }
        }

        public void _AddPiece((int x, int y) point, Piece piece, bool isMoving = true)
        {
            if (isMoving)
            {
                // if there are pieces at such point                    AND it is a beetle
                if (Pieces.ContainsKey(point) && Pieces[point].Count > 0 && piece.Insect == Insect.Beetle)
                {
                        // let it crawl on top
                        Pieces[point].Push(piece);
                }

                Pieces[point].Clear();
                Pieces[point].Push(piece);
                _piece_point[piece.ToString()] = point;
                _color_pieces[piece.Color].Remove(piece.Point);
                _color_pieces[piece.Color].Add(point);
                UpdateAllNeighbors();
                // else, do not add this move, because only the beetle is allowed to get pushed on the stack
            }
            else
            {
                if (!Pieces.ContainsKey(point))
                {
                    Pieces.Add(point, new Stack<Piece>());
                    Pieces[point].Push(piece);
                    _piece_point.Add(piece.ToString(), point);
                    _color_pieces[piece.Color].Add(point);
                    UpdateAllNeighbors();
                }
            }
        }

        public void _RemovePiece(Piece piece)
        {
            if (_piece_point.ContainsKey(piece.ToString()))
            {
                (int, int) piecePointToRemove = _piece_point[piece.ToString()];

                // if no one is there
                if (Pieces[piecePointToRemove].Count == 0)
                {
                    // Delete the reference, as it is now an open spot
                    Pieces.Remove(piecePointToRemove);
                }
                else
                {
                    // Someone is there, so remove it
                    Pieces[piecePointToRemove].Pop();
                    // if no remains there
                    if (Pieces[piecePointToRemove].Count == 0)
                    {
                        // Delete the reference too, as it is now an open spot
                        Pieces.Remove(piecePointToRemove);
                    }
                }

                _piece_point.Remove(piece.ToString());
                _color_pieces[piece.Color].Remove(piecePointToRemove);
                UpdateAllNeighbors();
            }
        }

        public void PlacePiece(Player player, Piece piece, (int, int) to)
        {
            _BackUpPreviousState();
            // first piece on the board. Place it on the origin (0, 0)
            piece.Point = to;
            _AddPiece(to, piece, false);
            // Does not remove on the first turn
            // player.Pieces.Remove(move.MovingPiece);
            player.Pieces.Remove(piece);
            // PrintAvailableMovesForThePiece(piece);
        }

        public void _BackUpPreviousState()
        {
            _last_Pieces.Clear();
            foreach (KeyValuePair<(int, int), Stack<Piece>> kvp in Pieces)
            {
                _last_Pieces.Add(kvp.Key, kvp.Value);
            }


            _last_piece_point.Clear();
            foreach (KeyValuePair<string, (int, int)> kvp in _piece_point)
            {
                _last_piece_point.Add(kvp.Key, kvp.Value);
            }

            _last_color_pieces[Color.Black].Clear();
            _last_color_pieces[Color.White].Clear();
            foreach (KeyValuePair<Color, List<(int, int)>> kvp in _color_pieces)
            {
                _last_color_pieces[kvp.Key].AddRange(kvp.Value);
            }
        }

        public void MovePiece(Piece piece, (int, int) to)
        {
            _BackUpPreviousState();
            // Move such existing piece
            // remove piece
            _RemovePiece(piece);
            // re-add it
            piece.Point = to;
            _AddPiece(to, piece, false);
            // PrintAvailableMovesForThePiece(piece);
        }

        public void Undo()
        {
            Pieces.Clear();
            foreach(KeyValuePair<(int, int), Stack<Piece>> kvp in _last_Pieces)
            {
                Pieces.Add(kvp.Key, kvp.Value);
            }


            _piece_point.Clear();
            foreach(KeyValuePair<string, (int, int)> kvp in _last_piece_point)
            {
                _piece_point.Add(kvp.Key, kvp.Value);
            }

            _color_pieces[Color.Black].Clear();
            _color_pieces[Color.White].Clear();
            foreach (KeyValuePair<Color, List<(int, int)>> kvp in _last_color_pieces)
            {
                _color_pieces[kvp.Key].AddRange(kvp.Value);
            }
        }

        public void AIMove (Player player, Piece piece, (int, int) to, bool isMoving)
        {
            if (isMoving)
            {
                // treat it as moving
                MovePiece(piece, to);
            }
            else
            {
                // treat it as a placing
                PlacePiece(player, piece, to);
            }
        }

        private void _DFS(ref Dictionary<(int, int), bool> visited, (int, int) piecePoint)
        {
            visited[piecePoint] = true;
            if (Pieces[piecePoint].Count == 0)
            {
                return;
            }
            Piece curPiece = Pieces[piecePoint].Peek();
            foreach ((int, int) neighbor in curPiece.Neighbors.Values)
            {
                if ((!visited.ContainsKey(neighbor) || (visited.ContainsKey(neighbor) && !visited[neighbor])) && Pieces.ContainsKey(neighbor))
                    _DFS(ref visited, neighbor);
            }
        }

        public bool IsAllConnected()
        {
            var visited = new Dictionary<(int, int), bool>();
            if (Pieces.Count > 0)
            {
                (int, int) start = Pieces.Keys.First();

                _DFS(ref visited, start);

                return visited.Count == Pieces.Count;
            }
            else
            {
                return true;
            }
        }
    }
}