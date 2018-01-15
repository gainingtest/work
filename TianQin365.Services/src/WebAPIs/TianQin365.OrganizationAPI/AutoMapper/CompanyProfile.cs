using AutoMapper;
using TianQin365.Organization.Models;

namespace TianQin365.OrganizationAPI.AutoMapper
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyOutput>();
            CreateMap<CompanyPut, Company>();
        }
    }
}