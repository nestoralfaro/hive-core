using static GameCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    class GameCore
    {
        private Player _blackPlayer;
        private Player _whitePlayer;
        private bool _isBlackPlayerTurn;

        public GameManager Game { get; }
        public GameCore()
        {
            Game = new GameManager();
            _blackPlayer = new Player(Color.Black);
            _whitePlayer = new Player(Color.White);
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
                if (Game.MakeMove(ref _blackPlayer))
                {
                    _isBlackPlayerTurn = false;
                    _blackPlayer.TurnCount++;
                }
            }
            else
            {
                PrintPlayerHeader(_whitePlayer);
                if (Game.MakeMove(ref _whitePlayer))
                {
                    _isBlackPlayerTurn = true;
                    _whitePlayer.TurnCount++;
                }
            }
            Game.PrintFormatted();
        }

        public bool IsGameOver()
        {
            return Game.IsGameOver();
        }
    }
}