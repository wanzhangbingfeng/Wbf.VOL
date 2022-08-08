using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.ObjectActionValidator.ExpressValidator;

namespace Wbf.VOL.Core.ObjectActionValidator
{
    public class NullObjectModelValidator : IObjectModelValidator
    {
        public void Validate(ActionContext actionContext, ValidationStateDictionary? validationState, string prefix, object? model)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                actionContext.ModelValidator(prefix, model);
                return;
            }
        }
    }
}
