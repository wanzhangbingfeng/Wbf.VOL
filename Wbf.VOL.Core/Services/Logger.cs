using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wbf.VOL.Core.Configuration;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.UserManager;
using Wbf.VOL.Entity.DomainModels.System;

namespace Wbf.VOL.Core.Services
{
    public static class Logger
    {
        private static string _loggerPath = AppSetting.DownLoadPath + "Logger\\Queue\\";
        public static ConcurrentQueue<Sys_Log> loggerQueueData = new ConcurrentQueue<Sys_Log>();
        public static void Info(LoggerType loggerType, string requestParam, string resposeParam, string ex = null)
        {
            Add(loggerType, requestParam, resposeParam, ex, LoggerStatus.Info);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="requestParameter">请求参数</param>
        /// <param name="responseParameter">响应参数</param>
        /// <param name="success">响应结果1、成功,2、异常，0、其他</param>
        /// <param name="userInfo">用户数据</param>
        private static void Add(LoggerType loggerType, string requestParameter, string responseParameter, string ex, LoggerStatus status)
        {
            Sys_Log log = null;
            try
            {
                HttpContext context = Utilities.HttpContext.Current;
                if (context.Request.Method == "OPTIONS") return;
                ActionObserver cctionObserver = context.RequestServices.GetService(typeof(ActionObserver)) as ActionObserver;
                if (context == null)
                {
                    WriteText($"未获取到httpcontext信息,type:{loggerType.ToString()},reqParam:{requestParameter},respParam:{responseParameter},ex:{ex},success:{status}");
                    return;
                }
                UserInfo userInfo = UserContext.Current.UserInfo;
                log = new Sys_Log()
                {
                    BeginDate = cctionObserver.RequestDate,
                    EndDate = DateTime.Now,
                    User_Id = userInfo.User_Id,
                    UserName = userInfo.UserTrueName,
                    Role_Id = userInfo.Role_Id,
                    LogType = loggerType.ToString(),
                    ExceptionInfo = ex,
                    RequestParameter = requestParameter,
                    ResponseParameter = responseParameter,
                    Success = (int)status
                };
                SetServicesInfo(log, context);
            }
            catch (Exception exception)
            {
                log = log ?? new Sys_Log()
                {
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    LogType = loggerType.ToString(),
                    RequestParameter = requestParameter,
                    ResponseParameter = responseParameter,
                    Success = (int)status,
                    ExceptionInfo = ex + exception.Message
                };
            }
            loggerQueueData.Enqueue(log);
        }

        private static void WriteText(string message)
        {
            try
            {
                Utilities.FileHelper.WriteFile(_loggerPath + "WriteError\\", $"{DateTime.Now.ToString("yyyyMMdd")}.txt", message + "\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"日志写入文件时出错:{ex.Message}");
            }
        }

        public static void SetServicesInfo(Sys_Log log, HttpContext context)
        {
            string result = String.Empty;
            log.Url = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase +
                context.Request.Path;

            log.UserIP = context.GetUserIp()?.Replace("::ffff:", "");
            log.ServiceIP = context.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + context.Connection.LocalPort;

            log.BrowserType = context.Request.Headers["User-Agent"];
            if (log.BrowserType.Length > 190)
            {
                log.BrowserType = log.BrowserType.Substring(0, 190);
            }
            if (string.IsNullOrEmpty(log.RequestParameter))
            {
                try
                {
                    log.RequestParameter = context.GetRequestParameters();
                    if (log.RequestParameter != null)
                    {
                        log.RequestParameter = HttpUtility.UrlDecode(log.RequestParameter, Encoding.UTF8);
                    }
                }
                catch (Exception ex)
                {
                    log.ExceptionInfo += $"日志读取参数出错:{ex.Message}";
                    Console.WriteLine($"日志读取参数出错:{ex.Message}");
                }
            }
        }

    }
}
