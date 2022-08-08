using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.Enums;
using Wbf.VOL.Core.Extensions.Response;

namespace Wbf.VOL.Core.Utilities.Response
{
    public class WebResponseContent
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public object Data { get; set; }

        public WebResponseContent()
        {
        }
        public WebResponseContent(bool status)
        {
            this.Status = status;
        }
        public WebResponseContent Error(string message = "")
        {
            this.Status = false;
            this.Message = message;
            return this;
        }
        public WebResponseContent Error(ResponseType responseType)
        {
            return Set(responseType, false);
        }
        public WebResponseContent Set(ResponseType responseType, bool? status)
        {
            return this.Set(responseType, null, status);
        }

        public WebResponseContent Set(ResponseType responseType, string msg, bool? status)
        {
            if (status != null)
            {
                this.Status = (bool)status;
            }
            this.Code = ((int)responseType).ToString();
            if (!string.IsNullOrEmpty(msg))
            {
                Message = msg;
                return this;
            }
            Message = responseType.GetMsg();
            return this;
        }

        public WebResponseContent OK(ResponseType responseType)
        {
            return Set(responseType, true);
        }

    }
}
