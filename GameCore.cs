using static HiveCore.Utils;
#pragma warning disable IDE1006 // Private members naming style

namespace HiveCore
{
    class GameCore
    {
        private bool _isFirstPlayersTurn;
        private Color player1 {get;}
        private Color player2 {get;}
        public GameManager GameManager { get; }
        public GameCore()
        {
            GameManager = new GameManager();
            _isFirstPlayersTurn = true;
            player1 = Color.White;
            player2 = Color.Black;
        }

        public void Play()
        {
            if (_isFirstPlayersTurn)
            {
                PrintPlayerHeader(player1, GameManager.Board);
                if (GameManager.HumanMove(player1))
                    _isFirstPlayersTurn = false;
            }
            else
            {
                PrintPlayerHeader(player2, GameManager.Board);
                // if (GameManager.HumanMove(player2))
                if (GameManager.AIMove(player2))
                    _isFirstPlayersTurn = true;
            }
            GameManager.PrintFormatted();
        }

        public bool IsGameOver()
        {
            if (GameManager.IsGameOver())
            {
                Console.WriteLine(GameManager.Winner);
                return true;
            }
            return false;
        }

    }
}