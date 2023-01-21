using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameCore
{
    public class Program
    {
        public static void Main()
        {
            GameCore gameCore = new GameCore();
            while (!gameCore.IsGameOver())
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                gameCore.Play();

                stopwatch.Stop();
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Elapsed time: " + stopwatch.Elapsed.Milliseconds + "ms");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
}