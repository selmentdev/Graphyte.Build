using System.Runtime;

namespace Graphyte.Build.Application
{
    public class Program
    {
        public static void Main(string[] _)
        {
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }
    }
}
