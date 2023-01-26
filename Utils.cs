namespace GameCore
{
    public static class Utils
    {
        public static readonly Dictionary<string, (int, int)> SIDE_OFFSETS = new()
        {
            // Notice how each side is only valid if it adds up to an even number
            { "NT", (1, 1) },    // [0] North
            { "NW", (-1, 1) },   // [1] Northwest
            { "SW", (-2, 0) },   // [2] Southwest
            { "ST", (-1, -1) },  // [3] South
            { "SE", (1, -1) },   // [4] Southeast
            { "NE", (2, 0) },    // [5] Northeast
        };

        public static readonly List<(int, int)> SIDE_OFFSETS_LIST = new()
        {
            // Notice how each side is only valid if it adds up to an even number
            (1, 1),    // [0] North
            (-1, 1),   // [1] Northwest
            (-2, 0),   // [2] Southwest
            (-1, -1),  // [3] South
            (1, -1),   // [4] Southeast
            (2, 0),    // [5] Northeast
        };
    }
}