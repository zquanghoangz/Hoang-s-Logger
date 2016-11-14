using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Logger.Cross.Common;
using Logger.Cross.Responses;
using Logger.Domain;
using Logger.Service.Interfaces;

namespace Logger.Service
{
    public class ApplicationService : ServiceBase<Application>, IApplicationService
    {
        public ApplicationService(DbContext context) : base(context)
        {
        }

        public async Task<RegisterEndpointResponse> Register(string displayName)
        {
            var secret = Utils.NewId();
            var app = new Application
            {
                ApplicationId = Utils.NewId(),
                DisplayName = displayName,
                Secret = Utils.GetCode(secret)
            };
            var response = await Add(app);

            return new RegisterEndpointResponse
            {
                ApplicationId = response.ApplicationId,
                DisplayName = response.DisplayName,
                ApplicationSecret = secret //not encoded
            };
        }

        public async Task<AuthorizationResponse> GetAccessToken(string applicationId, string applicationSecret)
        {
            var encodedSecret = Utils.GetCode(applicationSecret);
            var app = (await
                Get(x => string.Equals(x.ApplicationId, applicationId) && string.Equals(x.Secret, encodedSecret)))
                .FirstOrDefault();
            return app == null ? null : new AuthorizationResponse {AccessToken = Utils.NewId()};
        }

        public async Task<bool> SaveAccessToken(string accessToken, string applicationId)
        {
            //Encoding
            accessToken = Utils.GetCode(accessToken);

            if (CacheManager.ExistToken(accessToken))
            {
                return false;
            }

            int exprationTime = 480;
            SettingService settingService = new SettingService(Context);
            var setting = (await settingService.Get(x => x.SettingKey == "EXPIRATION_TIME")).FirstOrDefault();
            if (setting!=null)
            {
                exprationTime = setting.Value;
            }

            CacheManager.SaveToken(accessToken, applicationId, exprationTime);
            return true;
        }
    }
}