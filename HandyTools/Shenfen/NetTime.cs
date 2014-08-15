using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HandyTools.Shenfen
{
    class NetTime
    {
        /// <summary>  
        /// 获取标准北京时间，读取http://www.beijing-time.org/time.asp  
        /// </summary>  
        /// <returns>返回网络时间</returns>  
        public async Task<DateTime> GetBeijingTime()
        {

            DateTime dt = DateTime.Now;
            try
            {
                HttpClient httpClient = new HttpClient();
                string html = await httpClient.GetStringAsync("http://www.beijing-time.org/time.asp");
                string[] tempArray = html.Split(';');
                for (int i = 0; i < tempArray.Length; i++)
                {
                    tempArray[i] = tempArray[i].Replace("\r\n", "");
                }

                string year = tempArray[1].Split('=')[1];
                string month = tempArray[2].Split('=')[1];
                string day = tempArray[3].Split('=')[1];
                string hour = tempArray[5].Split('=')[1];
                string minite = tempArray[6].Split('=')[1];
                string second = tempArray[7].Split('=')[1];

                dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            }
            catch (Exception e)
            {
                new MessageDialog(e.Message, "提示").ShowAsync();
            }
           
            return dt;

        }
    }
}
