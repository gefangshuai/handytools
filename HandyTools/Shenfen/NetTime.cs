using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                byte[] result =
                    await httpClient.GetByteArrayAsync("http://www.sogou.com/websearch/features/standardtimeadjust.jsp");
                long times = long.Parse(System.Text.Encoding.UTF8.GetString(result, 0, result.Length).Replace("\r", "").Replace("\n", "").Replace("standardtime(", "").Replace(",0);", ""));
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0); 
                long tricks1970 = dt1970.Ticks;//1970年1月1日刻度         
                long timeTricks = tricks1970 + times * 10000;//日志日期刻度         
                dt = new DateTime(timeTricks).AddHours(8);//转化为DateTime
            }
            catch (Exception e)
            {
                new MessageDialog("获取失败，请刷新重试！", "提示").ShowAsync();
            }
           
            return dt;

        }
    }
}
