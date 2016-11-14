namespace Logger.Cross.Common
{
    public static class Constants
    {
        public const string HttpResponse_400 = "400 Bad Request";
        public const string HttpResponse_401 = "401 Unauthorized";
        public const string HttpResponse_403 = "403 Forbidden";
        public const string HttpResponse_403_LimitExceeded = "403 Application Rate Limit Exceeded. Must be wait 5 minutes for request again";
        public const string HttpResponse_406 = "406 Not Acceptable. Only one active session per application is allowed";
        public const string HttpResponse_500 = "500 Internal Server Error";

        public const string Header_AccessToken = "access_token";
        public const string Header_ApplicationId = "application_id";
        public const string Header_ApplicationSecret = "application_secret";
    }
}
