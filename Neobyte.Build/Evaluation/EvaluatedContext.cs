using Neobyte.Build.Framework;
using Neobyte.Build.Platforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neobyte.Build.Evaluation
{
    public sealed class EvaluatedContext
    {
        private static readonly TargetConfiguration[] g_Configurations;

        /// <summary>
        /// Gets list of available configurations.
        /// </summary>
        public static IEnumerable<TargetConfiguration> Configurations
            => EvaluatedContext.g_Configurations;

        private static readonly TargetFlavor[] g_Flavors;

        /// <summary>
        /// Gets list of available flavors.
        /// </summary>
        public static IEnumerable<TargetFlavor> Flavors
            => EvaluatedContext.g_Flavors;


        static EvaluatedContext()
        {
            EvaluatedContext.g_Configurations = Enum
                .GetValues<TargetConfiguration>()
                .ToArray();

            EvaluatedContext.g_Flavors = Enum
                .GetValues<TargetFlavor>()
                .ToArray();
        }

        public EvaluatedContext(
            IEnumerable<TargetRulesMetadata> targets,
            IEnumerable<ModuleRulesMetadata> modules,
            Profile profile,
            IEnumerable<PlatformFactory> platforms)
        {
            foreach (var factory in platforms)
            {
                foreach (var target in targets)
                {
                    foreach (var flavor in Flavors)
                    {
                        foreach (var configuration in Configurations)
                        {
                            var platform = factory.CreatePlatform(profile);
                            var toolchain = factory.CreateToolchain(profile);

                            var context = new TargetContext(platform, toolchain);

                            var descriptor = new TargetDescriptor(
                                factory.Platform,
                                factory.Architecture,
                                factory.Toolchain,
                                configuration,
                                flavor);

                            var evaluated = new EvaluatedTargetRules(
                                target,
                                descriptor,
                                context,
                                modules.ToArray());

                            _ = evaluated;
                        }
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        public void Dump()
        {
        }
    }
}
