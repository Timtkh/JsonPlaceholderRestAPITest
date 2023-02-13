using Aquality.Selenium.Core.Logging;

namespace RESTAPITest.Framework.Utils
{
    public static class RandomUtil
    {
        private static readonly Random random = new();
        private const string Chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";

        public static string GetRandomLatinString()
        {
            Logger.Instance.Info("Get random latin string");
            return new string(Enumerable.Repeat(Chars, random.Next(1, 10)).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}