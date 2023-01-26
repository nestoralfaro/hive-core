using static GameCore.Utils;
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
            _isBlackPlayerTurn = false;
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

        public void Play()
        {
            if (_isBlackPlayerTurn)
            {
                PrintPlayerHeader(_blackPlayer);
                if (Board.MakeMove(ref _blackPlayer))
                {
                    _isBlackPlayerTurn = false;
                    _blackPlayer.TurnCount++;
                }
            }
            else
            {
                PrintPlayerHeader(_whitePlayer);
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