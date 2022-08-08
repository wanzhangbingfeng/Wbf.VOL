using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.ObjectActionValidator.Filters;
using static Wbf.VOL.Core.ObjectActionValidator.ValidatorContainer;

namespace Wbf.VOL.Core.ObjectActionValidator.ExpressValidator
{
    public static class MethodsValidator
    {
        /// <summary>
        /// 方法上的model校验配置
        /// </summary>
        public static Dictionary<string, string[]> ValidatorCollection { get; } = new Dictionary<string, string[]>();

        /// <summary>
        /// model校验
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="prefix"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static void ModelValidator(this ActionContext actionContext, string prefix, object model)
        {
            string[] parameters = actionContext.GetModelValidatorParams();
            //没有设置模型校验的直接返回
            if (parameters == null) return;
            if (model == null)
            {
                actionContext.ErrorResult("没有获取到参数");
                return;
            }
            //model==list未判断
            PropertyInfo[] properties = model.GetType().GetProperties().Where(x => parameters.Contains(x.Name.ToLower())).ToArray();
            foreach (var item in properties)
            {
                if (!item.ValidationRquiredValueForDbType(item.GetValue(model), out string message))
                {
                    actionContext.ErrorResult(message);
                    return;
                }
            }
            actionContext.OkModelResult();
        }

        /// <summary>
        /// 是否添加了ModelValidator实体校验
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public static string[] GetModelValidatorParams(this ActionContext actionContext)
        {
            return (actionContext.ActionDescriptor
                     .EndpointMetadata
                     .Where(item => item is ObjectModelValidatorFilter)
                     .FirstOrDefault() as ObjectModelValidatorFilter)?.MethodsParameters;
        }

        /// <summary>
        ///参数验证没有通过的直接返回校验结果
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="message"></param>
        public static void ErrorResult(this ActionContext actionContext, string message)
        {
            ObjectModelValidatorState objectModel = actionContext.HttpContext.GetService<ObjectModelValidatorState>();
            if (!objectModel.Status)
            {
                return;
            }
            objectModel.Status = false;
            objectModel.Message = message;
        }

        /// <summary>
        /// 获取方法上绑定的model校验字段
        /// </summary>
        /// <param name="validatorGroup"></param>
        /// <returns></returns>
        public static string[] GetModelParameters(this ValidatorModel validatorGroup)
        {
            return validatorGroup.ToString().GetModelParameters();
        }

        /// <summary>
        /// 获取方法上绑定的model校验字段
        /// </summary>
        /// <param name="validatorGroup"></param>
        /// <returns></returns>
        public static string[] GetModelParameters(this string validatorGroup)
        {
            if (!ValidatorCollection.TryGetValue(validatorGroup.ToLower(), out string[] values))
            {
                Console.WriteLine($"未注册{validatorGroup}参数的表达式");
                throw new Exception($"未注册{validatorGroup}参数的表达式");
            }
            return values;
        }

        public static void OkModelResult(this ActionContext actionContext)
        {
            ObjectModelValidatorState objectModel = actionContext.HttpContext.GetService<ObjectModelValidatorState>();
            objectModel.HasModelContent = true;
        }
    }
}
