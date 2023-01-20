using System;
using System.Collections;
using System.Collections.Generic;

namespace GameCore
{
    public class Program
    {
        public static void Main()
        {
            GameCore gameCore = new GameCore();
            while (!gameCore.IsGameOver())
            {
                gameCore.Play();
            }
        }
    }
}