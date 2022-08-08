using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wbf.VOL.Entity.AttributeManager
{
    public class PermissionTableAttribute: Attribute
    {
        /// <summary>
        /// 需要控制权限的表名与Sys_Menu表的表名必须一致
        /// </summary>
        public string Name { get; set; }
    }
}
