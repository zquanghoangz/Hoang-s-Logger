using System.Threading.Tasks;
using Logger.Cross.Requests;
using Logger.Cross.Responses;
using Logger.Domain;

namespace Logger.Service.Interfaces
{
    public interface ILogService: IServiceBase<Log>
    {
        Task<LoggingEndpointResponse> AddLog(LoggingEndpointRequest logData);
        bool CheckAccessToken(string token);
        bool UpdateNCheckLimitExceeded(string token);
    }
}