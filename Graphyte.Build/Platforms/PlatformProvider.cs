using Graphyte.Build.Core;
using Graphyte.Build.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Graphyte.Build.Platforms
{
    public sealed class PlatformProvider
    {
        public PlatformFactory[] Factories { get; }

        public PlatformProvider()
        {
            var providers = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDefined(typeof(TypesProviderAttribute)))
                .SelectMany(x => x.GetTypes())
                .Where(IsValidType)
                .Select(x => Activator.CreateInstance(x))
                .Cast<PlatformFactoryProvider>();

            this.Factories = providers
                .SelectMany(x => x.Provide())
                .ToArray();
        }

        private static bool IsValidType(Type type)
        {
            return type.IsClass
                && !type.IsAbstract
                && type.IsSealed
                && type.IsDefined(typeof(PlatformFactoryProviderAttribute))
                && type.IsVisible
                && type.IsSubclassOf(typeof(PlatformFactoryProvider));
        }

        [Conditional("DEBUG")]
        public void Dump()
        {
            Trace.WriteLine("Platforms:");
            Trace.Indent();

            foreach (var factory in this.Factories)
            {
                Trace.WriteLine(factory);
            }

            Trace.Unindent();
        }

        public PlatformFactory[] GetPlatformFactories(
            TargetPlatform targetPlatform,
            TargetToolchain targetToolchain)
        {
            return this.Factories
                .Where(x => x.TargetPlatform == targetPlatform && x.TargetToolchain == targetToolchain)
                .ToArray();
        }
    }
}
