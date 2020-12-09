namespace Graphyte.Build
{
    public sealed class SourceList
    {
        public int MergeFiles { get; init; }
        public string[] InputPaths { get; init; }
        public string InputPattern { get; init; }
        public string[] ExcludePaths { get; init; }
        public string ExcludePattern { get; init; }
        public string[] Files { get; init; }
    }
}
