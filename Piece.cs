using System.Diagnostics;
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

        public HashSet<(int, int)> Neighbors { get; set; }

                                                        // If `Sides` is another HashSet, we could use `ExceptWith` instead, which would
                                                        // not require LINQ, that way using a more native way from `HashSet`.
                                                        // We need to discover whether we need the mapping to the letters before tho–e.g., "NT"
        public HashSet<(int, int)> OpenSpotsAround { get { return Sides.Select(s => s.Key).ToHashSet().Except(Neighbors).ToHashSet(); } }
        public int ManyNeighbors { get{ return Neighbors.Count; } }
        public override string ToString() { return _piece; }
        public bool IsSurrounded { get; set; }
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
            Point = (0, 0);
            IsOnBoard = false;
            IsSurrounded = false;
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
            // and have the same string (which should validate their Color, Insect, and Number)
            && _piece == p._piece
            // and the same point
            && (p.Point.x == Point.x) && (p.Point.y == p.Point.y)

            // maybe unnecessary? to be benchmarked
            // and the same amount of spots around
            && p.OpenSpotsAround == OpenSpotsAround
            // and the same amount of neighbors
            && p.ManyNeighbors == ManyNeighbors;
        }

        // hashed by its string form
        public override int GetHashCode()
        {
            return HashCode.Combine(_piece);
        }
    }
}