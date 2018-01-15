using Microsoft.VisualStudio.TestTools.UnitTesting;
using TianQin365.Common.Encrypt;

namespace TianQin365.Common.Tests
{
    [TestClass]
    public class HashTests
    {
        [TestMethod]
        public void CreateAndCheck()
        {
            var hash = new Hash();

            var text = "123456";

            var p = hash.Create(text);

            var isCorrect = hash.Check(p.Item1, p.Item2, text);

            Assert.IsTrue(isCorrect);
        }
    }
}
