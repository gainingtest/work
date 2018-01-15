using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TianQin365.Common.Models;
using TianQin365.Organization.Models;

namespace TianQin365.Organization.Repositories.Tests
{
    [TestClass]
    public class CompanyRepositoryTests
    {
        private CompanyRepository _repository = new CompanyRepository();

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Config.Initialize();
        }

        [TestMethod]
        public void Create()
        {
            var c = new Company()
            {
                ID = Guid.NewGuid(),
                Name = "hangtian",
                CreateTime = DateTime.Now,
            };
            var r = this._repository.Create(c);

            Assert.IsTrue((Guid)r != default(Guid));
        }

        [TestMethod]
        public void ReadList()
        {
            var companies = this._repository.ReadList(string.Empty);

            Assert.IsTrue(companies.Count > 0);
        }

        [TestMethod]
        public void Delete()
        {
            this._repository.Delete(Guid.Empty);
        }
    }
}
