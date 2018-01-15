using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TianQin365.Organization.IRepositories;
using TianQin365.Organization.Models;
using TianQin365.Organization.Repositories;

namespace TianQin365.AuthorizationServer
{
    public class ApplicationUserManager : IDisposable
    {
        private readonly IUserRepository _userRepository = null;

        public ApplicationUserManager(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public User Find(string phoneNumber, string password)
        {
            var u = this._userRepository.ReadList($"phonenumber='{phoneNumber}'").FirstOrDefault();

            return u != null && u.IsPasswordCorrect(password) ? u : null;
        }

        public List<string> GetUserRoles(User user)
        {
            // TODO:获取用户角色
            return new List<string> { "Admin", "User" };
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserRepository());
            return manager;
        }

        public void Dispose() { }
    }
}