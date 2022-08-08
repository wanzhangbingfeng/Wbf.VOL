using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Const;

namespace Wbf.VOL.Core.Configuration
{
    public static class AppSetting
    {
        #region 字段属性
        public static IConfiguration Configuration { get; private set; }
        private static Connection _connection;
        public static Secret Secret { get; private set; }
        /// <summary>
        /// JWT有效期(分钟=默认120)
        /// </summary>
        public static int ExpMinutes { get; private set; } = 120;

        public static string CurrentPath { get; private set; } = null;
        public static string DownLoadPath { get { return CurrentPath + "\\Download\\"; } }

        public static string DbConnectionString
        {
            get { return _connection.DbConnectionString; }
        }
        #endregion

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            services.Configure<Secret>(configuration.GetSection("Secret"));
            services.Configure<Connection>(configuration.GetSection("Connection"));
            var provider = services.BuildServiceProvider();

            Secret = provider.GetRequiredService<IOptions<Secret>>().Value;

            _connection = provider.GetRequiredService<IOptions<Connection>>().Value;
        }
        public static bool UseRedis
        {
            get { return _connection.UseRedis; }
        }
    }

    #region model
    public class Connection
    {
        public string? DBType { get; set; }
        public string? DbConnectionString { get; set; }
        public string? RedisConnectionString { get; set; }
        public bool UseRedis { get; set; }
        public bool UseSignalR { get; set; }
    }
    #endregion
}
