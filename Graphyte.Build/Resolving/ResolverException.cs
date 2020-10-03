using System;
using System.Runtime.Serialization;

namespace Graphyte.Build.Resolving
{

    [Serializable]
    public class ResolverException : Exception
    {
        public ResolverException()
        {
        }

        public ResolverException(string message)
            : base(message)
        {
        }

        public ResolverException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ResolverException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
    }
}
