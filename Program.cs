using System.Diagnostics;
using static GameCore.Utils;

namespace GameCore
{
    public class Program
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