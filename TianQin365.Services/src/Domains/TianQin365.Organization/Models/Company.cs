using System;
using Com.EnjoyCodes.DapperRepositories.SqlAttributes;

namespace TianQin365.Organization.Models
{
    [Table(Name = "Companies")]
    public class Company
    {
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    #region Inputs
    public class CompanyPost
    {
        public string Name { get; set; }
     //   public string Creator { get; set; }
    }
    public class CompanyPut
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
       // public string Creator { get; set; }
    }
    public class CompanyDelete
    {
        public Guid ID { get; set; }
    }
    #endregion

    #region Outputs
    public class CompanyOutput
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        public string CreateTime { get; set; }
    }
    #endregion
}
