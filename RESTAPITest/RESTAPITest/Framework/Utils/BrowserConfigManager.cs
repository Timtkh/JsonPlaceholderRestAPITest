using Aquality.Selenium.Core.Logging;
using Newtonsoft.Json.Linq;

namespace RESTAPITest.Framework.Utils
{
    public class BrowserConfigManager
    {
        private static readonly JObject configObject = GetConfigurationObject();
        public static string BaseUrl { get; private set; } = configObject.Value<string>("BaseUrl");
        public static string PostsUrl { get; private set; } = configObject.Value<string>("PostsUrl");
        public static string UsersUrl { get; private set; } = configObject.Value<string>("UsersUrl");

        private static JObject GetConfigurationObject()
        {
            Logger.Instance.Info("Get browser configuration object");
            using var fs = new StreamReader(@"TestSolution\Configs\browserconfig.json");
            return JObject.Parse(fs.ReadToEnd());
        }
    }
}