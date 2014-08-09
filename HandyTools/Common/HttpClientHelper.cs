using System;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.UI.Popups;

namespace HandyTools.Common
{
    public class HttpClientHelper
    {
        private static HttpClient _httpClient;
        private static HttpClient GetHttpClient()
        {
            return _httpClient ?? (_httpClient = new HttpClient());
        }

        public async static Task<string> GetWithGbk(String url, params object[] par)
        {
            try
            {
                var response = await GetHttpClient().GetByteArrayAsync(new Uri(string.Format(url, par)));
           
                Gb2312Encoding encoding = new Gb2312Encoding();
                return encoding.GetString(response, 0, response.Length).ToLower();
            }
            catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog(e.Message);
                dialog.ShowAsync();
            }
            return "";
        }

        public async static Task<string> GetWithUtf8(string url, params object[] par)
        {
            try
            {
                return await GetHttpClient().GetStringAsync(new Uri(string.Format(url, par)));
            }
            catch (Exception e)
            {

                MessageDialog dialog = new MessageDialog(e.Message);
                dialog.ShowAsync();
            }
            return null;
        } 
    }
}
