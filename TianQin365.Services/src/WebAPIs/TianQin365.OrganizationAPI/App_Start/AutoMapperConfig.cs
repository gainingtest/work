using AutoMapper;
using TianQin365.OrganizationAPI.AutoMapper;

namespace TianQin365.OrganizationAPI
{
    public class AutoMapperConfig
    {
        public static void RegisterProfile()
        {
            Mapper.Initialize(m =>
            {
                m.AddProfile<CompanyProfile>();
            });
        }
    }
}