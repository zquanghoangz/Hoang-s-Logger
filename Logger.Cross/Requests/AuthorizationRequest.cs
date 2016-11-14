using Newtonsoft.Json;

namespace Logger.Cross.Requests
{
    public class AuthorizationRequest
    {
        [JsonProperty(PropertyName = "application_id")]
        public string ApplicationId { get; set; }
        [JsonProperty(PropertyName = "application_secret")]
        public string ApplicationSecret { get; set; }
    }
}