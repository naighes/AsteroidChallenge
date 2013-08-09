namespace System
{
    public static class ObjectExtensions
    {
        public static Boolean IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        public static Boolean IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }
    }
}