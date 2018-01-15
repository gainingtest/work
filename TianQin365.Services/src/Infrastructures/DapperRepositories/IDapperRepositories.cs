using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Com.EnjoyCodes.DapperRepositories.Entities;

namespace Com.EnjoyCodes.DapperRepositories
{
    /// <summary>
    /// 对象仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDapperRepositories<T> : IDapperRepositories
    {
        #region Table
        int CreateTable();
        #endregion

        #region Select/Get/Query
        T Read(object id);

        List<T> ReadList(string where = null, object parament = null, string field = "*");

        Task<List<T>> GetAllListAsync(string where = null, object parament = null, string field = "*");

        Task<T> GetAsync(int id);
        #endregion

        #region Paging
        Page<T> ReadPaging(int pageNumber, int pageSize, string sqlWhere = null, string sqlPre = null, string sqlFields = null, string sqlFrom = null, string sqlOrderBy = null, object parament = null);

        Page<T> ReadPagingByJoin(int pageNumber, int pageSize, string sqlWhere = null, string sqlJoin = null, string sqlPre = null, string sqlFields = null, string sqlFrom = null, string sqlOrderBy = null, object parament = null);

        Page<T> ReadPagingByProc(int pageNumber, int pageSize, string procName, object parament = null);
        #endregion

        #region Insert
        object Create(T entity);

        Task<object> InsertAsync(T entity);
        #endregion

        #region Update
        T Update(T entity);

        Task<T> UpdateAsync(T entity);
        #endregion

        #region Delete
        Task DeleteAsync(T entity);

        void Delete(object id);

        Task DeleteAsync(object id);

        void Delete(string where, object parament = null);

        Task DeleteAsync(string where, object parament = null);
        #endregion

        #region Aggregates
        int Count();
        int Count(string where, object parament = null);
        #endregion

        #region ExecuteNonQuery
        int Execute(string sql, object parament = null, CommandType commandType = CommandType.Text);
        #endregion
    }

    /// <summary>
    /// 轻仓储（无状态）
    /// </summary>
    public interface IDapperRepositories
    {
        #region Paging
        Page<TOutType> ReadPaging<TOutType>(string sql, int pageNumber, int pageSize, bool sumCount, string translate = "*", string orderBy = "Id", object parament = null);
        #endregion        
    }
}
