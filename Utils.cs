using System;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Private members naming style
#nullable enable

namespace HiveCore
{
    public static class Utils
    {

        // Pieces represented by a binary number that maps to
        // their index in the pieces arrays–i.e., WhitePieces or BlackPieces
        public static readonly int A1 = 0b0000; // 0
        public static readonly int A2 = 0b0001; // 1
        public static readonly int A3 = 0b0010; // 2
        public static readonly int G1 = 0b0011; // 3
        public static readonly int G2 = 0b0100; // 4
        public static readonly int G3 = 0b0101; // 5
        public static readonly int B1 = 0b0110; // 6
        public static readonly int B2 = 0b0111; // 7
        public static readonly int S1 = 0b1000; // 8
        public static readonly int S2 = 0b1001; // 9
        public static readonly int Q1 = 0b1010; // 10

        // Color
        public static readonly int BLACK = 0b00100000; // 16
        public static readonly int WHITE = 0b00010000; // 32

        // Black Pieces
        public static readonly int bA1 = 0b010000;
        public static readonly int bA2 = 0b010001;
        public static readonly int bA3 = 0b010010;
        public static readonly int bG1 = 0b010011;
        public static readonly int bG2 = 0b010100;
        public static readonly int bG3 = 0b010101;
        public static readonly int bB1 = 0b010110;
        public static readonly int bB2 = 0b010111;
        public static readonly int bS1 = 0b011000;
        public static readonly int bS2 = 0b011001;
        public static readonly int bQ1 = 0b011010;

        // White Pieces
        public static readonly int wA1 = 0b100000;
        public static readonly int wA2 = 0b100001;
        public static readonly int wA3 = 0b100010;
        public static readonly int wG1 = 0b100011;
        public static readonly int wG2 = 0b100100;
        public static readonly int wG3 = 0b100101;
        public static readonly int wB1 = 0b100110;
        public static readonly int wB2 = 0b100111;
        public static readonly int wS1 = 0b101000;
        public static readonly int wS2 = 0b101001;
        public static readonly int wQ1 = 0b101010;



        // For parsing piece number 
        public static readonly int INSECT_PARSER = 0b00001111;
        // For parsing color
        public static readonly int COLOR_PARSER = 0b11110000;

        public static Color GetColorFromBin(int piece) {
            return (Color)(piece & COLOR_PARSER);
        }

        public static Insect GetInsectFromBin(int piece)
        {
            switch (piece & INSECT_PARSER)
            {
                case 0:
                case 1:
                case 2:
                    return Insect.Ant;
                case 3:
                case 4:
                case 5:
                    return Insect.Grasshopper;
                case 6:
                case 7:
                    return Insect.Beetle;
                case 8:
                case 9:
                    return Insect.Spider;
                default: // case 10
                    return Insect.QueenBee;
            };
        }

        public static readonly Dictionary<string, int> STRING_TO_BIN = new Dictionary<string, int>
        {
            {"bA1", bA1},
            {"bA2", bA2},
            {"bA3", bA3},
            {"bG1", bG1},
            {"bG2", bG2},
            {"bG3", bG3},
            {"bB1", bB1},
            {"bB2", bB2},
            {"bS1", bS1},
            {"bS2", bS2},
            {"bQ1", bQ1},
            {"wA1", wA1},
            {"wA2", wA2},
            {"wA3", wA3},
            {"wG1", wG1},
            {"wG2", wG2},
            {"wG3", wG3},
            {"wB1", wB1},
            {"wB2", wB2},
            {"wS1", wS1},
            {"wS2", wS2},
            {"wQ1", wQ1},
        };

        public static readonly int[] PIECE_NUMBER = {1, 2, 3, 1, 2, 3, 1, 2, 1, 2, 1};

        public static readonly Dictionary<string, int> STRING_TO_INDEX = new Dictionary<string, int>
        {
            {"bA1", A1},
            {"bA2", A2},
            {"bA3", A3},
            {"bG1", G1},
            {"bG2", G2},
            {"bG3", G3},
            {"bB1", B1},
            {"bB2", B2},
            {"bS1", S1},
            {"bS2", S2},
            {"bQ1", Q1},
            {"wA1", A1},
            {"wA2", A2},
            {"wA3", A3},
            {"wG1", G1},
            {"wG2", G2},
            {"wG3", G3},
            {"wB1", B1},
            {"wB2", B2},
            {"wS1", S1},
            {"wS2", S2},
            {"wQ1", Q1},
        };

        public enum Color { Black = 16, White = 32 };
        // public enum Color { Black, White }
        public const int _MAX_DEPTH_TREE_SEARCH = 8;
        public enum Insect { Ant, Grasshopper, Beetle, Spider, QueenBee }
        // public enum Insect { QueenBee, Beetle, Grasshopper, Spider, Ant }
        public const int MANY_SIDES = 6;
        public const int _SPIDER_MAX_STEP_COUNT = 3;
        public static readonly Dictionary<string, (int x, int y)> SIDE_OFFSETS = new Dictionary<string, (int x, int y)>
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

        // For Unity
        // public static readonly Dictionary<string, (int x, int y)> SIDE_OFFSETS = new Dictionary<string, (int x, int y)>
        // {
        //     // Notice how each side is only valid if it adds up to an even number
        //     { "NT", (0, 55) },    // [0] North
        //     { "NW", (-48, 28) },   // [1] Northwest
        //     { "SW", (-48, -28) },   // [2] Southwest
        //     { "ST", (0, -55) },  // [3] South
        //     { "SE", (48, -28) },   // [4] Southeast
        //     { "NE", (48, 28) },    // [5] Northeast
        // };

        // public static readonly (int x, int y)[] SIDE_OFFSETS_ARRAY =
        // {
        //     // Notice how each side is only valid if it adds up to an even number
        //     (0, 55),    // [0] North
        //     (-48, 28),   // [1] Northwest
        //     (-48, -28),   // [2] Southwest
        //     (0, -55),  // [3] South
        //     (48, -28),   // [4] Southeast
        //     (48, 28),    // [5] Northeast
        // };

        public static void PrintYellow(string warning)
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
                _PrintRemainingPieces(color == Color.Black ? board.BlackPieces : board.WhitePieces);
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

        private static void _PrintRemainingPieces(Piece[] pieces)
        {
            string Output = "";
            foreach (Piece piece in pieces)
            {
                if (!piece.IsOnBoard)
                {
                    Output += $"{piece}|";
                }
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
                    Console.WriteLine($"{entry.Value.Peek()}: {entry.Value.Peek().IsOnBoard} is at {entry.Value.Peek().Point}");
                    //Print the neighbours
                    // foreach (var neighbour in entry.Value.Peek().GetNeighbors(in Pieces))
                    foreach (var neighbour in entry.Value.Peek().Neighbors)
                    {
                        Console.WriteLine($"{neighbour}");
                    }
                }
                Console.WriteLine("/*********************************/");
            }
        }

        public static void PrintFormatted(Board board)
        {
            if (board.Pieces.Count != 0)
            {
                Dictionary<(int, int), bool> hasBeenPrinted = new Dictionary<(int, int), bool>();
                foreach (var entry in board.Pieces)
                {
                    foreach (Piece piece in entry.Value)
                    {
                        if (!hasBeenPrinted.ContainsKey(piece.Point) || !hasBeenPrinted[piece.Point])
                        {
                            PrintAsHexagon(piece);
                            hasBeenPrinted[piece.Point] = true;
                        }
                    }
                }
            }
        }

        public static void PrintAsHexagon(Piece piece)
        {
            string NT = piece.Neighbors.Contains(piece.GetSidePointByStringDir("NT")) ? ("\t\t" + piece.GetSidePointByStringDir("NT") + Environment.NewLine + "\t\t-----------") : "\t\t-----------";
            string NW = piece.Neighbors.Contains(piece.GetSidePointByStringDir("NW")) ? ("\t  " + piece.GetSidePointByStringDir("NW") + "/") : "\t\t/";
            string SW = piece.Neighbors.Contains(piece.GetSidePointByStringDir("SW")) ? ("\t  " + piece.GetSidePointByStringDir("SW") +  "\\") : "\t\t\\";
            string ST = piece.Neighbors.Contains(piece.GetSidePointByStringDir("ST")) ? ("\t\t-----------" + Environment.NewLine + "\t\t" + piece.GetSidePointByStringDir("ST")) : "\t\t-----------";
            string SE = piece.Neighbors.Contains(piece.GetSidePointByStringDir("SE")) ? ("\t /" + piece.GetSidePointByStringDir("SE")) : "\t /";
            string NE = piece.Neighbors.Contains(piece.GetSidePointByStringDir("NE")) ? ("\t \\" + piece.GetSidePointByStringDir("NE")) : "\t \\";

            Console.WriteLine(NT);
            Console.WriteLine(NW + NE);
            Console.WriteLine($"\t\t{piece} {piece.Point}");
            Console.WriteLine(SW + SE);
            Console.WriteLine(ST);
            Console.WriteLine("*************************************");
            Console.WriteLine();
        }
    }
}