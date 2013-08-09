using System;

namespace DNT.Engine.Core.Validation
{
    public static class VerifyStringExtensions
    {
        public static void IsNotNullOrWhiteSpace(this Verify<String> verify)
        {
            verify.IsTrue(obj => obj.IsNotNullOrWhiteSpace(), v => new ArgumentNullException(v, "Value for inspected string parameter cannot be null, empty or white space."));
        }
    }
}