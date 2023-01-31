#pragma warning disable IDE1006 // Private members naming style
#nullable enable
namespace HiveCore
{
    public class Board
    {
        private Dictionary<string, (int, int)> _piece_point;
        public Dictionary<(int, int), Stack<Piece>> Pieces;
        private Dictionary<Color, List<(int, int)> > _color_pieces;

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            Pieces = new Dictionary<(int, int), Stack<Piece>>();
            Pieces.EnsureCapacity(22);

            _piece_point = new Dictionary<string, (int, int)>();
            _piece_point.EnsureCapacity(22);

            _color_pieces = new Dictionary< Color, List<(int, int)> >()
            {
                {Color.Black, new List<(int, int)>()},
                {Color.White, new List<(int, int)>()},
            };
            _color_pieces.EnsureCapacity(2);
        }

        public Board Clone()
        {
            return new Board()
            {
                Pieces = this.Pieces.ToDictionary(point => point.Key, stack => new Stack<Piece>(stack.Value.Select(piece => piece.Clone()))),
                _color_pieces = this._color_pieces.ToDictionary(color => color.Key, pieces => new List<(int, int)>(pieces.Value)),
                _piece_point = new Dictionary<string, (int, int)>(this._piece_point),
            };
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

        public Piece? GetCloneTopPieceByStringName(string piece)
        {
            return _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p.Clone() : null) : null;
        }

        public Piece? GetRefTopPieceByStringName(string piece)
        {
            return _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p : null) : null;
        }

        public bool IsAQueenSurrounded()
        {
            return (GetRefTopPieceByStringName("wQ1")?.IsSurrounded() == true) || (GetRefTopPieceByStringName("bQ1")?.IsSurrounded() == true);
        }

        public Piece GetCloneTopPieceByPoint((int, int) point)
        {
            return Pieces[point].Peek().Clone();
        }

        public Piece GetRefTopPieceByPoint((int, int) point)
        {
            return Pieces[point].Peek();
        }

        public Piece GetClonePieceByStringName(string piece)
        {
            return Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece)).Clone();
        }

        public Piece GetRefPieceByStringName(string piece)
        {
            return Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece));
        }

        public Piece GetClonePieceByPoint((int x, int y) point)
        {
            return Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y).Clone();
        }

        public Piece GetRefPieceByPoint((int x, int y) point)
        {
            return Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y);
        }

        public List<Piece> GetClonePiecesByColor(Color color)
        {
            List<Piece> res = new();
            foreach(var entry in _color_pieces[color])
            {
                if (Pieces[entry].TryPeek(out Piece topPiece))
                {
                    res.Add(topPiece.Clone());
                }
            }
            return res;
        }

        public List<Piece> GetRefPiecesByColor(Color color)
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
            // first piece on the board. Place it on the origin (0, 0)
            piece.Point = to;
            _AddPiece(to, piece, false);
            int i = player.Pieces.FindIndex(p => p.ToString().Equals(piece.ToString()));
            player.Pieces.RemoveAt(i);
        }

        public void MovePiece(Piece piece, (int, int) to)
        {
            // remove piece
            _RemovePiece(piece);
            // re-add it
            piece.Point = to;
            _AddPiece(to, piece, false);
        }

        public void AIMove (Player player, ActionKind action, Piece piece, (int, int) to)
        {
            if (action == ActionKind.Moving)
            {
                MovePiece(piece, to);
            }

            if (action == ActionKind.Placing)
            {
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