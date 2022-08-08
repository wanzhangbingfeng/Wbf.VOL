using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Extensions;
using Wbf.VOL.Core.Extensions.AutofacManager;

namespace Wbf.VOL.Core.BaseProvider.ServerMapPath
{
    public interface IPathProvider: IDependency
    {
        string MapPath(string path);
        string MapPath(string path, bool rootPath);
    }

    public class PathProvider : IPathProvider
    {
        private IWebHostEnvironment _hostingEnvironment;
        public string MapPath(string path)
        {
            return MapPath(path, false);
        }

        public string MapPath(string path, bool rootPath)
        {
            if (rootPath)
            {
                if (_hostingEnvironment.WebRootPath == null)
                {
                    _hostingEnvironment.WebRootPath = _hostingEnvironment.ContentRootPath + "/wwwroot".ReplacePath();
                }
                return Path.Combine(_hostingEnvironment.WebRootPath, path).ReplacePath();
            }
            return Path.Combine(_hostingEnvironment.ContentRootPath, path).ReplacePath();
        }
    }
}
