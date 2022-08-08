using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Utilities.Response;
using Wbf.VOL.Entity.SystemModels;

namespace Wbf.VOL.Core.BaseProvider
{
    public abstract class ServiceBase<T, TRepository>
        where T : BaseEntity
        where TRepository : IRepository<T>
    {
        protected IRepository<T> repository;
        private WebResponseContent Response { get; set; }

        public ServiceBase(TRepository repository)
        {
            Response = new WebResponseContent(true);
            this.repository = repository;
        }

        protected virtual void Init(IRepository<T> repository)
        {

        }
    }
}
