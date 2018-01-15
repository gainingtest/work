using System;
using Com.EnjoyCodes.DapperRepositories.SqlAttributes;
using TianQin365.Common.Encrypt;

namespace TianQin365.Organization.Models
{
    /// <summary>
    /// 用户模型
    /// </summary>
    [Table(Name = "Users")]
    public partial class User
    {
        [Key]
        public Guid ID { get; set; }
        public Guid CompanyID { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Hash加密后的密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Hash加密密码的salt
        /// </summary>
        public string PasswordSalt { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// 用户模型方法
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// 使用Hash加密密码
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <returns></returns>
        public Tuple<string, string> EncryptPassword(string password)
        {
            return new Hash().Create(password);
        }

        /// <summary>
        /// 密码校验
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <returns></returns>
        public bool IsPasswordCorrect(string password)
        {
            return new Hash().Check(this.Password, this.PasswordSalt, password);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="newPassword">新的密码</param>
        public void RestPassword(string newPassword = "111111")
        {
            var p = new Hash().Create(newPassword);
            this.Password = p.Item1;
            this.PasswordSalt = p.Item2;
        }
    }
}
