using Aquality.Selenium.Core.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace RESTAPITest.Framework.Utils
{
    public static class APIUtil
    {
        public static RestResponse GetRestResponse(RestClient client)
        {
            Logger.Instance.Info("Get method");
            var request = new RestRequest();
            return client.GetAsync(request).Result;
        }

        public static RestResponse PostDataToClient(RestClient client, JObject payload)
        {
            Logger.Instance.Info("Post method");
            var request = new RestRequest();
            request.AddJsonBody(payload.ToString());
            return client.PostAsync(request).Result;
        }
    }
}