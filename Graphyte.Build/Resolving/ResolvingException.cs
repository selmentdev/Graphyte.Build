using System;
using System.Runtime.Serialization;

namespace Graphyte.Build.Resolving
{
    [Serializable]
    public class ResolvingException : Exception
    {
        public ResolvingException()
        {
        }

        public ResolvingException(string message)
            : base(message)
        {
        }

        public ResolvingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ResolvingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
