using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wbf.VOL.Core.ObjectActionValidator.ExpressValidator
{
    public class ObjectModelValidatorState
    {
        public ObjectModelValidatorState()
        {
            this.Status = true;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public bool HasModelContent { get; set; }
    }
}
