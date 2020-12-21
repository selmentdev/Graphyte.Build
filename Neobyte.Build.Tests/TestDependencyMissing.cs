using Neobyte.Build.Evaluation;
using Neobyte.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Neobyte.Build.Tests
{
    [TestClass]
    public class TestDependencyMissing
    {
        [ModuleRules]
        public sealed class A
            : ModuleRules
        {
            public A(TargetRules target)
                : base(target)
            {
                this.Language = ModuleLanguage.CPlusPlus;
                this.Type = ModuleType.SharedLibrary;
                this.Kind = ModuleKind.Runtime;

                this.PublicDependencies.Add(typeof(B));
            }
        }

        [ModuleRules]
        public sealed class B
            : ModuleRules
        {
            public B(TargetRules target)
                : base(target)
            {
                this.Language = ModuleLanguage.CPlusPlus;
                this.Type = ModuleType.SharedLibrary;
                this.Kind = ModuleKind.Runtime;

                this.PublicDependencies.Add(typeof(C));
            }
        }

        [ModuleRules]
        public sealed class C
            : ModuleRules
        {
            public C(TargetRules target)
                : base(target)
            {
                this.Language = ModuleLanguage.CPlusPlus;
                this.Type = ModuleType.SharedLibrary;
                this.Kind = ModuleKind.Runtime;
            }
        }

        [TargetRules]
        public sealed class SampleTargetRules
            : TargetRules
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
                new ModuleRulesMetadata(typeof(A)),
                new ModuleRulesMetadata(typeof(B)),
            };

            var target = new TargetRulesMetadata(typeof(SampleTargetRules));

            var descriptor = new TargetDescriptor(
                TargetPlatform.Windows,
                TargetArchitecture.X64,
                TargetToolchain.MSVC,
                TargetConfiguration.Debug,
                TargetFlavor.Game);

            var context = new TargetContext(null, null);

            Assert.ThrowsException<Exception>(() => new EvaluatedTargetRules(target, descriptor, context, modules));
        }
    }
}
