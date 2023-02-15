namespace HiveCore
{
    public static class Program
    {
        public static void Main()
        {
            GameCore game = new();
            while (!game.IsGameOver())
            {
                game.Play();
            }
        }
    }
}