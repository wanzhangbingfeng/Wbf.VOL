using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wbf.VOL.Core.Enums
{
    public enum LoggerType
    {
        System = 0,
        Info,
        Success,
        Error,
        Authorzie,
        Global,
        Login,
        Exception,
        ApiException,
        HandleError,
        OnActionExecuted,
        GetUserInfo,
        Edit,
        Search,
        Add,
        Del,
        AppHome,
        ApiLogin,
        ApiPINLogin,
        ApiRegister,
        ApiModifyPwd,
        ApiSendPIN,
        ApiAuthorize,
        Ask,
        JoinMeeting,
        JoinUs,
        EditUserInfo,
        Sell,
        Buy,
        ReportPrice,
        Reply,
        TechData,
        TechSecondData,
        DelPublicQuestion,
        DelexpertQuestion,
        CreateTokenError,
        IPhoneTest,
        SDKSuccess,
        SDKSendError,
        ExpertAuthority,
        ParEmpty,
        NoToken,
        ReplaceToeken,
        KafkaException
    }

    public enum LoggerStatus
    {
        Success = 1,
        Error = 2,
        Info = 3
    }
}
