namespace Graphyte.Build.Generators
{
    public abstract class BaseGeneratorSettings
        : BaseProfileSection
    {
    }

    public abstract class BaseGenerator
    {
        protected BaseGenerator(Profile profile)
        {
            this.m_Profile = profile;
        }

        protected Profile m_Profile;

        public abstract GeneratorType GeneratorType { get; }
    }
}
