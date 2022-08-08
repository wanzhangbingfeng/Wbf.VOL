using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Enums;

namespace Wbf.VOL.Core.Extensions
{
    public  static class CacheKeyExtensions
    {
        public static string GetUserIdKey(this int userId)
        {
            return CPrefix.UID.ToString() + userId;
        }
    }
}
