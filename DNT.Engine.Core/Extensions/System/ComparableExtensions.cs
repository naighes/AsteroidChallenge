namespace System
{
    public static class ComparableExtensions
    {
        public static Boolean IsLowerThan<T>(this T value, T target) where T : IComparable<T>
        {
            return value.CompareTo(target) < 0;
        }

        public static Boolean IsLowerThanOrEqualTo<T>(this T value, T target) where T : IComparable<T>
        {
            return value.CompareTo(target) <= 0;
        }

        public static Boolean IsGreaterThan<T>(this T value, T target) where T : IComparable<T>
        {
            return value.CompareTo(target) > 0;
        }

        public static Boolean IsGreaterThanOrEqualTo<T>(this T value, T target) where T : IComparable<T>
        {
            return value.CompareTo(target) >= 0;
        }
    }
}