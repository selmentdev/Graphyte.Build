using System.Diagnostics;

namespace Graphyte.Base.SourceControl
{
    public static class Git
    {
        private static string ExecuteGitCommand(string arguments)
        {
            using var handle = Process.Start(new ProcessStartInfo()
            {
                FileName = "git",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });

            return handle.StandardOutput.ReadLine();
        }

        /// <summary>
        /// Gets HEAD commit ID.
        /// </summary>
        /// <returns>The HEAD commit ID.</returns>
        public static string GetCommitId()
        {
            return ExecuteGitCommand("log -1 --format=%H");
        }

        /// <summary>
        /// Gets HEAD short commit ID.
        /// </summary>
        /// <returns></returns>
        public static string GetCommitIdShort()
        {
            return ExecuteGitCommand("log -1 --format=%h");
        }

        /// <summary>
        /// Gets current branch name.
        /// </summary>
        /// <returns>The branch name.</returns>
        public static string GetBranchName()
        {
            return ExecuteGitCommand("rev-parse --abbrev-ref HEAD");
        }
    }
}
