using Newtonsoft.Json;

namespace Logger.Cross.Requests
{
    public class RegisterEndpointRequest
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
    }
}