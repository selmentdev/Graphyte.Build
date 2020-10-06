using Graphyte.Build.Profiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json.Serialization;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestProfileSerialization
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum SomeEnum
        {
            Value1,
            Value2,
            Value3,
        }

        public sealed class TestProfileSerialization_Section1
            : BaseProfileSection
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }
            public SomeEnum[] Enums { get; set; }
        }

        public sealed class TestProfileSerialization_Section2
            : BaseProfileSection
        {
            public SomeEnum Value { get; set; }
            public int ReadonlyValue => 42;
            public string DefaultValue { get; set; } = "SomeDefaultValue";
            public bool SomeValue { get; set; }
            public int? OptionalValue { get; set; }
        }

        public sealed class TestProfileSerialization_Section3
            : BaseProfileSection
        {
            public string Test { get; set; }
            public float Something { get; set; }
        }

        [TestMethod]
        public void TestDeserialization()
        {
            var content = @"{
                ""TestProfileSerialization_Section1"": {
                    ""Property1"": ""Hello"",
                    ""Property2"": 42,
                    ""Enums"": [
                        ""Value1"",
                        ""Value2"",
                        ""Value1"",
                        ""Value3"",
                    ],
                },
                ""TestProfileSerialization_Section2"": {
                    ""Value"": ""Value3"",
                    ""DefaultValue"": ""Override"",
                    ""SomeValue"": true,
                    ""OptionalValue"": 43,
                },
            }";

            var profile = Profile.Parse(content);

            Assert.AreEqual(2, profile.Sections.Count);
            var section1 = profile.GetSection<TestProfileSerialization_Section1>();
            Assert.IsNotNull(section1);
            Assert.AreEqual("Hello", section1.Property1);
            Assert.AreEqual(42, section1.Property2);
            Assert.IsNotNull(section1.Enums);
            Assert.AreEqual(4, section1.Enums.Length);
            Assert.AreEqual(SomeEnum.Value1, section1.Enums[0]);
            Assert.AreEqual(SomeEnum.Value2, section1.Enums[1]);
            Assert.AreEqual(SomeEnum.Value1, section1.Enums[2]);
            Assert.AreEqual(SomeEnum.Value3, section1.Enums[3]);

            var section2 = profile.GetSection<TestProfileSerialization_Section2>();
            Assert.IsNotNull(section2);
            Assert.AreEqual(SomeEnum.Value3, section2.Value);
            Assert.AreEqual("Override", section2.DefaultValue);
            Assert.AreEqual(true, section2.SomeValue);
            Assert.IsTrue(section2.OptionalValue.HasValue);
            Assert.AreEqual(43, section2.OptionalValue.Value);

            var section3 = profile.GetSection<TestProfileSerialization_Section3>();
            Assert.IsNull(section3);
        }

        [TestMethod]
        public void TestEmptyProfile()
        {
            var profile = Profile.Parse(@"{}");
            Assert.AreEqual(0, profile.Sections.Count);
        }
    }
}
