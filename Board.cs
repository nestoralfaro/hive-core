#pragma warning disable IDE1006 // Private members naming style
namespace GameCore
{
    public class Board
    {
        public readonly Dictionary<string, (int, int)> _piece_point;
        public readonly Dictionary<(int, int), Stack<Piece>> pieces;
        public readonly Dictionary<Color, List<(int, int)> > _color_pieces;
        // public readonly Dictionary<Player, List<Piece>> _player_pieces;

        public Board()
        {
            // Ensuring capacities so that each time an element is added
            // there is no need to dynamically allocate more memory.
            // This is an approach that should help performance
            pieces = new Dictionary<(int, int), Stack<Piece>>();
            pieces.EnsureCapacity(22);

            _piece_point = new Dictionary<string, (int, int)>();
            _piece_point.EnsureCapacity(22);

            _color_pieces = new Dictionary< Color, List<(int, int)> >()
            {
                {Color.Black, new List<(int, int)>()},
                {Color.White, new List<(int, int)>()},
            };
            _color_pieces.EnsureCapacity(2);
        }
        // Change the `List<Piece>` to `List<(int, int)` so that it can later be accessed by _point_stack
        // this way, we dont have to update two hashmaps. That would take too much time.

        public void UpdateAllNeighbors()
        {
            foreach (KeyValuePair<(int, int), Stack<Piece>> stack in pieces)
            {
                foreach (Piece piece in stack.Value)
                {
                    PopulateNeighborsFor(piece);
                }
            }
        }
        private void PopulateNeighborsFor(Piece piece)
        {
            piece.Neighbors.Clear();
            foreach (KeyValuePair<string, (int, int)> side in piece.Sides)  
            {
                // bool IsNeighbour = (point % side == (0, 0));
                // (int, int) neighborPoint = (point.x + side.Value.Item1, point.y + side.Value.Item2);
                bool neighbourExists = pieces.ContainsKey(side.Value);
                if (neighbourExists)
                {
                    piece.Neighbors[side.Key] = side.Value;
                }
            }
        }

        public void _AddPiece((int x, int y) point, Piece piece)
        {
            // There are pieces at this spot    AND  It is a beetle
            if (pieces.ContainsKey(point) && piece.Insect == Insect.Beetle)
            {
                pieces[point].Push(piece);
                _piece_point.Add(piece.ToString(), point);
                _color_pieces[piece.Color].Add(point);
                UpdateAllNeighbors();
            }

            // No pieces at this spot
            else
            {
                pieces.Add(point, new Stack<Piece>());
                pieces[point].Push(piece);
                _piece_point.Add(piece.ToString(), point);
                _color_pieces[piece.Color].Add(point);
                UpdateAllNeighbors();
            }
        }

        public void _RemovePiece(Piece piece)
        {
            (int, int) piecePointToRemove = _piece_point[piece.ToString()];
            pieces[piecePointToRemove].Pop();

            // If this is an empty stack
            if (pieces[piecePointToRemove].Count == 0)
            {
                // Delete the reference, as it is now an open spot
                pieces.Remove(piecePointToRemove);
            }

            _piece_point.Remove(piece.ToString());
            _color_pieces[piece.Color].Remove(piecePointToRemove);
            UpdateAllNeighbors();
        }

        public void PlacePiece(Player player, Move move, Piece piece, (int, int) to)
        {
            // first piece on the board. Place it on the origin (0, 0)
            _AddPiece(to, piece);
            // Does not remove on the first turn
            player.Pieces.Remove(move.MovingPiece);
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
            if (!pieces.ContainsKey(piecePoint))
            {
                return;
            }
            Piece curPiece = pieces[piecePoint].Peek();
            foreach ((int, int) neighbor in curPiece.Neighbors.Values)
            {
                if ((!visited.ContainsKey(neighbor) || (visited.ContainsKey(neighbor) && !visited[neighbor])) && pieces.ContainsKey(neighbor))
                    _DFS(ref visited, neighbor);
            }
        }

        public bool IsAllConnected()
        {
            var visited = new Dictionary<(int, int), bool>();
            (int, int) start = pieces.Keys.First();

            _DFS(ref visited, start);

            return visited.Count == pieces.Count;
        }
    }
}