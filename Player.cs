using System;
using System.Collections;
using System.Collections.Generic;

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
            Move move = new Move(input);
            return move;
        }
    }

    public class Move
    {
        private string movingPiece;
        private string destinationSide;
        private string destinationPiece;
        private string _move;

        public string MovingPiece { get; set; }
        public string DestinationSide { get; set; }
        public string DestinationPiece { get; set; }

        public string ToString()
        {
            return _move;
        }

        public Move (string input)
        {
            _move = input;
            MovingPiece = _move.Substring(0, 3);
            if (_move.Length > 3)
            {
                DestinationSide = _move.Substring(3, 2);
                DestinationPiece = _move.Substring(5);
            }
        }

        public bool IsMoveWithDestination()
        {
            return _move.Contains("*");
        }
    }

    public enum Color
    {
        Black,
        White
    }
}