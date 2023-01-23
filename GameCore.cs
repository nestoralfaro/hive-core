using System;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable IDE1006 // Private members naming style

namespace GameCore
{
    class GameCore
    {
        private Player _blackPlayer;
        private Player _whitePlayer;
        private bool IsBlackPlayerTurn;
        private Board _board;

        // getter used for testing
        public Board Board { get { return _board; } }

        public GameCore()
        {
            _board = new Board();
            _blackPlayer = new Player('b');
            _whitePlayer = new Player('w');
            IsBlackPlayerTurn = true;
        }

        private void _PrintRemainingPieces(Player player)
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
            if (IsBlackPlayerTurn)
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
            if (IsBlackPlayerTurn)
            {
                // if it is a valid move
                if (_board.MakeMove(ref _blackPlayer))
                    IsBlackPlayerTurn = false;
            }
            else
            {
                if (_board.MakeMove(ref _whitePlayer))
                    IsBlackPlayerTurn = true;
            }
            _board.Print();
        }

        public bool IsGameOver()
        {
            return _board.IsGameOver();
        }
    }
}