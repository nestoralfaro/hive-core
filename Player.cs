using System.Text.RegularExpressions;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    public class Player
    {
        public char Color { get; set; }
        public List<string> Pieces { get; set; }
        public Player(char color)
        {
            Color = color;
            Pieces = new List<string>()
            {
                $"{color}Q1",
                $"{color}A1",
                $"{color}A2",
                $"{color}A3",
                $"{color}B1",
                $"{color}B2",
                $"{color}G1",
                $"{color}G2",
                $"{color}G3",
                $"{color}S1",
                $"{color}S2",
            };
        }
        public Move GetMove()
        {
            Console.WriteLine("Enter move");
            string input = Console.ReadLine();
            // temporary for debugging
            // string input = "bS1";
            return new Move(input!);
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