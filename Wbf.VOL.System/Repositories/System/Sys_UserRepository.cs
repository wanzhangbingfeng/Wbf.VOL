using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.BaseProvider;
using Wbf.VOL.Core.EFDbContext;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Entity.DomainModels.System;
using Wbf.VOL.System.IRepositories.System;

namespace Wbf.VOL.System.Repositories.System
{
    public partial class Sys_UserRepository : RepositoryBase<Sys_User>, ISys_UserRepository
    {
        public Sys_UserRepository(VOLContext dbContext):base(dbContext)
        {

        }
    }
}
