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
            _whitePlayer = new AI(Color.White);
            _isBlackPlayerTurn = false;
        }

        public void Play()
        {
            if (_isBlackPlayerTurn)
            {
                PrintPlayerHeader(_blackPlayer);
                if (Game.MakeMove(ref _blackPlayer))
                {
                    _isBlackPlayerTurn = false;
                    ++_blackPlayer.TurnCount;
                }
            }
            else
            {
                PrintPlayerHeader(_whitePlayer);
                // if (Game.MakeMove(ref _whitePlayer))
                if (_whitePlayer.MakeMove(ref Game.Board, ref _blackPlayer))
                {
                    _isBlackPlayerTurn = true;
                    ++_whitePlayer.TurnCount;
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