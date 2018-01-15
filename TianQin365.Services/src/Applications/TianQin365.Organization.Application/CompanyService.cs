using System;
using System.Collections.Generic;
using System.Linq;
using TianQin365.Organization.IRepositories;
using TianQin365.Organization.Models;
using TianQin365.Organization.Repositories;

namespace TianQin365.Organization.Application
{
    public interface ICompanyService
    {
        List<Company> Get();

        Company GetById(object id);

        Company GetByName(string name);
        Guid add(CompanyPost company);

        Boolean put(Company company);

        Boolean delete(string id);
    }

    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository = new CompanyRepository();

        public List<Company> Get() => this._repository.ReadList();
        public Guid add(CompanyPost c)
        {
            var company = new Company()
            {
                ID = Guid.NewGuid(),
                Name = c.Name,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            var result = this._repository.Create(company);
            return (Guid)result;
        }

        public Company GetById(object id)
        {            
            return this._repository.Read(id);
        }

        public Boolean put(Company company)
        {
            //遗留问题：如果update中某个参数为空（createtime）,怎么处理？
            company.CreateTime = DateTime.Now;
            company.UpdateTime = DateTime.Now;

            company= this._repository.Update(company);
            //如何验证是否更新成功
            //if (company.Name.Equals("DES"))
            //{
                return true;
            //}
            //else {
                //return false;
            //}
        }

        public Company GetByName(string name)
        {
            //return this._repository.ReadList($"name=@name",new {name=name}).FirstOrDefault();
            return this._repository.ReadList($"name='{name}'").FirstOrDefault();
        }

        public Boolean delete(string id)
        {
            this._repository.Delete(id);
            return true; //如何判断是否真正成功弄
        }
    }
}
