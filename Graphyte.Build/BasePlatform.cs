namespace Graphyte.Build
{
    public abstract class BasePlatformSettings
        : BaseProfileSection
    {
    }

    public abstract class BasePlatform
    {
        protected BasePlatform(
            Profile profile,
            ArchitectureType architectureType)
        {
            this.ArchitectureType = architectureType;
            this.m_Profile = profile;
        }

        protected Profile m_Profile;

        public ArchitectureType ArchitectureType { get; }

        public abstract PlatformType PlatformType { get; }

        public abstract bool IsPlatformKind(PlatformKind platformKind);

        public abstract string AdjustTargetName(string name, TargetType targetType);

        public string[] IncludePaths { get; protected set; }

        public string[] LibraryPaths { get; protected set; }
    }
}
