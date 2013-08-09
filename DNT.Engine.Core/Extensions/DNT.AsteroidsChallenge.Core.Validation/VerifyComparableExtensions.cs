using System;
using DNT.Engine.Core.Validation;

namespace DNT.AsteroidChallenge.App.App.Core.Validation
{
    public static class VerifyComparableExtensions
    {
        public static void IsBetween<T>(this Verify<T> verify, T start, T end) where T : IComparable<T>
        {
            verify.IsTrue(obj => obj.IsGreaterThanOrEqualTo(start) && obj.IsLowerThanOrEqualTo(start),
                          v => new ArgumentOutOfRangeException(v, String.Format("Value for inspected parameter must be between '{0}' and '{1}'.", start, end)));
        }
    }
}