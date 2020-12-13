using Graphyte.Build.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestPredictiveGuidGenerator
    {
        [TestMethod]
        public void TestEmptyGuid()
        {
            var guid = Tools.MakeGuid("");
            Assert.AreEqual(guid, new Guid("42c4b0e3-fc98-141c-9afb-f4c8996fb924"));
        }

        [TestMethod]
        public void TestNamedGuid()
        {
            var guid = Tools.MakeGuid("Project 1");
            Assert.AreEqual(guid, new Guid("6873ab7d-7218-9174-9519-18a2559fad19"));
        }
    }
}
