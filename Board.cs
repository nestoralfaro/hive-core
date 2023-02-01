#pragma warning disable IDE1006 // Private members naming style
using static HiveCore.Utils;
#nullable enable
namespace HiveCore
{
    public class Board
    {
        public Dictionary<(int, int), Stack<Piece>> Pieces;
        private readonly Dictionary<string, (int, int)> _piece_point;

        // TODO: Look into changing from `List<(int, int)>` to `HashSet<(int, int)>`
        private readonly Dictionary<Color, List<(int, int)> > _color_pieces;

        public List<Piece> WhitePieces { get; set; }
        public List<Piece> BlackPieces { get; set; }

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

            WhitePieces = new List<Piece>()
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

            BlackPieces = new List<Piece>()
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








































        // public Board Clone()
        // {
        //     return new Board()
        //     {
        //         Pieces = this.Pieces.ToDictionary(point => point.Key, stack => new Stack<Piece>(stack.Value.Select(piece => piece.Clone()))),
        //         _color_pieces = this._color_pieces.ToDictionary(color => color.Key, pieces => new List<(int, int)>(pieces.Value)),
        //         _piece_point = new Dictionary<string, (int, int)>(this._piece_point),
        //     };
        // }


        // public Piece? GetCloneTopPieceByStringName(string piece)
        // {
        //     return _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p.Clone() : null) : null;
        // }

        public Piece? GetRefTopPieceByStringName(string piece)
        {
            return _piece_point.ContainsKey(piece) ? (Pieces[_piece_point[piece]].TryPeek(out Piece p) ?  p : null) : null;
        }

        public bool IsAQueenSurrounded()
        {
            return (GetRefTopPieceByStringName("wQ1")?.IsSurrounded() == true) || (GetRefTopPieceByStringName("bQ1")?.IsSurrounded() == true);
        }

        // public Piece GetCloneTopPieceByPoint((int, int) point)
        // {
        //     return Pieces[point].Peek().Clone();
        // }

        public Piece GetRefTopPieceByPoint((int, int) point)
        {
            return Pieces[point].Peek();
        }

        // public Piece GetClonePieceByStringName(string piece)
        // {
        //     return Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece)).Clone();
        // }

        public Piece GetRefPieceByStringName(string piece)
        {
            return Pieces[_piece_point[piece]].First(p => p.ToString().Equals(piece));
        }

        // public Piece GetClonePieceByPoint((int x, int y) point)
        // {
        //     return Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y).Clone();
        // }

        public Piece GetRefPieceByPoint((int x, int y) point)
        {
            return Pieces[point].First(piece => piece.Point.x == point.x && piece.Point.y == point.y);
        }

        // public List<Piece> GetClonePiecesByColor(Color color)
        // {
        //     List<Piece> res = new();
        //     foreach((int, int) point in _color_pieces[color])
        //     {
        //         if (Pieces[point].TryPeek(out Piece topPiece))
        //         {
        //             res.Add(topPiece.Clone());
        //         }
        //     }
        //     return res;
        // }

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
            foreach ((int, int) sidePoint in piece.Sides.Keys)
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

        public void UpdateNeighborsAt((int x, int y) point)
        {
            // if this is a busy spot
            if (Pieces.ContainsKey(point))
            {
                // for each piece at this spot
                foreach (Piece piece in Pieces[point])
                {
                    // re-populate its neighbors
                    PopulateNeighborsFor(piece);

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
                        }
                    }
                }
            }

            // instead of making it a method that updates when called,
            // just update it now, so that it doesn't add to the call stack? idk wdyt
            // piece.IsSurrounded = piece.Neighbors.Count == MANY_SIDES
        }

        public void AddPiece(Piece piece, (int, int) to)
        {
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
            UpdateNeighborsAt(to);
        }

        public void _RemovePiece(Piece piece)
        {
            (int, int) removingSpot = piece.Point;
            // remove it
            Pieces[removingSpot].Pop();
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
            UpdateNeighborsAt(removingSpot);
        }

        public void MovePiece(Piece piece, (int, int) to)
        {
            // remove piece
            _RemovePiece(piece);
            // add it to the new point
            AddPiece(piece, to);
        }

        private void _DFS(ref Dictionary<(int, int), bool> visited, (int, int) piecePoint)
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
                        _DFS(ref visited, neighbor);
                }
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