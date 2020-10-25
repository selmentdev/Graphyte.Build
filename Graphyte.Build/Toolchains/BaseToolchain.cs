using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Toolchains
{
    public abstract class BaseToolchain
    {
        public abstract bool IsHostSupported { get; }

        public abstract void Initialize(Profile profile);
    }
}
