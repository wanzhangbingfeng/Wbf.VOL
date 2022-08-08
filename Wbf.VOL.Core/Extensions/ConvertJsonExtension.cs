using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wbf.VOL.Core.Extensions
{
    public static class ConvertJsonExtension
    {
        public static string Serialize(this object obj, JsonSerializerSettings formatDate = null)
        {
            if (obj == null) return null;
            formatDate = formatDate ?? new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            return JsonConvert.SerializeObject(obj, formatDate);
        }
    }
}
