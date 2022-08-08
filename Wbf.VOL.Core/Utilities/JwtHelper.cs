﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Configuration;
using Wbf.VOL.Core.UserManager;
using Wbf.VOL.Entity.DomainModels.System;

namespace Wbf.VOL.Core.Utilities
{
    public class JwtHelper
    {
        /// <summary>
        /// 生成JWT
        /// </summary>
        /// <param name="serInfo"></param>
        /// <returns></returns>
        public static string IssueJwt(UserInfo userInfo)
        {
            string exp = $"{new DateTimeOffset(DateTime.Now.AddMinutes(UserContext.MenuType == 1 ? 43200 : AppSetting.ExpMinutes)).ToUnixTimeSeconds()}";
            var claims = new List<Claim>
                {
                //new Claim(ClaimTypes.Name,userInfo.UserName ),
                //new Claim(ClaimTypes.Role,userInfo.Role_Id ),
                new Claim(JwtRegisteredClaimNames.Jti,userInfo.User_Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //JWT过期时间
                //验证是否过期 从User读取过期 时间，再将时间戳转换成日期，如果时间在半个小时内即将过期，通知前台刷新JWT
                //int val= HttpContext.User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Exp).FirstOrDefault().Value;
                //new DateTime(621355968000000000 + (long)val* (long)10000000, DateTimeKind.Utc).ToLocalTime()
                //默认设置jwt过期时间120分钟
                new Claim (JwtRegisteredClaimNames.Exp,exp),
                new Claim(JwtRegisteredClaimNames.Iss,AppSetting.Secret.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud,AppSetting.Secret.Audience),
               };

            //秘钥16位
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.Secret.JWT));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new JwtSecurityToken(issuer: AppSetting.Secret.Issuer, claims: claims, signingCredentials: creds);
            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return jwt;
        }
    }
}
