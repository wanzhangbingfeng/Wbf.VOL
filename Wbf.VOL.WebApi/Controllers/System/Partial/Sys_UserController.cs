using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.Utilities;
using Wbf.VOL.Entity.DomainModels.System;
using Wbf.VOL.System.IServices.System;

namespace Wbf.VOL.WebApi.Controllers.System
{
    [Route("api/User")]
    public partial class Sys_UserController
    {
        //public Sys_UserController(ISys_UserService userService):base(userService)
        //{

        //}
        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getVierificationCode")]
        public IActionResult GetVierificationCode()
        {
            string code = VierificationCode.RandomText();
            var data = new
            {
                img = VierificationCodeHelpers.CreateBase64Image(code),
                uuid = Guid.NewGuid()
            };
            HttpContext.GetService<IMemoryCache>().Set(data.uuid.ToString(), code, new TimeSpan(0, 5, 0));
            return Json(data);
        }

        [HttpPost, HttpGet, Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInfo loginInfo)
        {
            return Json(await Service.Login(loginInfo));
        }
    }
}
