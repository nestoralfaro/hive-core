namespace HiveCore
#pragma warning disable IDE1006 // Private members naming style

{
    public static class Utils
    {
        public enum Color { Black, White }
        public enum Insect { QueenBee, Beetle, Grasshopper, Spider, Ant }
        public enum ActionType { Moving, Placing }
        public const int MANY_SIDES = 6;
        public const int _SPIDER_MAX_STEP_COUNT = 3;
        public static readonly Dictionary<string, (int x, int y)> SIDE_OFFSETS = new()
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

        public static void PrintPlayerHeader(Color color, Board board)
        {
                Console.BackgroundColor = color == Color.Black ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.ForegroundColor = color == Color.Black ? ConsoleColor.White : ConsoleColor.Black;
                Console.Write($"It is {color}'s turn.");
                _PrintRemainingPieces(color == Color.Black ? board.BlackPiecesKeys : board.WhitePiecesKeys);
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

        private static void _PrintRemainingPieces(HashSet<string> pieces)
        {
            string Output = "";
            foreach (string piece in pieces)
            {
                // Maybe we should use the String builder for performance improvement
                // However, since this is just a helper method, probably doesn't matter much
                Output += $"{piece.ToString()[1]}{piece.ToString()[2]}|";
            }
            Console.Write($" | Remaining pieces: {Output}");
        }
        public static void PrintPieces(Dictionary<(int, int), Stack<Piece>> Pieces)
        {
            // Print the board
            if (Pieces.Count != 0)
            {
                Console.WriteLine("/*********************************/");
                Console.WriteLine("Current board state:");
                foreach (var entry in Pieces)
                {
                    Console.WriteLine($"{entry.Value.Peek()} is at {entry.Value.Peek().Point}");
                    //Print the neighbours
                    foreach (var neighbour in entry.Value.Peek().Neighbors)
                    {
                        Console.WriteLine($"{neighbour}");
                    }
                }
                Console.WriteLine("/*********************************/");
            }
        }

    }
}