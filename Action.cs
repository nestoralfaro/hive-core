using static HiveCore.Utils;
using System.Text.RegularExpressions;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    public class HumanAction
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
            pieceAsChars[0] = char.ToLower(pieceAsChars[0]); // Color lower
            pieceAsChars[1] = char.ToUpper(pieceAsChars[1]); // Insect UPPER
            return new string(pieceAsChars);
        }

        public HumanAction (string input)
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

    public class AIAction
    {
        public Piece Piece { get; set; }
        public (int x, int y) To { get; set; }
        public ActionType Action {get; set;}
        public AIAction (Piece piece, (ActionType action, (int x, int y) to) move)
        {
            Piece = piece;
            Action = move.action;
            To = move.to;
        }
    }
}