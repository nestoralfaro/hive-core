namespace HiveCore
{
    public static class Program
    {
        public static void Main()
        {
            GameCore game = new GameCore();
            while (!game.IsGameOver())
            {
                game.Play();
            }
        }
    }
}