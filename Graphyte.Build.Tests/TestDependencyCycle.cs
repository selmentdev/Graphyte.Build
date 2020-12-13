using Graphyte.Build.Evaluation;
using Graphyte.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestDependencyCycle
    {
        [ModuleRules]
        public sealed class A : ModuleRules
        {
            public A(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(B));
            }
        }

        [ModuleRules]
        public sealed class B : ModuleRules
        {
            public B(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(C));
            }
        }

        [ModuleRules]
        public sealed class C : ModuleRules
        {
            public C(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(D));
            }
        }

        [ModuleRules]
        public sealed class D : ModuleRules
        {
            public D(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(E));
            }
        }

        [ModuleRules]
        public sealed class E : ModuleRules
        {
            public E(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(F));
            }
        }

        [ModuleRules]
        public sealed class F : ModuleRules
        {
            public F(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(G));
            }
        }

        [ModuleRules]
        public sealed class G : ModuleRules
        {
            public G(TargetRules targetRules) : base(targetRules)
            {
                this.ModuleKind = ModuleKind.Runtime;
                this.ModuleLanguage = ModuleLanguage.CPlusPlus;
                this.ModuleType = ModuleType.SharedLibrary;

                this.PublicDependencies.Add(typeof(A));
            }
        }

        [TargetRules]
        public sealed class SampleTargetRules : TargetRules
        {
            public SampleTargetRules(TargetDescriptor descriptor, TargetContext context) : base(descriptor, context)
            {
            }
        }

        [TestMethod]
        public void Cycle()
        {
            var modules = new[]
            {
                typeof(A),
                typeof(B),
                typeof(C),
                typeof(D),
                typeof(E),
                typeof(F),
                typeof(G),
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
