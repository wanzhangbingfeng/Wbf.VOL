using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.ObjectActionValidator.ExpressValidator;
using static Wbf.VOL.Core.ObjectActionValidator.ValidatorContainer;

namespace Wbf.VOL.Core.ObjectActionValidator.Filters
{
    public class ObjectModelValidatorFilter: Attribute
    {
        public ObjectModelValidatorFilter(ValidatorModel validatorGroup)
        {
            MethodsParameters = validatorGroup.GetModelParameters()?.Select(x => x.ToLower())?.ToArray();
        }
        public string[] MethodsParameters { get; }
    }
}
