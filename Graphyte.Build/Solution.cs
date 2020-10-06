using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build
{
    public abstract class Solution
    {
        public virtual void PreConfigure(Target target)
        {
        }

        public virtual void PostConfigure(Target target)
        {
        }
    }
}
