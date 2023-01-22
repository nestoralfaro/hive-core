
namespace GameCore
{
    public static class Utils
    {
        // TODO: make the points (int x, int y)
        public static readonly Dictionary<string, (int, int)> SIDE_OFFSETS = new Dictionary<string, (int, int)>()
        {
            // Notice how each side is only valid if it adds up to an even number
            { "*/", (-1, 1) },   // [0] Northwest
            { "*|", (-2, 0) },   // [1] West
            { "*\\", (-1, -1) }, // [2] Southwest
            { "/*", (1, -1) },   // [3] Southeast
            { "|*", (2, 0) },    // [4] East
            { "\\*", (1, 1) },   // [5] Northeast
        };
    }
}