using System;

namespace QuizApp.Exceptions
{
    public class PlatformAccessViolationException : Exception
    {
        public PlatformAccessViolationException()
        {
        }

        public PlatformAccessViolationException(string message)
            : base(message)
        {
        }

        public PlatformAccessViolationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}