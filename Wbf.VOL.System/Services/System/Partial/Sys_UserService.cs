using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Configuration;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.Services;
using Wbf.VOL.Core.UserManager;
using Wbf.VOL.Core.Utilities;
using Wbf.VOL.Core.Utilities.Response;
using Wbf.VOL.Entity.DomainModels.System;

namespace Wbf.VOL.System.Services.System
{
    public partial class Sys_UserService
    {
        public async Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            string msg = string.Empty;
            WebResponseContent responseContent = new WebResponseContent();

            //验证码验证
            IMemoryCache memoryCache = HttpContext.Current.GetService<IMemoryCache>();
            string cacheCode = (memoryCache.Get(loginInfo.UUID) ?? "").ToString();
            if (string.IsNullOrEmpty(cacheCode))
            {
                return responseContent.Error("验证码已失效");
            }
            if (cacheCode.ToLower() != loginInfo.VerificationCode.ToLower())
            {
                memoryCache.Remove(loginInfo.UUID);
                return responseContent.Error("验证码不正确");
            }
            try
            {
                Sys_User user = await repository.FindAsIQueryable(x => x.UserName == loginInfo.UserName).FirstOrDefaultAsync();

                if (user == null || loginInfo.Password.Trim().EncryptDES(AppSetting.Secret.User) != (user.UserPwd ?? ""))
                    return responseContent.Error(ResponseType.LoginError);

                string token = JwtHelper.IssueJwt(new UserInfo()
                {
                    User_Id = user.User_Id,
                    UserName = user.UserName,
                    Role_Id = user.Role_Id
                });
                user.Token = token;
                responseContent.Data = new { token, userName = user.UserTrueName, img = user.HeadImageUrl };
                repository.Update(user, x => x.Token, true);
                UserContext.Current.LogOut(user.User_Id);
                loginInfo.Password = string.Empty;

                return responseContent.OK(ResponseType.LoginSuccess);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return responseContent.Error(ResponseType.ServerError);
            }
            finally
            {
                memoryCache.Remove(loginInfo.UUID);
                Logger.Info(LoggerType.Login, loginInfo.Serialize(), responseContent.Message, msg);
            }
        }
    }
}
