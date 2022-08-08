using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.BaseProvider;
using Wbf.VOL.Core.Extensions.AutofacManager;
using Wbf.VOL.Core.Utilities.Response;
using Wbf.VOL.Entity.DomainModels.System;
using Wbf.VOL.System.IRepositories.System;
using Wbf.VOL.System.IServices.System;

namespace Wbf.VOL.System.Services.System
{
    public partial class Sys_UserService : ServiceBase<Sys_User, ISys_UserRepository>,ISys_UserService, IDependency
    {
        public Sys_UserService(ISys_UserRepository repository)
             : base(repository)
        {
            Init(repository);
        }
    }
}
