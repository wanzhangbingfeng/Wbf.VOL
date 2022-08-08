using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wbf.VOL.Core.ObjectActionValidator
{
    public static class ValidatorContainer
    {
        //方法参数是实体配置验证字段
        public enum ValidatorModel
        {
            Login,
            LoginOnlyPassWord//只验证密码
        }
    }
}
