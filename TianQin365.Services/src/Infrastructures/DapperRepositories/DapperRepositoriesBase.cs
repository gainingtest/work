using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Com.EnjoyCodes.DapperRepositories.Entities;

namespace Com.EnjoyCodes.DapperRepositories
{
    public abstract class DapperRepositoriesBase<T> : DapperRepositories, IDapperRepositories<T>
    {
        #region Table
        public abstract int CreateTable();
        #endregion

        #region Query
        public abstract T Read(object id);
        public abstract List<T> ReadList(string where, object parament = null, string field = "*");

        public virtual Task<List<T>> GetAllListAsync(string where, object parament = null, string field = "*")
        {
            return Task.FromResult(ReadList(where, parament, field));
        }

        public virtual Task<T> GetAsync(int id)
        {
            return Task.Run(() => Read(id));
        }
        #endregion

        #region Paging
        public abstract Page<T> ReadPaging(int pageNumber, int pageSize, string sqlWhere = null, string sqlPre = null, string sqlFields = null, string sqlFrom = null, string sqlOrderBy = null, object parament = null);
        public abstract Page<T> ReadPagingByJoin(int pageNumber, int pageSize, string sqlWhere = null, string sqlJoin = null, string sqlPre = null, string sqlFields = null, string sqlFrom = null, string sqlOrderBy = null, object parament = null);
        public abstract Page<T> ReadPagingByProc(int pageNumber, int pageSize, string procName, object parament = null);
        #endregion

        #region Insert
        public abstract object Create(T entity);

        public virtual Task<object> InsertAsync(T entity)
        {
            return Task.FromResult(Create(entity));
        }
        #endregion

        #region Update
        public abstract T Update(T entity);

        public virtual Task<T> UpdateAsync(T entity)
        {
            return Task.FromResult(Update(entity));
        }
        #endregion

        #region Delete        
        public virtual Task DeleteAsync(T entity)
        {
            return Task.Run(() => Delete(entity));
        }

        public abstract void Delete(object id);

        public virtual Task DeleteAsync(object id)
        {
            return Task.Run(() => Delete(id));
        }

        public abstract void Delete(string where, object parament = null);

        public virtual async Task DeleteAsync(string where, object parament = null)
        {
            await Task.Run(() => Delete(where, parament));
        }
        #endregion

        #region Aggregation
        public abstract int Count();

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public abstract int Count(string sqlWhere, object parament = null);

        public virtual Task<int> CountAsync(string sqlWhere, object parament = null)
        {
            return Task.FromResult(Count(sqlWhere, parament));
        }
        #endregion

        #region ExecuteNonQuery

        public abstract int Execute(string sql, object parament = null, CommandType commandType = CommandType.Text);
        #endregion
    }
}
