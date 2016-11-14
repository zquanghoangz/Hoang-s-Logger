using Newtonsoft.Json;

namespace Logger.Cross.Requests
{
    public class LoggingEndpointRequest
    {
        [JsonProperty(PropertyName = "application_id")]
        public string ApplicationId { get; set; }
        [JsonProperty(PropertyName = "logger")]
        public string Logger { get; set; }
        [JsonProperty(PropertyName = "level")]
        public string Level { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}