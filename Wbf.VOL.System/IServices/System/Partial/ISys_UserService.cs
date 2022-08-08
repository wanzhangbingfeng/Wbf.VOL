using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Utilities.Response;
using Wbf.VOL.Entity.DomainModels.System;

namespace Wbf.VOL.System.IServices.System
{
    public partial interface ISys_UserService
    {
        Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true);
    }
}
