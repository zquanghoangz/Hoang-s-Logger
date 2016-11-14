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
    /// Application controller
    /// </summary>
    public class ApplicationController : ApiController
    {
        private readonly IApplicationService _applicationService;

        /// <summary>
        /// Construtor to create application service
        /// </summary>
        /// <param name="appService"></param>
        public ApplicationController(IApplicationService appService)
        {
            _applicationService = appService;
        }

        /// <summary>
        /// There is one way to authenticate through Logger Api v1 using a basic authentication
        /// </summary>
        /// <returns>AuthorizationResponse</returns>
        [HttpPost, Route("api/auth")]
        public async Task<AuthorizationResponse> Authorization()
        {
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains(Constants.Header_ApplicationId) && headers.Contains(Constants.Header_ApplicationSecret))
            {
                var applicationId = headers.GetValues(Constants.Header_ApplicationId).First();
                var applicationSecret = headers.GetValues(Constants.Header_ApplicationSecret).First();

                try
                {
                    //Get token
                    AuthorizationResponse response =
                        await _applicationService.GetAccessToken(applicationId, applicationSecret);

                    if (response == null)
                    {
                        throw new HttpResponseException(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            Content = new StringContent(Constants.HttpResponse_401)
                        });
                    }

                    //Save token in section
                    var saved = await _applicationService.SaveAccessToken(response.AccessToken, applicationId);
                    if (!saved)
                    {
                        throw new HttpResponseException(new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotAcceptable,
                            Content = new StringContent(Constants.HttpResponse_406)
                        });
                    }

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

        /// <summary>
        /// Register a new application within the Logger Service, unauthenticated end point
        /// </summary>
        /// <param name="request">RegisterEndpointRequest</param>
        /// <returns>RegisterEndpointResponse</returns>
        [HttpPost, Route("api/register")]
        public async Task<RegisterEndpointResponse> RegisterEndpoint(RegisterEndpointRequest request)
        {
            if (request == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(Constants.HttpResponse_400)
                });
            }

            try
            {
                //Do register and get data to response
                var app = await _applicationService.Register(request.DisplayName);

                if (app == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        Content = new StringContent(Constants.HttpResponse_403)
                    });
                }

                return app;
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
    }
}