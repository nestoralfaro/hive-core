#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    class GameCore
    {
        private Player _blackPlayer;
        private Player _whitePlayer;
        private bool _isBlackPlayerTurn;

        public Logic Board { get; }
        public GameCore()
        {
            Board = new Logic();
            _blackPlayer = new Player('b');
            _whitePlayer = new Player('w');
            _isBlackPlayerTurn = true;
        }

        private static void _PrintRemainingPieces(Player player)
        {
            string Output = "";
            foreach (string piece in player.Pieces)
            {
                // Maybe we should use the String builder for performance improvement
                Output += $"{piece[1]}{piece[2]}|";
            }
            Console.Write($" | Remaining pieces: {Output}");
        }

        private void _PrintPlayerState()
        {
            if (_isBlackPlayerTurn)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Black's Turn");
                _PrintRemainingPieces(_blackPlayer);
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("White's Turn");
                _PrintRemainingPieces(_whitePlayer);
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        public void Play()
        {
            _PrintPlayerState();
            if (_isBlackPlayerTurn)
            {
                if (Board.MakeMove(ref _blackPlayer))
                {
                    _isBlackPlayerTurn = false;
                    _blackPlayer.TurnCount++;
                }
            }
            else
            {
                if (Board.MakeMove(ref _whitePlayer))
                {
                    _isBlackPlayerTurn = true;
                    _whitePlayer.TurnCount++;
                }
            }
            Board.PrintFormatted();
        }

        public bool IsGameOver()
        {
            return Board.IsGameOver();
        }
    }
}