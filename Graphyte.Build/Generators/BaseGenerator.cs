using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Generators
{
    public abstract class BaseGenerator
    {
        public abstract bool IsHostSupported { get; }

        public abstract void Initialize(Profile profile);
    }
}
