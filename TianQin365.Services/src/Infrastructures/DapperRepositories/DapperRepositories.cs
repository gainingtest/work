using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Com.EnjoyCodes.DapperRepositories.Entities;
using Com.EnjoyCodes.DapperRepositories.SqlAttributes;
using Dapper;
using TianQin365.Common.Factories;

namespace Com.EnjoyCodes.DapperRepositories
{
    public class DapperRepositories<T> : DapperRepositoriesBase<T>
    {
        #region Properties & Members
        public string TableName { get; set; }
        public string Prefix { get; set; }
        public string KeyName { get; set; }
        public PropertyInfo[] Properties { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public PropertyInfo Key { get; set; }
        /// <summary>
        /// 自增字段
        /// </summary>
        public PropertyInfo[] Identities { get; set; }

        public static readonly Dictionary<Type, SqlDbType> SqlDbTypes = new Dictionary<Type, SqlDbType>
        {
            [typeof(long)] = SqlDbType.BigInt,
            [typeof(int)] = SqlDbType.Int,
            [typeof(short)] = SqlDbType.SmallInt,
            [typeof(byte)] = SqlDbType.TinyInt,
            [typeof(decimal)] = SqlDbType.Decimal,
            [typeof(double)] = SqlDbType.Float,
            [typeof(float)] = SqlDbType.Real,
            [typeof(bool)] = SqlDbType.Bit,
            [typeof(string)] = SqlDbType.NVarChar,
            [typeof(char)] = SqlDbType.Char,
            [typeof(DateTime)] = SqlDbType.DateTime,
            [typeof(TimeSpan)] = SqlDbType.Timestamp,
            [typeof(Guid)] = SqlDbType.UniqueIdentifier,
            [typeof(Enum)] = SqlDbType.Int,

            [typeof(byte[])] = SqlDbType.Binary,
            [typeof(byte?)] = SqlDbType.TinyInt,
            //[typeof(sbyte?)] = SqlDbType.TinyInt,
            [typeof(short?)] = SqlDbType.SmallInt,
            //[typeof(ushort?)] = SqlDbType.SmallInt,
            [typeof(int?)] = SqlDbType.Int,
            //[typeof(uint?)] = SqlDbType.Int,
            [typeof(long?)] = SqlDbType.BigInt,
            //[typeof(ulong?)] = SqlDbType.BigInt,
            [typeof(float?)] = SqlDbType.Real,
            [typeof(double?)] = SqlDbType.Float,
            [typeof(decimal?)] = SqlDbType.Decimal,
            [typeof(bool?)] = SqlDbType.Bit,
            [typeof(char?)] = SqlDbType.Char,
            [typeof(Guid?)] = SqlDbType.UniqueIdentifier,
            [typeof(DateTime?)] = SqlDbType.DateTime,
            [typeof(DateTimeOffset?)] = SqlDbType.DateTimeOffset,
            [typeof(TimeSpan?)] = SqlDbType.Timestamp,
        };
        #endregion

        #region Structures & Utilities
        public DapperRepositories()
        {
            var type = typeof(T);

            // 表特性
            TableAttribute tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
            if (tableAttribute != null)
            {
                TableName = tableAttribute.Name;
                Prefix = tableAttribute.Prefix;
            }
            else
            {
                TableName = type.Name;
            }

            // 属性
            Properties = type.GetProperties();

            // 主键
            var keyProperty = Properties.FirstOrDefault(p => p.GetCustomAttribute(typeof(KeyAttribute), true) != null);
            if (keyProperty != null) KeyName = Prefix + keyProperty.Name;

            // 自增字段
            this.Identities = Properties.Where(p => p.GetCustomAttribute(typeof(IdentityAttribute), true) != null).ToArray();

            if (string.IsNullOrEmpty(KeyName))
                throw new Exception("模型" + type.FullName + "未指定主键！", new Exception("请指定主键KeyAttribute属性。"));

            Key = type.GetProperty(keyProperty.Name);
        }

        /// <summary>
        /// 根据对象获取DynamicParamenters  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public DynamicParameters GetParamenters(T t, out List<string> list)
        {
            list = new List<string>();
            var parament = new DynamicParameters();
            foreach (var node in typeof(T).GetProperties())
            {
                if (node.Name == Key.Name && (node.PropertyType == typeof(Int16) || node.PropertyType == typeof(Int32) || node.PropertyType == typeof(Int64))) continue; // 数值型主键，不赋值
                if (Identities.FirstOrDefault(m => m.Name == node.Name) != null) continue; // 自增字段，不赋值
                if (node.GetValue(t) == null) continue; // 空值字段，不赋值
                list.Add(Prefix + node.Name);
                parament.Add("@" + Prefix + node.Name, node.GetValue(t));
            }
            return parament;
        }
        #endregion

        #region Table
        public override int CreateTable()
        {
            var sqlStr = new StringBuilder();
            sqlStr.AppendFormat("CREATE TABLE [{0}](", TableName);

            foreach (var item in Properties)
            {
                try
                {
                    if (!item.PropertyType.IsSealed) continue;

                    sqlStr.AppendFormat("[{0}{1}] ", Prefix, item.Name);
                    if (item.PropertyType == typeof(string))
                        sqlStr.AppendFormat("{0}(max)", SqlDbTypes[item.PropertyType]);
                    else if (item.PropertyType.IsEnum)
                        sqlStr.AppendFormat("{0}", SqlDbTypes[typeof(Enum)]);
                    else
                        sqlStr.AppendFormat("{0}", SqlDbTypes[item.PropertyType]);

                    if (Prefix + item.Name.ToLower() == KeyName.ToLower())
                    {
                        sqlStr.Append(" PRIMARY KEY ");
                        if (item.PropertyType == typeof(Int64) || item.PropertyType == typeof(Int32) || item.PropertyType == typeof(Int16))
                            sqlStr.Append("IDENTITY");
                    }
                    sqlStr.Append(",");
                }
                catch { }
            }

            sqlStr.Append(")");

            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                conn.Execute(sqlStr.ToString());
            }
            return 1;
        }
        #endregion

        #region CRUD,Paging
        public override object Create(T entity)
        {
            List<string> list;
            var parament = GetParamenters(entity, out list);
            var sql = new StringBuilder($"INSERT INTO [{TableName}] ({string.Join(",", list)}) VALUES (@{string.Join(",@", list)});");
            if (Key.PropertyType != typeof(Guid))
            {
                sql.Append("SET @ID_FYUJMNBVFGHJ=SCOPE_IDENTITY();");
                parament.Add("@ID_FYUJMNBVFGHJ", null, DbType.Object, ParameterDirection.Output);
                using (IDbConnection conn = new SqlConnection(GetConnectionString_Write(typeof(T))))
                {
                    conn.ExecuteScalar(sql.ToString(), parament);
                }
                return parament.Get<object>("ID_FYUJMNBVFGHJ");
            }
            else
            {
                using (IDbConnection conn = new SqlConnection(GetConnectionString_Write(typeof(T))))
                {
                    conn.ExecuteScalar(sql.ToString(), parament);
                }
                return Key.GetValue(entity);
            }
        }

        public override T Read(object id)
        {
            var field = string.IsNullOrEmpty(Prefix) ? "*" : string.Join(",", Properties.Where(m => m.PropertyType.IsSealed || m.PropertyType.IsValueType).Select(m => "[" + Prefix + m.Name + "] [" + m.Name + "]"));
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                return conn.Query<T>($"SELECT TOP 1 {field} FROM [{TableName}] WHERE {KeyName} = '{id}'").FirstOrDefault();
            }
        }

        public override List<T> ReadList(string where, object parament = null, string field = "*")
        {
            if (!string.IsNullOrEmpty(where))
                where = $" WHERE {where}";

            field = string.IsNullOrEmpty(Prefix) ? "*" : string.Join(",", Properties.Where(m => m.PropertyType.IsSealed || m.PropertyType.IsValueType).Select(m => "[" + Prefix + m.Name + "] [" + m.Name + "]"));
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                return conn.Query<T>($"SELECT {field} FROM [{TableName}] {where}", parament).ToList();
            }
        }

        public override Page<T> ReadPaging(int pageNumber, int pageSize, string sqlWhere = null, string sqlPre = null, string sqlFields = null, string sqlFrom = null, string sqlOrderBy = null, object parament = null)
        {
            // 查询参数处理
            if (string.IsNullOrEmpty(sqlFields)) sqlFields = "*";
            if (!string.IsNullOrEmpty(Prefix))
                sqlFields = string.Join(",", Properties.Where(m => m.PropertyType.IsSealed || m.PropertyType.IsValueType).Select(m => "[" + Prefix + m.Name + "] [" + m.Name + "]"));
            if (string.IsNullOrEmpty(sqlFrom))
                sqlFrom = TableName;
            if (!string.IsNullOrEmpty(sqlWhere))
                sqlWhere = "WHERE " + sqlWhere;
            if (string.IsNullOrEmpty(sqlOrderBy))
                sqlOrderBy = KeyName;

            // 查询数据行数
            var sqlStr = new StringBuilder();
            if (!string.IsNullOrEmpty(sqlPre))
                sqlStr.AppendFormat("{0}", sqlPre);
            sqlStr.AppendFormat("SELECT COUNT(1) FROM {0} {1};", sqlFrom, sqlWhere);

            // 查询分页
            sqlStr.Append($"SELECT {sqlFields} FROM (SELECT TOP {pageNumber * pageSize} ROW_NUMBER() OVER (ORDER BY {sqlOrderBy}) ROWINDEX, * FROM {sqlFrom} {sqlWhere}) F WHERE F.ROWINDEX BETWEEN {(pageNumber - 1) * pageSize + 1} AND { pageNumber * pageSize}");

            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                var r = conn.QueryMultiple(sqlStr.ToString(), parament);
                return new Page<T>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Count = r.Read<int>().Single(),
                    DataList = r.Read<T>().ToList()
                };
            }
        }

        public override Page<T> ReadPagingByJoin(int pageNumber, int pageSize, string sqlWhere = null, string sqlJoin = null, string sqlPre = null, string sqlFields = null, string sqlFrom = null, string sqlOrderBy = null, object parament = null)
        {
            // 查询参数处理
            if (string.IsNullOrEmpty(sqlFields)) sqlFields = "*";
            if (!string.IsNullOrEmpty(Prefix))
                sqlFields = string.Join(",", Properties.Where(m => m.PropertyType.IsSealed || m.PropertyType.IsValueType).Select(m => "[" + Prefix + m.Name + "] [" + m.Name + "]"));
            if (string.IsNullOrEmpty(sqlFrom))
                sqlFrom = TableName;
            if (!string.IsNullOrEmpty(sqlWhere))
                sqlWhere = "WHERE " + sqlWhere;
            if (string.IsNullOrEmpty(sqlOrderBy))
                sqlOrderBy = KeyName;

            // 查询数据行数
            var sqlStr = new StringBuilder();
            if (!string.IsNullOrEmpty(sqlPre))
                sqlStr.AppendFormat("{0}", sqlPre);
            sqlStr.Append($"SELECT COUNT(1) FROM {sqlFrom} {sqlJoin} {sqlWhere};");

            // 查询分页
            sqlStr.Append($"SELECT {sqlFields} FROM (SELECT TOP {pageNumber * pageSize} ROW_NUMBER() OVER (ORDER BY {sqlOrderBy}) ROWINDEX, * FROM {sqlFrom} {sqlJoin} {sqlWhere}) F WHERE F.ROWINDEX BETWEEN {(pageNumber - 1) * pageSize + 1} AND { pageNumber * pageSize}");

            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                var r = conn.QueryMultiple(sqlStr.ToString(), parament);
                return new Page<T>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Count = r.Read<int>().Single(),
                    DataList = r.Read<T>().ToList()
                };
            }
        }

        /// <summary>
        /// 读分页
        ///     存储过程实现
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parament"></param>
        /// <returns></returns>
        public override Page<T> ReadPagingByProc(int pageNumber, int pageSize, string procName, object parament = null)
        {
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                var r = conn.QueryMultiple(procName, parament, null, null, CommandType.StoredProcedure);
                return new Page<T>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Count = r.Read<int>().Single(),
                    DataList = r.Read<T>().ToList()
                };
            }
        }

        public override T Update(T entity)
        {
            List<string> list;
            var parament = GetParamenters(entity, out list);
            var agg = list.Aggregate(new StringBuilder(), (x, y) =>
            {
                x.Append(y);
                x.Append("=@");
                x.Append(y);
                x.Append(",");
                return x;
            });
            var sql = $"UPDATE [{TableName}] SET {agg.ToString().TrimEnd(',')} WHERE {KeyName} = '{Key.GetValue(entity)}'";
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Write(typeof(T))))
            {
                conn.Execute(sql, parament);
            }
            return entity;
        }

        public override void Delete(object id)
        {
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Write(typeof(T))))
            {
                conn.Execute($"DELETE FROM [{TableName}] WHERE {KeyName} = '{id}'");
            }
        }

        public override void Delete(string where, object parament = null)
        {
            if (!string.IsNullOrEmpty(where))
                where = $" WHERE {where}";

            using (IDbConnection conn = new SqlConnection(GetConnectionString_Write(typeof(T))))
            {
                conn.Execute($"DELETE FROM [{ TableName }] {where}", parament);
            }
        }

        public override int Count()
        {
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                return conn.ExecuteScalar<int>("SELECT COUNT(1) FROM [" + TableName + "]");
            }
        }

        public override int Count(string sqlWhere, object parament = null)
        {
            if (!string.IsNullOrEmpty(sqlWhere))
                sqlWhere = "WHERE " + sqlWhere;

            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                return conn.ExecuteScalar<int>($"SELECT COUNT(1) FROM [{TableName}] {sqlWhere}", parament);
            }
        }
        #endregion

        #region ExecuteNonQuery
        public override int Execute(string sql, object parament = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(T))))
            {
                return conn.ExecuteScalar<int>(sql, parament, null, null, commandType);
            }
        }
        #endregion
    }

    public class DapperRepositories
    {
        //public string GetConnectionString(string key) => ConfigurationManager.ConnectionStrings[key].ConnectionString;
        public string GetConnectionString_Read(Type type) => Connections.GetReadString(type);
        public string GetConnectionString_Write(Type type) => Connections.GetWriteString(type);

        #region Table
        public static int CreateTable<T>() => new DapperRepositories<T>().CreateTable();
        #endregion

        #region Paging
        public virtual Page<TOutType> ReadPaging<TOutType>(string tableName, int pageNumber, int pageSize, bool sumCount, string translate = "*", string orderBy = null, object parament = null)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                var type = typeof(TOutType);
                var keyProperty = type.GetProperties().FirstOrDefault(p => p.GetCustomAttribute(typeof(SqlAttributes.KeyAttribute), true) != null);
                if (keyProperty != null) orderBy = keyProperty.Name;

                if (string.IsNullOrEmpty(orderBy))
                    throw new Exception("模型" + type.FullName + "未指定主键！", new Exception("请指定主键KeyAttribute属性。"));
            }

            var excuteSql = GetPagingSql(tableName, pageNumber, pageSize, translate, orderBy) + $";SELECT A.COUNT FROM ( SELECT COUNT(1) AS COUNT FROM {tableName}) A";
            using (IDbConnection conn = new SqlConnection(GetConnectionString_Read(typeof(TOutType))))
            {
                var r = conn.QueryMultiple(excuteSql, parament);
                return new Page<TOutType>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Count = r.Read<int>().Single(),
                    DataList = r.Read<TOutType>().ToList()
                };
            }
        }
        public static string GetPagingSql(string tableName, int pageNumber, int pageSize, string translate = "*", string orderBy = null)
        {
            var start = pageNumber == 0 || pageNumber == 1 ? 1 : ((pageNumber - 1) * pageSize) + 1;
            return $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {orderBy})NewRow ,{translate} FROM {tableName} ) AUS WHERE NewRow BETWEEN {start} AND {start + pageSize - 1}";
        }
        #endregion
    }
}
