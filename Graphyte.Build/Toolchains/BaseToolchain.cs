using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Toolchains
{
    public abstract class BaseToolchain
        : ISupportQuery
    {
        /// <summary>
        /// Gets value indicating whether definition is supported on host machine.
        /// </summary>
        public abstract bool IsHostSupported { get; }

        /// <summary>
        /// Gets value indicating whether target tuple is supported by this platform.
        /// </summary>
        /// <param name="targetTuple">Provides a target tuple.</param>
        /// <returns>Returns <c>true</c> when target tuple is supported, <c>false</c> otherwise</returns>
        public abstract bool IsTargetTupleSupported(TargetTuple targetTuple);
    }
}
