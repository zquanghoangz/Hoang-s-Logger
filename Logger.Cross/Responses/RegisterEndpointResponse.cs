using Newtonsoft.Json;

namespace Logger.Cross.Responses
{
    public class RegisterEndpointResponse
    {
        [JsonProperty(PropertyName = "application_id")]
        public string ApplicationId { get; set; }
        [JsonProperty(PropertyName = "application_secret")]
        public string ApplicationSecret { get; set; }
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
    }
}