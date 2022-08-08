using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Wbf.VOL.Core.Controllers.Basic
{
    [ApiController]
    public class ApiBaseController<IServiceBase> : Controller
    {
        protected IServiceBase Service;
        public ApiBaseController(string projectName, string folder, string tablename, IServiceBase service)
        {
            Service = service;
        }

        public ApiBaseController(IServiceBase service)
        {
            Service = service;
        }
    }
}
