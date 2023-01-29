using static GameCore.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Player
    {
        public Color Color { get; set; }
        public List<string> Pieces { get; set; }
        public int TurnCount { get; set; }
        public Player(Color color)
        {
            TurnCount = 1;
            char c = char.ToLower(color.ToString()[0]);
            Color = color;
            Pieces = new List<string>()
            {
                $"{c}Q1",
                $"{c}A1",
                $"{c}A2",
                $"{c}A3",
                $"{c}B1",
                $"{c}B2",
                $"{c}G1",
                $"{c}G2",
                $"{c}G3",
                $"{c}S1",
                $"{c}S2",
            };
        }

        public static Move GetMove()
        {
            Console.WriteLine("Enter move");
            string input = Console.ReadLine()!;
            return new Move(input!);
        }

        public bool HasNotPlayedQueen()
        {
            return Pieces.Contains($"{Color}Q1");
        }

        public List<(int, int)> GetPlacingSpots(Dictionary<(int, int), Stack<Piece>> pieces, Dictionary<Color, List<(int, int)>> colorPieces, bool IsAQueenSurrounded)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            List<(int, int)> positions = new();

            if (!IsAQueenSurrounded)
            {
                // iterate through the current player's color's pieces
                foreach ((int, int) point in colorPieces[this.Color])
                {
                    // iterate through this piece's available spots
                    foreach ((int, int) spot in pieces[point].Peek().SpotsAround)
                    {
                        //      Not been visited        It is not neighboring an opponent
                        if (!positions.Contains(spot) && !_HasOpponentNeighbor(spot, pieces))
                        {
                                positions.Add(spot);
                        }
                    }
                }
            }

            stopwatch.Stop();
            PrintRed($"Generating Available Spots for Player {Color} took: {stopwatch.Elapsed.Milliseconds} ms");

            return positions;
        }


        private bool _HasOpponentNeighbor((int x, int y) point, Dictionary<(int, int), Stack<Piece>> pieces)
        {
            // foreach ((int, int) side in SIDE_OFFSETS.Values)
            for (int i = 0; i < 6; ++i)
            {
                (int, int) potentialOpponentNeighborPosition = (point.x + SIDE_OFFSETS_ARRAY[i].x, point.y + SIDE_OFFSETS_ARRAY[i].y);
                // If piece is on the board                                     And Is not the same color as the piece that is about to be placed
                if (pieces.ContainsKey(potentialOpponentNeighborPosition) && pieces[potentialOpponentNeighborPosition].Peek().Color != this.Color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // Checked each side, and no opponent's pieces were found
            return false;
        }
    }

    public class Move
    {
        private readonly string _move;
        public string MovingPiece { get; set; }
        public string DestinationSide { get; set; }
        public string DestinationPiece { get; set; }

        public override string ToString()
        {
            return _move;
        }

        private static string _formatPieceString(string piece)
        {
            char[] pieceAsChars = piece.ToCharArray();
            pieceAsChars[0] = char.ToLower(pieceAsChars[0]);
            pieceAsChars[1] = char.ToUpper(pieceAsChars[1]);
            return new string(pieceAsChars);
        }

        public Move (string input)
        {
            // To make the compiler happy so that it does not throw the
            // `Non-nullable property`
            MovingPiece = null!;
            DestinationPiece = null!;
            DestinationSide = null!;

            // Remove whitespaces
            input = Regex.Replace(input, @"\s+", "");
            //                  Case insensitive Not optional piece     Optional Side                Optional Piece
            const string validPattern = "^([wbWB])([ABGQSabgqs])([1-3])([NSns])?([TWEtwe])?([wbWB]?)([ABGQSabgqs]?)([1-3]?)$";
            if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                _move = input.ToLower();
            }
            else if (Regex.IsMatch(input, validPattern))
            {
                MovingPiece = _formatPieceString(input[..3]);
                if (input.Length > 3)
                {
                    DestinationSide = input.Substring(3, 2).ToUpper();
                    DestinationPiece = _formatPieceString(input[5..]);
                }
                _move = input;
            }
            else
            {
                _move = "invalid";
            }
        }

        public bool IsMoveWithDestination()
        {
            const string validPattern = "^([wbWB])([ABGQSabgqs])([1-3])([NSns])([TWEtwe])([wbWB])([ABGQSabgqs])([1-3])$";
            return Regex.IsMatch(_move, validPattern);
        }
    }

    public enum Color
    {
        Black,
        White
    }
}