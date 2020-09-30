using System;

namespace Graphyte.Build
{
    public enum ProjectLanguage
    {
        C,
        CPlusPlus,
        CSharp,
    }

    public abstract class Project
    {
        public abstract void Configure(Target target, IContext context);

        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
