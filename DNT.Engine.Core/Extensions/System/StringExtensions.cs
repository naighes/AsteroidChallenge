namespace System
{
    public static class StringExtensions
    {
        public static Boolean IsNotNullOrWhiteSpace(this String obj)
        {
            return !String.IsNullOrWhiteSpace(obj);
        }
    }
}