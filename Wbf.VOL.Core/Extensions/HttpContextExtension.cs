using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wbf.VOL.Core.Extensions
{
    public static class HttpContextExtension
    {
        public static T GetService<T>(this HttpContext context) where T : class
        {
            return context.RequestServices.GetService(typeof(T)) as T;
        }

        public static string GetUserIp(this HttpContext context)
        {
            string realIP = null;
            string forwarded = null;
            string remoteIpAddress = context.Connection.RemoteIpAddress.ToString();
            if (context.Request.Headers.ContainsKey("X-Real-IP"))
            {
                realIP = context.Request.Headers["X-Real-IP"].ToString();
                if (realIP != remoteIpAddress)
                {
                    remoteIpAddress = realIP;
                }
            }
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                forwarded = context.Request.Headers["X-Forwarded-For"].ToString();
                if (forwarded != remoteIpAddress)
                {
                    remoteIpAddress = forwarded;
                }
            }
            return remoteIpAddress;
        }

        /// <summary>
        /// 获取请求的参数
        /// net core 2.0已增加回读方法 context.Request.EnableRewind();
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public static string GetRequestParameters(this HttpContext context)
        {
            if (context.Request.Body == null || !context.Request.Body.CanRead || !context.Request.Body.CanSeek)
                return null;
            if (context.Request.Body.Length == 0)
                return null;
            if (context.Request.Body.Position > 0)
                context.Request.Body.Position = 0;

            string prarameters = null;
            var bodyStream = context.Request.Body;

            using (var buffer = new MemoryStream())
            {
                bodyStream.CopyToAsync(buffer);
                buffer.Position = 0L;
                bodyStream.Position = 0L;
                using (var reader = new StreamReader(buffer, Encoding.UTF8))
                {
                    buffer.Seek(0, SeekOrigin.Begin);
                    prarameters = reader.ReadToEnd();
                }
            }
            return prarameters;
        }
    }
}
