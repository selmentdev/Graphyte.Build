using System.Diagnostics;

namespace Graphyte.Build
{
    public static class Git
    {
        private static string ExecuteGitCommand(string arguments) {
            using var handle = Process.Start(new ProcessStartInfo() {
                FileName = "git",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });

            return handle.StandardOutput.ReadLine();
        }

        public static string GetCommitId(){
            return ExecuteGitCommand("log -1 --format=%H");
        }

        public static string GetCommitIdShort() {
            return ExecuteGitCommand("log -1 --format=%h");
        }

        public static string GetBranchName() {
            return ExecuteGitCommand("rev-parse --abbrev-ref HEAD");
        }
    }
}
