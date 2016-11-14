using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Logger.Cross.Common;
using Logger.Cross.Requests;
using Logger.Cross.Responses;
using Logger.Service.Interfaces;

namespace Logger.Web.Controllers
{
    /// <summary>
    /// Log controller
    /// </summary>
    public class LogController : ApiController
    {
        private readonly ILogService _logService;

        /// <summary>
        ///     Construtor to create log service
        /// </summary>
        /// <param name="logService"></param>
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        ///     Sends a new application log message to be stored in the Logger Service, an access  token is required. Please, pay
        ///     close attention to the rate limiting access
        /// </summary>
        /// <param name="request">LoggingEndpointRequest</param>
        /// <returns>LoggingEndpointResponse</returns>
        [HttpPost, Route("api/log")]
        public async Task<LoggingEndpointResponse> LoggingEndpoint(LoggingEndpointRequest request)
        {
            if (request == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(Constants.HttpResponse_400)
                });
            }

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains(Constants.Header_AccessToken))
            {
                try
                {
                    var token = headers.GetValues(Constants.Header_AccessToken).First();

                    //Check token
                    bool isValidToken = _logService.CheckAccessToken(token);
                    if (!isValidToken)
                    {
                        throw new HttpResponseException(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            Content = new StringContent(Constants.HttpResponse_403)
                        });
                    }

                    //Check
                    if (_logService.UpdateNCheckLimitExceeded(token))
                    {
                        throw new HttpResponseException(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            Content = new StringContent(Constants.HttpResponse_403_LimitExceeded)
                        });
                    }
                    //Add log
                    LoggingEndpointResponse response = await _logService.AddLog(request);

                    if (response == null)
                    {
                        throw new HttpResponseException(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            Content = new StringContent(Constants.HttpResponse_403)
                        });
                    }

                    //Response
                    return response;
                }
                catch (Exception)
                {
                    throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent(Constants.HttpResponse_500)
                    });
                }
            }

            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(Constants.HttpResponse_400)
            });
        }
    }
}