using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.BaseProvider;
using Wbf.VOL.Core.Extensions.AutofacManager;
using Wbf.VOL.Entity.DomainModels.System;

namespace Wbf.VOL.System.IRepositories.System
{
    public partial interface ISys_UserRepository: IDependency, IRepository<Sys_User>
    {
    }
}
