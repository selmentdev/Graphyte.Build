using Graphyte.Build.Evaluation;

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

        public abstract void Generate(
            string outputPath,
            PlatformType platformType,
            ToolchainType toolchainType,
            Solution solution,
            EvaluatedSolution[] evaluatedSolutions);
    }
}
