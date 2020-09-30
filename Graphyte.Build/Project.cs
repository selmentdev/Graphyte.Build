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
        public abstract void Configure(ConfiguredTarget target, ConfigurationContext context);

        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }
}
