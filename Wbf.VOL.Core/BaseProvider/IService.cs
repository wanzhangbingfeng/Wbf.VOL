using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Entity.SystemModels;

namespace Wbf.VOL.Core.BaseProvider
{
    public interface IService<T> where T : BaseEntity
    {
    }
}
