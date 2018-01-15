using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TianQin365.Common.Encrypt;
using TianQin365.Common.Models;
using TianQin365.Organization.Models;

namespace TianQin365.Organization.Repositories.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private UserRepository _repository = new UserRepository();

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Config.Initialize();
        }

        [TestMethod]
        public void Create()
        {
            var password = new Hash().Create("123456");

            var user = new User()
            {
                ID = Guid.NewGuid(),
                Name = "test",
                PhoneNumber = "13400000001",
                Password = password.Item1,
                PasswordSalt = password.Item2,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            var u = this._repository.ReadList($"phonenumber={user.PhoneNumber}").FirstOrDefault();
            if (u != null) throw new Exception("手机号已注册！");

            var r = this._repository.Create(user);

            Assert.IsTrue((Guid)r != default(Guid));
        }

        [TestMethod]
        public void Login()
        {
            var phoneNumber = "13400000001";
            var password = "123456";

            var u = this._repository.ReadList($"phonenumber='{phoneNumber}'").FirstOrDefault();
            if (u == null) throw new Exception("账户不存在！");

            Assert.IsTrue(u.IsPasswordCorrect(password));
        }
    }
}
