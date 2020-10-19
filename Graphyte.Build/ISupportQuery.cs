using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public interface ISupportQuery
    {
        bool IsHostSupported { get; }
        bool IsTargetTupleSupported(TargetTuple targetTuple);
    }
}
