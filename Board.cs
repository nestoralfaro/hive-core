#pragma warning disable IDE1006 // Private members naming style
namespace GameCore
{
    public class Board
    {
        public readonly Dictionary<string, (int, int)> _piece_point;
        public readonly Dictionary<(int, int), Stack<Piece>> Pieces;
        public readonly Dictionary<Color, List<(int, int)> > _color_pieces;
        // public readonly Dictionary<Player, List<Piece>> _player_pieces;

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

        public Piece GetTopPieceByStringName(string piece)
        {
            return Pieces[_piece_point[piece]].Peek();
        }

        public bool IsAQueenSurrounded()
        {
            return (_piece_point.ContainsKey("wQ1") && GetTopPieceByStringName("wQ1").IsSurrounded()) || (_piece_point.ContainsKey("bQ1") && GetTopPieceByStringName("bQ1").IsSurrounded());
        }

        public Piece GetTopPieceByPoint((int, int) point)
        {
            return Pieces[point].Peek();
        }

        public Piece GetPieceByStringName(string piece)
        {
            return Pieces[_piece_point[piece]].First(piece => piece.ToString().Equals(piece));
        }

        public Piece GetPieceByPoint((int x, int y) point)
        {
            return Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y);
        }

        public List<Piece> GetPiecesByColor(Color color)
        {
            return _color_pieces[color].ConvertAll(piecePoint => Pieces[piecePoint].Peek());
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

        public void _AddPiece((int x, int y) point, Piece piece)
        {
            // There are pieces at this spot    AND  It is a beetle
            if (Pieces.ContainsKey(point) && piece.Insect == Insect.Beetle)
            {
                Pieces[point].Push(piece);
                _piece_point.Add(piece.ToString(), point);
                _color_pieces[piece.Color].Add(point);
                UpdateAllNeighbors();
            }

            // No pieces at this spot
            else
            {
                Pieces.Add(point, new Stack<Piece>());
                Pieces[point].Push(piece);
                _piece_point.Add(piece.ToString(), point);
                _color_pieces[piece.Color].Add(point);
                UpdateAllNeighbors();
            }
        }

        public void _RemovePiece(Piece piece)
        {
            (int, int) piecePointToRemove = _piece_point[piece.ToString()];
            Pieces[piecePointToRemove].Pop();

            // If this is an empty stack
            if (Pieces[piecePointToRemove].Count == 0)
            {
                // Delete the reference, as it is now an open spot
                Pieces.Remove(piecePointToRemove);
            }

            _piece_point.Remove(piece.ToString());
            _color_pieces[piece.Color].Remove(piecePointToRemove);
            UpdateAllNeighbors();
        }

        public void PlacePiece(Player player, Piece piece, (int, int) to)
        {
            // first piece on the board. Place it on the origin (0, 0)
            _AddPiece(to, piece);
            // Does not remove on the first turn
            // player.Pieces.Remove(move.MovingPiece);
            player.Pieces.Remove(piece.ToString());
            // PrintAvailableMovesForThePiece(piece);
        }

        public void MovePiece(Piece piece, (int, int) to)
        {
            // Move such existing piece
            // remove piece
            _RemovePiece(piece);
            // re-add it
            piece.Point = to;
            _AddPiece(to, piece);
            // PrintAvailableMovesForThePiece(piece);
        }

        private void _DFS(ref Dictionary<(int, int), bool> visited, (int, int) piecePoint)
        {
            visited[piecePoint] = true;
            if (!Pieces.ContainsKey(piecePoint))
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