using System.Linq;

namespace System.Collections
{
    public static class EnumerableExtensions
    {
        public static Boolean IsNullOrEmpty(this IEnumerable obj)
        {
            return obj.IsNull() || !obj.Cast<Object>().Any();
        }

        public static Boolean IsNotNullOrEmpty(this IEnumerable obj)
        {
            return !obj.IsNull() && obj.Cast<Object>().Any();
        }
    }
}