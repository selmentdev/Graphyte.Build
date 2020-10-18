using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestCommandLineParsing
    {
        public enum SomeEnum
        {
            None,
            OptionA,
            OptionB,
            OptionC,
        }

        public class CommandLineParams
        {
            public string SingleString;
            public bool SingleBool;
            public FileInfo SingleFileInfo;
            public int SingleInt;
            public float SingleFloat;
            public SomeEnum SingleEnum;
        }

        public class CommandLineParamsWithArray
        {
            public string SingleString;
            public string[] MultipleString;
        }

        [TestMethod]
        public void EmptyCommandLine()
        {
            var options = CommandLineParser.Parse<CommandLineParams>(new string[] { });

            Assert.IsNull(options.SingleString);
            Assert.IsFalse(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.AreEqual(0.0f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.None, options.SingleEnum);
        }

        [TestMethod]
        public void SingleStringValue()
        {
            var options = CommandLineParser.Parse<CommandLineParams>(new[]
            {
                @"-SingleString:""Hello World"""
            });

            Assert.AreEqual("Hello World", options.SingleString);
            Assert.IsFalse(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.AreEqual(0.0f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.None, options.SingleEnum);
        }

        [TestMethod]
        public void ArrayParamsAreNotSupported()
        {
            Assert.ThrowsException<CommandLineParsingException>(() => CommandLineParser.Parse<CommandLineParamsWithArray>(new[] {
                @"-MultipleString=""abcd""",
                @"-MultipleString:""efgh""",
            }));
        }

        [TestMethod]
        public void SpecifyingSingleValueFails()
        {
            Assert.ThrowsException<CommandLineParsingException>(() => CommandLineParser.Parse<CommandLineParams>(new[]
            {
                @"-SingleBool:true",
                @"-SingleBool:false",
            }));
        }

        [TestMethod]
        public void SpecifyingMultipleValuesSucceeds()
        {
            var options = CommandLineParser.Parse<CommandLineParams>(new[]
            {
                @"-SingleBool",
                @"-SingleFloat:3.14",
            });

            Assert.IsNull(options.SingleString);
            Assert.IsTrue(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.AreEqual(3.14f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.None, options.SingleEnum);
        }
    }
}
