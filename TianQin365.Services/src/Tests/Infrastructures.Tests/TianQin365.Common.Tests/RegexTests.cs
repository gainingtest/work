using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TianQin365.Common.Tests
{
    [TestClass]
    public class RegexTests
    {
        [TestMethod]
        public void GetLetter()
        {
            var r = "T8j6UO7f5uAk0MMkVNb6dWlx20Dt5cOPTJuTf_QjD2w_r4dOn7b5HvWuyFI-mdjK3";
            var r1 = string.Join("", r.Where(p => (p >= 'a' && p <= 'z') || (p >= 'A' && p <= 'Z')).Take(24));
        }
    }
}
