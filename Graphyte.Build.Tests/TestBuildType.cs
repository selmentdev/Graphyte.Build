using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graphyte.Build.Tests
{
    [TestClass]
    public class TestBuildType
    {
        [TestMethod]
        public void CheckComparison()
        {
            Assert.AreNotEqual(BuildType.Retail, null);
            Assert.AreNotEqual(null, BuildType.Retail);
            Assert.AreNotEqual(BuildType.Developer, BuildType.Retail);
            Assert.AreEqual(BuildType.Testing, BuildType.Testing);
            Assert.AreEqual(BuildType.Testing, BuildType.Create("Testing"));
        }
    }
}
