using System;
using System.Diagnostics;
using System.Linq;

namespace Graphyte.Build.Toolchains
{
    public sealed class ToolchainsProvider
    {
        private static Type[] Discover()
        {
            var baseType = typeof(BaseToolchain);

            var result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsSubclassOf(baseType) && x.IsClass && !x.IsAbstract && x.IsVisible && x.IsSealed)
                .ToArray();

#if DEBUG
            foreach (var platform in result)
            {
                Debug.Assert(platform.Name.EndsWith("Toolchain"));
            }
#endif
            return result;
        }

        public Type[] Toolchains { get; } = ToolchainsProvider.Discover();

        public BaseToolchain Create(ToolchainType toolchain)
        {
            var name = $@"{toolchain}Toolchain";
            var type = this.Toolchains.FirstOrDefault(x => x.Name == name);

            if (type != null)
            {
                return (BaseToolchain)Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
