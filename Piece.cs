using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public class Piece
    {
        private readonly string _piece;
        public Color Color { get; set; }
        public int Number { get; set; }
        public bool IsOnBoard { get; set; }
        public Insect Insect { get; set; }
        public (int x, int y) Point { get; set; }
        public HashSet<(int, int)> Sides { get {
            return new HashSet<(int, int)>()
                {
                    (Point.x + SIDE_OFFSETS_ARRAY[0].x, Point.y + SIDE_OFFSETS_ARRAY[0].y), // [0] North
                    (Point.x + SIDE_OFFSETS_ARRAY[1].x, Point.y + SIDE_OFFSETS_ARRAY[1].y), // [1] Northwest
                    (Point.x + SIDE_OFFSETS_ARRAY[2].x, Point.y + SIDE_OFFSETS_ARRAY[2].y), // [2] Southwest
                    (Point.x + SIDE_OFFSETS_ARRAY[3].x, Point.y + SIDE_OFFSETS_ARRAY[3].y), // [3] South
                    (Point.x + SIDE_OFFSETS_ARRAY[4].x, Point.y + SIDE_OFFSETS_ARRAY[4].y), // [4] Southeast
                    (Point.x + SIDE_OFFSETS_ARRAY[5].x, Point.y + SIDE_OFFSETS_ARRAY[5].y), // [5] Northeast
                };
        } }
        public HashSet<(int, int)> Neighbors { get; set; }
        public (int x, int y) GetSidePointByStringDir (string dir) => (SIDE_OFFSETS[dir].x + Point.x, SIDE_OFFSETS[dir].y + Point.y);
        public string GetSideStringByPoint ((int x, int y) sidePoint) => SIDE_OFFSETS.Keys.First(dir => (GetSidePointByStringDir(dir).x == sidePoint.x) && (GetSidePointByStringDir(dir).y == sidePoint.y));
        public HashSet<(int, int)> OpenSpotsAround { get { return Sides.Except(Neighbors).ToHashSet(); } }
        public override string ToString() { return _piece; }
        public bool IsSurrounded { get; set; }
        // When calling `GenerateMovesFor` (Board.cs:ln. 27) `IsPinned` should get updated
        public bool IsPinned { get; set; }

        // When calling `_PlacePiece` and `_RemovePiece` from `Board`, `IsTop` property should get updated
        public bool IsTop { get; set; }
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
            Neighbors = new HashSet<(int, int)>();
            Neighbors.EnsureCapacity(MANY_SIDES);
            SetToDefault();
        }

        public void SetToDefault()
        {
            Neighbors.Clear();
            Point = (1, 2); // invalid position
            IsOnBoard = false;
            IsSurrounded = false;
            IsPinned = false;
            IsTop = false;
        }
        public Piece Clone()
        {
            return new Piece(this._piece) {
                Color = this.Color,
                Insect = this.Insect,
                Number = this.Number,
                Point = this.Point,
                IsOnBoard = this.IsOnBoard,
                Neighbors = new HashSet<(int, int)>(this.Neighbors),
            };
        }

        public override bool Equals(object? obj)
        {
            return
            // if they are the same type
            obj is Piece p
            // and both are either on or off the board
            && p.IsOnBoard == IsOnBoard
            // and have the same string form (which should validate their Color, Insect, and Number)
            && _piece.Equals(p._piece);
        }

        // hashed by its string form
        public override int GetHashCode()
        {
            return HashCode.Combine(_piece);
        }
    }
}