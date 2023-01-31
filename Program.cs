﻿using System.Diagnostics;
using static HiveCore.Utils;

namespace HiveCore
{
    public static class Program
    {
        public static void Main()
        {
            GameCore gameCore = new();
            while (!gameCore.IsGameOver())
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                gameCore.Play();
                stopwatch.Stop();
                PrintRed("Elapsed time: " + stopwatch.Elapsed.Milliseconds + "ms");
            }
        }
    }
}