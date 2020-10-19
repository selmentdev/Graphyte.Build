using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Generators
{
    public abstract class BaseGenerator
        : ISupportQuery
    {
        public abstract bool IsHostSupported { get; }

        public abstract bool IsTargetTupleSupported(TargetTuple targetTuple);
    }
}
