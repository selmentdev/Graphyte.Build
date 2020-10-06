using System;
using System.Runtime.Serialization;

namespace Graphyte.Build
{
    [Serializable]
    public class ConfigurationFailedException : Exception
    {
        public ConfigurationFailedException()
        {
        }

        public ConfigurationFailedException(string message)
            : base(message)
        {
        }

        public ConfigurationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ConfigurationFailedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
