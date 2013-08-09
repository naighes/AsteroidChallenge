using System;

namespace DNT.Engine.Core.Validation
{
    public sealed class Verify<T>
    {
        internal Verify(T obj)
        {
            _inspectedParameter = obj;
        }

        private readonly T _inspectedParameter;

        public Verify<T> Named(String parameterName)
        {
            _parameterName = parameterName;
            return this;
        }

        private String _parameterName;

        private String GetFullErrorMessage(String message)
        {
            return String.Format("Parameter validation failure.{0}{0}{1}{0}Parameter name: '{2}'{0}",
                                 Environment.NewLine,
                                 message,
                                 _parameterName ?? "<no-name-supplied>");
        }

        public void IsTrue(Predicate<T> predicate)
        {
            IsTrue(predicate, v => new ArgumentException("The supplied condition is not met, condition was expected to be true.", v));
        }

        internal void IsTrue(Predicate<T> predicate, Func<String, Exception> func)
        {
            if (!predicate(_inspectedParameter))
                Throw(func(_parameterName));
        }

        private static void Throw(Exception error)
        {
            if (error.IsNull())
                throw new ArgumentNullException("error");

            throw error;
        }
    }
}