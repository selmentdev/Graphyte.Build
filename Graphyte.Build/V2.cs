using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Graphyte.V2.Build
{
    public enum Platform
    {
        Windows,
        UWP,
        Linux,
        Android,
        MacOS,
        IOS,
        NX,
        Orbis,
        Prospero,
        XboxOne,
        Scarlett,
    }

    public static class PlatformExtensions
    {
        private static readonly Platform[] g_DesktopPlatforms = new[]
        {
            Platform.Windows,
            Platform.UWP,
            Platform.MacOS,
            Platform.Linux,
        };

        private static readonly Platform[] g_ConsolePlatforms = new[]
        {
            Platform.NX,
            Platform.Orbis,
            Platform.Prospero,
            Platform.XboxOne,
            Platform.Scarlett,
        };

        private static readonly Platform[] g_MobilePlatforms = new[]
        {
            Platform.Android,
            Platform.IOS,
        };

        private static readonly Platform[] g_ServerPlatforms = new[]
        {
            Platform.Linux,
        };

        public static Platform[] GetPlatforms(PlatformKind kind)
        {
            switch (kind)
            {
                case PlatformKind.Desktop:
                    return g_DesktopPlatforms;
                case PlatformKind.Console:
                    return g_ConsolePlatforms;
                case PlatformKind.Mobile:
                    return g_MobilePlatforms;
                case PlatformKind.Server:
                    return g_ServerPlatforms;
            }

            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        public static bool IsKindOf(this Platform self, PlatformKind kind)
        {
            var platforms = PlatformExtensions.GetPlatforms(kind);
            return platforms.Contains(self);
        }
    }

    public enum PlatformKind
    {
        Desktop,
        Mobile,
        Console,
        Server,
    }

    public enum Architecture
    {
        X64,
        X86,
        ARM,
        ARM64,
        PPC64,
    }

    public enum Compiler
    {
        Default,
        MSVC,
        ClangCL,
        Clang,
        GCC,
        Intel,
    }

    public enum Configuration
    {
        Debug,          // Game non-optimized, Engine non-optimized
        DebugGame,      // Game non-optimized, Engine optimized,
        Development,    // Game mostly-optimized, Engine optimized,
        Testing,        // Game optimized, Engine optimized, developer tools enabled
        Release,        // Game optimized, Engine optimized,
    }

    /// <summary>
    /// Specifies how target is being linked against other targets.
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// Lets platform decide how to link target.
        /// </summary>
        /// <remarks>
        /// This value is replaced during dependency resolving to specific link type.
        /// </remarks>
        Default,

        /// <summary>
        /// Target is linked as shared library.
        /// </summary>
        SharedLibrary,

        /// <summary>
        /// Target is linked as static library.
        /// </summary>
        StaticLibrary,

        /// <summary>
        /// Target does not produce any library, but may provide transient dependencies.
        /// </summary>
        HeaderLibrary,

        /// <summary>
        /// Target is a common application.
        /// </summary>
        Application,

        /// <summary>
        /// Target is a game application.
        /// </summary>
        GameApplication,
    }

    public static class TargetTypeExtensions
    {
        public static bool IsImportable(this TargetType self)
        {
            switch (self)
            {
                case TargetType.Default:
                    throw new InvalidEnumArgumentException(nameof(self), (int)self, typeof(TargetType));

                case TargetType.StaticLibrary:
                case TargetType.HeaderLibrary:
                    return true;

                case TargetType.SharedLibrary:
                case TargetType.Application:
                case TargetType.GameApplication:
                    return false;
            }

            throw new ArgumentOutOfRangeException(nameof(self));
        }
    }

    /// <summary>
    /// Specifies component type.
    /// </summary>
    public enum ComponentType
        // TODO: This is somewhat mixed up. Clarify this.
        //
        // We want to determine two things here:
        //  - what kind of build we are doing
        //  - what kind of component do we have
    {
        /// <summary>
        /// Target compiles to game application.
        /// </summary>
        Game,

        /// <summary>
        /// Target compiles to editor application.
        /// </summary>
        Editor,

        /// <summary>
        /// Target compiles to game without server code.
        /// </summary>
        Client,

        /// <summary>
        /// Target compiles to game withoout client code.
        /// </summary>
        Server,

        /// <summary>
        /// Target compiles to developer application.
        /// </summary>
        Developer,

        /// <summary>
        /// Target compiles to engine module.
        /// </summary>
        Engine,

        /// <summary>
        /// Target compiles to engine plugin.
        /// </summary>
        Plugin,

        /// <summary>
        /// Target specifies third-party SDK.
        /// </summary>
        ThirdPartySdk,
    }

    [Serializable]
    public class Profile
    {
        public string Prefix { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Platform Platform { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Architecture Architecture { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Compiler Compiler { get; set; }

        [DefaultValue(false)]
        public bool? EnableAddressSanitizer { get; set; }

        [DefaultValue(false)]
        public bool? EnableThreadSanitizer { get; set; }

        [DefaultValue(false)]
        public bool? EnableUndefinedBehaviorSanitizer { get; set; }

        [DefaultValue(false)]
        public bool? EnableUnityBuild { get; set; }

        [DefaultValue(false)]
        public bool? EnableDistributedBuild { get; set; }

        [DefaultValue(false)]
        public bool? EnableMemoryTracing { get; set; }

        [DefaultValue(false)]
        public bool? EnableProfiling { get; set; }

        public bool? EnableStaticAnalyzer { get; set; }

        public bool? EnableClangTimeTrace { get; set; }

        public string ProductName { get; set; }
        public string CompanyName { get; set; }

        [DefaultValue(false)]
        public bool? PgoOptimize { get; set; }

        [DefaultValue(false)]
        public bool? PgoProfile { get; set; }

        public string PgoDirectory { get; set; }

        public string PgoPrefix { get; set; }
    }

    public static class ProfileSerializer
    {
        private static readonly JsonSerializerOptions g_SerializerOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            WriteIndented = true,
            IgnoreNullValues = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            IgnoreReadOnlyProperties = true,
        };

        public static Profile Deserialize(string content)
        {
            return JsonSerializer.Deserialize<Profile>(content, ProfileSerializer.g_SerializerOptions);
        }

        public static string Serialize(Profile profile)
        {
            return JsonSerializer.Serialize<Profile>(profile, ProfileSerializer.g_SerializerOptions);
        }
    }
}

namespace Graphyte.V2.Build
{
    public static class Executor
    {
        public static int Main(string[] args)
        {
            //
            // Setup tracing.
            //

            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.WriteLine("Graphyte Build");

#if DEBUG
            for (var i = 0; i < args.Length; i++)
            {
                Debug.WriteLine($@"cmd[{i}] = ""{args[i]}""");
            }
#endif


            //
            // Start ticking
            //

            var start = Stopwatch.StartNew();


            //
            // Setup GC for our use-case
            //

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            {
                var profile = new Profile()
                {
                    Architecture = Architecture.X64,
                    Platform = Platform.Windows,
                    Compiler = Compiler.ClangCL,
                    EnableUnityBuild = true,
                    EnableDistributedBuild = true,
                    ProductName = "Rail Simulator",
                    CompanyName = "Graphyte",
                };

                File.WriteAllText("win64.json", ProfileSerializer.Serialize(profile));
            }


            //
            // Report results.
            //

            start.Stop();
            Trace.WriteLine($@"execution time: {start.Elapsed.TotalSeconds:0.0.00}");
            return 0;
        }
    }
}

// Solutions and Projects
namespace Graphyte.V2.Build
{
    public interface ICompilerContext
    {
    }

    public sealed class CppCompilerContext : ICompilerContext
    {
        public bool EnableRtti = false;
        public bool EnableExceptions = false;
        public bool EnableAvx = false;
        public bool EnableInlining = false;
        public bool EnableSecurityChecks = true;
        public bool EnableUnityBuild = true;
        public bool OptimizeCode = false;
        public bool OptimizeForSize = false;
        public bool UseStaticRuntime;
        public bool UseDebugRuntime;
        public bool CreateDebugInfo;
        public bool IsCompilingLibrary;
        public bool OmitFramePointers;
        public bool UsePdbFiles;
        public bool EnableIncrementalLinking;
        public bool EnableLinkTimeCodeGeneration;
    }

    public readonly struct TargetTuple
    {
        public readonly Platform Platform;
        public readonly Architecture Architecture;
        public readonly Compiler Compiler;
        public readonly Configuration Configuration;
    }

    public interface IPlatform
    {
        void PreConfigure(Target target);
        void PostConfigure(Target target);

        void Configure(CppCompilerContext context);
    }

    public interface IToolchain
    {
        void PreConfigure(Target target);
        void PostConfigure(Target target);

        void Configure(CppCompilerContext context);
    }

    public abstract class Solution
    {
        public virtual void PreConfigure(Target target)
        {
        }

        public virtual void PostConfigure(Target target)
        {
        }
    }

    public abstract class Project
    {
        private string m_Name;
        public string Name
        {
            get
            {
                if (this.m_Name != null)
                {
                    return this.m_Name;
                }

                return this.GetType().Name;
            }
            protected set => this.m_Name = value;
        }

        public string ProjectFileName { get; protected set; }

        public abstract void Configure(Target target);
    }

    public sealed class Target
    {
        /// <summary>
        /// Source project for which these target rules are configured.
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Configuration target tuple for target rules.
        /// </summary>
        public TargetTuple TargetTuple { get; }

        /// <summary>
        /// Specifies how current target will be linked.
        /// </summary>
        public TargetType TargetType { get; set; } = TargetType.Default;

        /// <summary>
        /// Specifies in what part of engine that target is placed.
        /// </summary>
        public ComponentType ComponentType { get; set; } = ComponentType.Game;

        public Target(Project project, TargetTuple targetTuple)
        {
            this.Project = project;
            this.TargetTuple = targetTuple;
        }
    }
}

namespace Graphyte.V2.Build
{
}

namespace Graphyte.V2.Build
{
}

namespace Graphyte.V2.Build
{
}

namespace Graphyte.V2.Build
{
    internal class SomeProject : Project
    {
        public override void Configure(Target target)
        {
            target.TargetType = TargetType.HeaderLibrary;
        }
    }
}
