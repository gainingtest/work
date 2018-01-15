using System;

namespace Com.EnjoyCodes.DapperRepositories.SqlAttributes
{
    /// <summary>
    /// 自增属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute { }
}
