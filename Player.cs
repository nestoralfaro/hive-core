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

        public List<(int, int)> GetPlacingSpots(Board board)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Maybe keep track of the visited ones with a hashmap and also pass it to the hasopponent neighbor?
            List<(int, int)> positions = new();

            if (!board.IsAQueenSurrounded())
            {
                // iterate through the current player's color's pieces
                foreach ((int, int) point in board._color_pieces[this.Color])
                {
                    // iterate through this piece's available spots
                    foreach ((int, int) spot in board.pieces[point].Peek().SpotsAround)
                    {
                        //      Not been visited        It is not neighboring an opponent
                        if (!positions.Contains(spot) && !_HasOpponentNeighbor(spot, board.pieces))
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


        private bool _HasOpponentNeighbor((int, int) point, Dictionary<(int, int), Stack<Piece>> _point_stack)
        {
            foreach ((int, int) side in SIDE_OFFSETS.Values)
            {
                (int, int) potentialOpponentNeighborPosition = (point.Item1 + side.Item1, point.Item2 + side.Item2);
                // If piece is on the board                                     And Is not the same color as the piece that is about to be placed
                if (_point_stack.ContainsKey(potentialOpponentNeighborPosition) && _point_stack[potentialOpponentNeighborPosition].Peek().Color != this.Color)
                {
                    // Has an opponent neighbor
                    return true;
                }
            }

            // // Checked each side, and no opponent's pieces were found
            return false;
            // return _point_stack[point].Peek().Neighbors.Any(neighbor => _point_stack[neighbor.Value].Peek().Color != Color);
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
            const string validPattern = "^([wbWB])([ABGQSabgqs])([1-3])([NSns])?([TWEtwe])?([wbWB]?)([ABGQSabgqs]?)([1-3]?)$";
            if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                _move = input.ToLower();
            }
            //  Case insensitive Not optional piece       Optional Side       Optional Piece
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