using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Configuration;
using Wbf.VOL.Core.Dapper;
using Wbf.VOL.Core.EFDbContext;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Core.Extensions;

namespace Wbf.VOL.Core.DBManager
{
    public class DBServerProvider
    {
        private static Dictionary<string, string> ConnectionPool = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static readonly string DefaultConnName = "defalut";
        static DBServerProvider()
        {
            SetConnection(DefaultConnName, AppSetting.DbConnectionString);
        }

        public static string GetConnectionString(string key)
        {
            key = key ?? DefaultConnName;
            if (ConnectionPool.ContainsKey(key))
            {
                return ConnectionPool[key];
            }
            return key;
        }

        /// <summary>
        /// 获取默认数据库连接
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return GetConnectionString(DefaultConnName);
        }
        public static void SetConnection(string key, string val)
        {
            if (ConnectionPool.ContainsKey(key))
            {
                ConnectionPool[key] = val;
                return;
            }
            ConnectionPool.Add(key, val);
        }

        public static VOLContext DbContext
        {
            get { return GetEFDbContext(); }
        }
        public static VOLContext GetEFDbContext()
        {
            return GetEFDbContext(null);
        }
        public static VOLContext GetEFDbContext(string dbName)
        {
            VOLContext beefContext = Utilities.HttpContext.Current.RequestServices.GetService(typeof(VOLContext)) as VOLContext;
            if (dbName != null)
            {
                if (!ConnectionPool.ContainsKey(dbName))
                {
                    throw new Exception("数据库连接名称错误");
                }
                beefContext.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
            }
            return beefContext;
        }

        /// <summary>
        /// 获取实体的数据库连接
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="defaultDbContext"></param>
        /// <returns></returns>
        public static void GetDbContextConnection<TEntity>(VOLContext defaultDbContext)
        {
          
        }

        /// <summary>
        /// 扩展dapper 获取MSSQL数据库DbConnection，默认系统获取配置文件的DBType数据库类型，
        /// </summary>
        /// <param name="connString">如果connString为null 执行重载GetDbConnection(string connString = null)</param>
        /// <param name="dapperType">指定连接数据库的类型：MySql/MsSql/PgSql</param>
        /// <returns></returns>
        public static IDbConnection GetDbConnection(string connString = null, DbCurrentType dbCurrentType = DbCurrentType.Default)
        {
            //默认获取DbConnection
            if (connString.IsNullOrEmpty() || DbCurrentType.Default == dbCurrentType)
            {
                return GetDbConnection(connString);
            }
            if (dbCurrentType == DbCurrentType.MySql)
            {
                return new MySqlConnection(connString);
            }
            if (dbCurrentType == DbCurrentType.PgSql)
            {
                return new NpgsqlConnection(connString);
            }
            return new SqlConnection(connString);

        }

        public static ISqlDapper GetSqlDapper<TEntity>()
        {
            //获取实体真实的数据库连接池对象名，如果不存在则用默认数据连接池名
            string dbName = typeof(TEntity).GetTypeCustomValue<DBConnectionAttribute>(x => x.DBName) ?? DefaultConnName;
            return GetSqlDapper(dbName);
        }

        public static ISqlDapper GetSqlDapper(string dbName = null)
        {
            return new SqlDapper(dbName ?? DefaultConnName);
        }
    }
}
