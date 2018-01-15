using Com.EnjoyCodes.DapperRepositories;
using TianQin365.Organization.IRepositories;
using TianQin365.Organization.Models;

namespace TianQin365.Organization.Repositories
{
    public class CompanyRepository : DapperRepositories<Company>, ICompanyRepository { }
}
