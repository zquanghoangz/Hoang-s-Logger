using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Logger.Cross.Common;
using Logger.Cross.Requests;
using Logger.Cross.Responses;
using Logger.Domain;
using Logger.Service.Interfaces;

namespace Logger.Service
{
    public class LogService : ServiceBase<Log>, ILogService
    {
        public LogService(DbContext context) : base(context)
        {
        }

        public async Task<LoggingEndpointResponse> AddLog(LoggingEndpointRequest logData)
        {
            var log = new Log
            {
                Logger = logData.Logger,
                Level = logData.Level,
                Message = logData.Message,
                ApplicationId = logData.ApplicationId
            };

            var response = await Add(log);

            return new LoggingEndpointResponse
            {
                Success = response != null
            };
        }

        public bool CheckAccessToken(string token)
        {
            token = Utils.GetCode(token);
            return CacheManager.ExistToken(token);
        }

        public bool UpdateNCheckLimitExceeded(string token)
        {
            token = Utils.GetCode(token);
            DateTimeOffset requestTime = DateTimeOffset.Now;

            //Check exceeded limit
            if (CacheManager.IsExceededLimit(token, requestTime))
            {
                return true;
            }

            //Add request time
            CacheManager.AddRequestTime(token, requestTime);

            //Check exceeded limit
            if (CacheManager.IsExceededLimit(token, requestTime))
            {
                return true;
            }

            return false;
        }
    }
}