namespace DNT.AsteroidChallenge.App
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(String[] args)
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}