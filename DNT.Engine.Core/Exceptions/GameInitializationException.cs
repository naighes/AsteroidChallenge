using System;

namespace DNT.Engine.Core.Exceptions
{
    public class GameInitializationException : Exception
    {
        public GameInitializationException()
            : this(null)
        {
        }

        public GameInitializationException(String message)
            : this(message, null)
        {
        }

        public GameInitializationException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}