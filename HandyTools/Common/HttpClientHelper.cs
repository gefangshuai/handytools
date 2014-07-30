using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace HandyTools.Common
{
    public class HttpClientHelper
    {
        private static HttpClient _httpClient;
        private static HttpClient GetHttpClient()
        {
            return _httpClient ?? (_httpClient = new HttpClient());
        }

        public async static Task<string> Get(String url, params object[] par)
        {
            var response = await GetHttpClient().GetByteArrayAsync(new Uri(string.Format(url, par)));
           
            Gb2312Encoding encoding = new Gb2312Encoding();
            return encoding.GetString(response, 0, response.Length).ToLower();
        }
    }
}
