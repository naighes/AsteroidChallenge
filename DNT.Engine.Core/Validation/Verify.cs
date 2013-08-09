namespace DNT.Engine.Core.Validation
{
    public static class Verify
    {
        public static Verify<T> That<T>(T obj)
        {
            return new Verify<T>(obj);
        }
    }
}