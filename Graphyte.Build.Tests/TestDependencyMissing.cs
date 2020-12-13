using Graphyte.Build.Evaluation;
using Graphyte.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyMissing
    {
        [ModuleRules]
        public sealed class A : ModuleRules
        {
            public A(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;
                this.ModuleKind = ModuleKind.Runtime;

                this.PublicDependencies.Add(typeof(B));
            }
        }

        [ModuleRules]
        public sealed class B : ModuleRules
        {
            public B(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;
                this.ModuleKind = ModuleKind.Runtime;

                this.PublicDependencies.Add(typeof(C));
            }
        }
        
        [ModuleRules]
        public sealed class C : ModuleRules
        {
            public C(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;
                this.ModuleKind = ModuleKind.Runtime;
            }
        }

        [TargetRules]
        public sealed class SampleTargetRules : TargetRules
        {
            public SampleTargetRules(TargetDescriptor descriptor, TargetContext context)
                : base(descriptor, context)
            {
                this.Type = TargetType.Game;
                this.LinkType = TargetLinkType.Modular;
            }
        }

        [TestMethod]
        public void MissingDependency()
        {
            var modules = new[]
            {
                typeof(A),
                typeof(B),
            };


            var descriptor = new TargetDescriptor(
                TargetPlatform.Windows,
                TargetArchitecture.X64,
                TargetToolchain.MSVC,
                TargetConfiguration.Debug);

            var context = new TargetContext(null, null);

            Assert.ThrowsException<Exception>(() =>
            {
                var e = new EvaluatedTargetRules(typeof(SampleTargetRules), descriptor, context, modules);
                _ = e;
            });
        }
    }
}
