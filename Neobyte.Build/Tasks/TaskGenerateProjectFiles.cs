using Neobyte.Build.Generators;
using System;

namespace Neobyte.Build.Tasks
{
    public abstract class TaskBase
    {
        public abstract void Execute();
    }

    public sealed class TaskGenerateProjectFiles
        : TaskBase
    {
        //private readonly GeneratorBase m_Generator;
        //private readonly

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
