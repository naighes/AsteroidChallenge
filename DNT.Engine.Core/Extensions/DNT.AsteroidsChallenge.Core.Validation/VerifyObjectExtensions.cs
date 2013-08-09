using System;
using DNT.Engine.Core.Validation;

namespace DNT.AsteroidChallenge.App.App.Core.Validation
{
    public static class EnsureObjectExtensions
    {
        public static void IsNotNull<T>(this Verify<T> verify) where T : class
        {
            verify.IsTrue(obj => obj.IsNotNull(), v => new ArgumentNullException(v, "Value for inspected parameter cannot be null."));
        }
    }
}