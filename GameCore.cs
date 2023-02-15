using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    class GameCore
    {
        private bool _isFirstPlayersTurn;
        private Color _player1 { get; }
        private Color _player2 { get; }
        public Board Board { get; }
        public GameCore()
        {
            _isFirstPlayersTurn = false;
            // _isFirstPlayersTurn = true;
            _player1 = Color.Black;
            _player2 = Color.White;
            Board = new();
        }

        public void Play()
        {
            if (_isFirstPlayersTurn)
            {
                PrintPlayerHeader(_player1, Board);
                if (HumanMove(_player1))
                // if (Board.AIMove(_player1))
                    _isFirstPlayersTurn = false;
            }
            else
            {
                PrintPlayerHeader(_player2, Board);
                // if (HumanMove(_player2))
                if (Board.AIMove(_player2))
                    _isFirstPlayersTurn = true;
            }
            PrintFormatted(Board);
        }

        public bool IsGameOver()
        {
            if (Board.IsGameOver())
            {
                if (Board.WhitePieces["wQ1"].IsSurrounded)
                {
                    PrintGreen("Black won!");
                }
                else
                {
                    PrintGreen("White won!");
                }
                return true;
            }
            return false;
        }

        public bool HumanMove(Color color)
        {
            try
            {
                (Piece piece, (int, int) to) = _GetHumanMove(color);
                if (Board.GenerateMovesFor(color).Contains((piece, to)))
                {
                    Board.MovePiece(piece, to);
                    return true;
                }
                else
                {
                    throw new ArgumentException("Invalid move!");
                }
            }
            catch (ArgumentException ex)
            {
                PrintYellow(ex.Message);
                return false;
            }
        }

        private (Piece piece, (int, int) to) _GetHumanMove(Color color)
        {
            Console.WriteLine("Enter move");
            string input = Console.ReadLine()!;
            string p = input[..3];

            Piece piece = color == Color.Black ? Board.BlackPieces[p] : Board.WhitePieces[p];
            (int, int) to = _GetNewPoint(input);
            return (piece, to);
        }

        private (int, int) _GetNewPoint(string move)
        {
            if (move.Length > 3)
            {
                string direction = move.Substring(3, 2).ToUpper();
                string referencePiece = move[5..];
                // Grab reference's piece point
                (int x, int y) = referencePiece[0] == 'b' ? Board.BlackPieces[referencePiece].Point : Board.WhitePieces[referencePiece].Point;
                // Grab the offset point
                (int x, int y) sideOffset = SIDE_OFFSETS[direction];
                return (x + sideOffset.x, y + sideOffset.y);
            }
            else
            {
                return (0, 0);
            }
        }
    }
}