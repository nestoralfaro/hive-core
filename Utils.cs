namespace HiveCore
#pragma warning disable IDE1006 // Private members naming style

{
    public static class Utils
    {
        public enum Color { Black, White }
        public enum Insect { QueenBee, Beetle, Grasshopper, Spider, Ant }
        public enum ActionKind { Moving, Placing }
        public const int MANY_SIDES = 6;
        public const int _SPIDER_MAX_STEP_COUNT = 3;
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

        public static readonly (int x, int y)[] SIDE_OFFSETS_ARRAY =
        {
            // Notice how each side is only valid if it adds up to an even number
            (1, 1),    // [0] North
            (-1, 1),   // [1] Northwest
            (-2, 0),   // [2] Southwest
            (-1, -1),  // [3] South
            (1, -1),   // [4] Southeast
            (2, 0),    // [5] Northeast
        };

        public static void _PrintWarning(string warning)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(warning);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintPlayerHeader(Color color)
        {
                Console.BackgroundColor = color == Color.Black ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.ForegroundColor = color == Color.Black ? ConsoleColor.White : ConsoleColor.Black;
                Console.Write($"It is {color}'s turn.");
                // _PrintRemainingPieces(player);
                Console.ResetColor();
                Console.WriteLine();
        }

        public static void PrintRed (string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintGreen (string message)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);
            Console.ResetColor();
            Console.WriteLine();
        }

        // private static void _PrintRemainingPieces(Color player)
        // {
        //     string Output = "";
        //     foreach (Piece piece in player.Pieces)
        //     {
        //         // Maybe we should use the String builder for performance improvement
        //         Output += $"{piece.ToString()[1]}{piece.ToString()[2]}|";
        //     }
        //     Console.Write($" | Remaining pieces: {Output}");
        // }
    }
}