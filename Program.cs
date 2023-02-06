using System.Diagnostics;
using static HiveCore.Utils;

namespace HiveCore
{
    public static class Program
    {
        public static void Main()
        {
            GameCore game = new();
            while (!game.IsGameOver())
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                game.Play();
                stopwatch.Stop();
                PrintRed("Elapsed time: " + stopwatch.Elapsed.Milliseconds + "ms");
            }
        }
    }
}