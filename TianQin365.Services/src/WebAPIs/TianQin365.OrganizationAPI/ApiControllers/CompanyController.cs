using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using TianQin365.Common.WebAPI;
using TianQin365.Organization.Application;
using TianQin365.Organization.Models;

namespace TianQin365.OrganizationAPI.ApiControllers
{
    /// <summary>
    /// 单位操作
    /// </summary>
    //[Authorize(Roles = "Admin1,User")]
    //[Authorize(Users = "test")]
    [RoutePrefix("bpi")]
    public class CompanyController : ApiController
    {
        private ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            this._service = service;
        }

        /// <summary>
        /// 获取单位集合
        /// </summary>
        /// <returns></returns>
        [Route("Companies")]
        [ResponseType(typeof(List<CompanyOutput>))]
        public List<CompanyOutput> Get()
        {
            var r = this._service.Get();

            return Mapper.Map<List<CompanyOutput>>(r);
        }


        //
        //
        // 通过主键单条查询
        /// <returns></returns>
        [Route("CompanyById")]
        [ResponseType(typeof(CompanyOutput))]
        public CompanyOutput GetbyId(string id)
        {

            var r = this._service.GetById(id);
            return Mapper.Map<CompanyOutput>(r);
        }

        //
        //
        // 通过其他单条查询
        /// <returns></returns>
        [Route("CompanyByName")]
        [ResponseType(typeof(CompanyOutput))]
        public CompanyOutput GetbyName(string name) {
            
            var r = this._service.GetByName(name);
            return Mapper.Map<CompanyOutput>(r);
        }


        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [Route("Company")]
        [ResponseType(typeof(Guid))]
        public Guid Post([FromBody]CompanyPost company)
        {
            return this._service.add(company);
             
        }

        /// <summary>
        /// 修改单位
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [Route("Company")]
        [ResponseType(typeof(bool))]
        public bool Put([FromBody]CompanyPut c)
        {
            //遗留问题 类型转换
            //CompanyPut c = new CompanyPut();
            Company company=Mapper.Map<Company>(c);

            return this._service.put(company);
            
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <returns></returns>
        [Route("CompanyDel")]
        [ResponseType(typeof(bool))]
        public bool DeleteById(string id)
        //public bool Delete([FromBody]CompanyDelete id)
        {
            //return this._service.delete(id.ID);
            return this._service.delete(id);
        }




    }
}
