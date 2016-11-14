using System.Threading.Tasks;
using Logger.Cross.Responses;
using Logger.Domain;

namespace Logger.Service.Interfaces
{
    public interface IApplicationService : IServiceBase<Application>
    {
        Task<RegisterEndpointResponse> Register(string displayName);
        Task<AuthorizationResponse> GetAccessToken(string applicationId, string applicationSecret);
        Task<bool> SaveAccessToken(string accessToken, string applicationId);
    }
}