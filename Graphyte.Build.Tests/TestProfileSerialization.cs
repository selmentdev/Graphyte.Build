using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestProfileSerialization
    {
        [TestMethod]
        public void EmptyProfile()
        {
            var profile = new Profile();

            var json = ProfileSerializer.Serialize(profile);

            Assert.IsTrue(json.Length > 0);
        }

        [TestMethod]
        public void WithContents()
        {
            var profile = new Profile
            {
                EnableAddressSanitizer = true,
                EnableThreadSanitizer = true,
                EnableUndefinedBehaviorSanitizer = true,
                Targets = new List<TargetTuple>()
                {
                    new TargetTuple(
                        PlatformType.Windows,
                        ArchitectureType.X64,
                        ToolsetType.MSVC),
                    new TargetTuple(
                        PlatformType.Windows,
                        ArchitectureType.Arm64,
                        ToolsetType.MSVC),
                    new TargetTuple(
                        PlatformType.Windows,
                        ArchitectureType.X64,
                        ToolsetType.Clang),
                    new TargetTuple(
                        PlatformType.Windows,
                        ArchitectureType.Arm64,
                        ToolsetType.Clang),
                }
            };


            var json = ProfileSerializer.Serialize(profile);

            Assert.IsTrue(json.Length > 0);

            var deserialized = ProfileSerializer.Deserialize(json);
            Assert.IsTrue(deserialized.EnableAddressSanitizer);
            Assert.IsTrue(deserialized.EnableThreadSanitizer);
            Assert.IsTrue(deserialized.EnableUndefinedBehaviorSanitizer);
            Assert.IsTrue(deserialized.Targets.Count == 4);
            Assert.AreEqual("Windows-X64-MSVC", deserialized.Targets[0].ToString());
            Assert.AreEqual("Windows-Arm64-MSVC", deserialized.Targets[1].ToString());
            Assert.AreEqual("Windows-X64-Clang", deserialized.Targets[2].ToString());
            Assert.AreEqual("Windows-Arm64-Clang", deserialized.Targets[3].ToString());
        }

        [TestMethod]
        public void ReadJson()
        {
            var content = @"{""Targets"":[""Windows-X64-MSVC"",""Windows-Arm64-MSVC"",""Windows-X64-Clang"",""Windows-Arm64-Clang""],""EnableAddressSanitizer"":true,""EnableThreadSanitizer"":true,""EnableUndefinedBehaviorSanitizer"":true}";

            var deserialized = ProfileSerializer.Deserialize(content);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(deserialized.EnableAddressSanitizer);
            Assert.IsTrue(deserialized.EnableThreadSanitizer);
            Assert.IsTrue(deserialized.EnableUndefinedBehaviorSanitizer);
            Assert.IsTrue(deserialized.Targets.Count == 4);
            Assert.AreEqual("Windows-X64-MSVC", deserialized.Targets[0].ToString());
            Assert.AreEqual("Windows-Arm64-MSVC", deserialized.Targets[1].ToString());
            Assert.AreEqual("Windows-X64-Clang", deserialized.Targets[2].ToString());
            Assert.AreEqual("Windows-Arm64-Clang", deserialized.Targets[3].ToString());
        }
    }
}
