using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wbf.VOL.Core.Controllers.Basic;
using Wbf.VOL.Entity.AttributeManager;
using Wbf.VOL.System.IServices.System;

namespace Wbf.VOL.WebApi.Controllers.System
{
    [Route("api/[controller]")]
    [PermissionTable(Name = "Sys_User")]
    public partial class Sys_UserController : ApiBaseController<ISys_UserService>
    {
        public Sys_UserController(ISys_UserService service):base("System", "System", "Sys_User", service)
        {

        }
    }
}
