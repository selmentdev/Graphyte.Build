using Neobyte.Build.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

[assembly: Neobyte.Build.Core.TypesProvider]

namespace Neobyte.Build.Tests
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

        public class Options
        {
            public string SingleString;
            public bool SingleBool;
            public FileInfo SingleFileInfo;
            public int? SingleInt;
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
            var options = CommandLineParser.Parse<Options>(Array.Empty<string>());

            Assert.IsNull(options.SingleString);
            Assert.IsFalse(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.IsNull(options.SingleInt);
            Assert.AreEqual(0.0f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.None, options.SingleEnum);
        }

        [TestMethod]
        public void SingleStringValue()
        {
            var options = CommandLineParser.Parse<Options>(new[]
            {
                @"-SingleString:""Hello World"""
            });

            Assert.AreEqual("Hello World", options.SingleString);
            Assert.IsFalse(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.IsNull(options.SingleInt);
            Assert.AreEqual(0.0f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.None, options.SingleEnum);
        }

        [TestMethod]
        public void ArrayParamsAreNotSupported()
        {
            Assert.ThrowsException<Exception>(() => CommandLineParser.Parse<CommandLineParamsWithArray>(new[] {
                @"-MultipleString=""abcd""",
                @"-MultipleString:""efgh""",
            }));
        }

        [TestMethod]
        public void SpecifyingSingleValueFails()
        {
            Assert.ThrowsException<Exception>(() => CommandLineParser.Parse<Options>(new[]
            {
                @"-SingleBool:true",
                @"-SingleBool:false",
            }));
        }

        [TestMethod]
        public void SpecifyingMultipleValuesSucceeds()
        {
            var options = CommandLineParser.Parse<Options>(new[]
            {
                @"-SingleBool",
                @"-SingleFloat:3.14",
                @"-SingleEnum:OptionA",
            });

            Assert.IsNull(options.SingleString);
            Assert.IsTrue(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.IsNull(options.SingleInt);
            Assert.AreEqual(3.14f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.OptionA, options.SingleEnum);
        }

        [TestMethod]
        public void ParsingOptionalValuesWithValue()
        {
            var options = CommandLineParser.Parse<Options>(new[] {
                @"-SingleFloat=42",
                @"-SingleInt:44",
            });

            Assert.IsNull(options.SingleString);
            Assert.IsFalse(options.SingleBool);
            Assert.IsNull(options.SingleFileInfo);
            Assert.AreEqual(44, options.SingleInt);
            Assert.AreEqual(42.0f, options.SingleFloat);
            Assert.AreEqual(SomeEnum.None, options.SingleEnum);
        }
    }
}
