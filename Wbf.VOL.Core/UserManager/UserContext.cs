using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.CacheManager.IService;
using Wbf.VOL.Core.DBManager;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.Extensions.AutofacManager;
using Wbf.VOL.Entity.DomainModels.System;

namespace Wbf.VOL.Core.UserManager
{
    public class UserContext
    {
        /// <summary>
        /// 为了尽量减少redis或Memory读取,保证执行效率,将UserContext注入到DI，
        /// 每个UserContext的属性至多读取一次redis或Memory缓存从而提高查询效率
        /// </summary>
        public static UserContext Current
        {
            get
            {
                return Context.RequestServices.GetService(typeof(UserContext)) as UserContext;
            }
        }

        private static Microsoft.AspNetCore.Http.HttpContext Context
        {
            get
            {
                return Utilities.HttpContext.Current;
            }
        }

        private static ICacheService CacheService
        {
            get { return GetService<ICacheService>(); }
        }
        private static T GetService<T>() where T : class
        {
            return AutofacContainerModule.GetService<T>();
        }
        public int UserId
        {
            get
            {
                return (Context.User.FindFirstValue(JwtRegisteredClaimNames.Jti)
                    ?? Context.User.FindFirstValue(ClaimTypes.NameIdentifier)).GetInt();
            }
        }
        private UserInfo _userInfo { get; set; }
        public UserInfo UserInfo
        {
            get
            {
                if (_userInfo != null)
                {
                    return _userInfo;
                }
                return GetUserInfo(UserId);
            }
        }
        public UserInfo GetUserInfo(int userId)
        {
            if (_userInfo != null) return _userInfo;
            if (userId <= 0)
            {
                _userInfo = new UserInfo();
                return _userInfo;
            }
            string key = userId.GetUserIdKey();
            _userInfo = CacheService.Get<UserInfo>(key);
            if (_userInfo != null && _userInfo.User_Id > 0) return _userInfo;

            _userInfo = DBServerProvider.DbContext.Set<Sys_User>()
                .Where(x => x.User_Id == userId).Select(s => new UserInfo()
                {
                    User_Id = userId,
                    Role_Id = s.Role_Id.GetInt(),
                    RoleName = s.RoleName,
                    Token = s.Token,
                    UserName = s.UserName,
                    UserTrueName = s.UserTrueName,
                    Enable = s.Enable
                }).FirstOrDefault();

            if (_userInfo != null && _userInfo.User_Id > 0)
            {
                CacheService.AddObject(key, _userInfo);
            }
            return _userInfo ?? new UserInfo();
        }
        /// <summary>
        /// 2022.03.26
        /// 菜单类型1:移动端，0:PC端
        /// </summary>
        public static int MenuType
        {
            get
            {
                return Context.Request.Headers.ContainsKey("uapp") ? 1 : 0;
            }
        }

        public void LogOut(int userId)
        {
            CacheService.Remove(userId.GetUserIdKey());
        }
    }
}
