using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using HandyTools.Haoma;
using HtmlAgilityPack;

namespace HandyTools.Common
{
    public class HtmlHelper
    {
        private static HtmlDocument LoadHtml(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }
        public static Guishudi ParseGuishudiResult(string html)
        {
            HtmlDocument document = LoadHtml(html);
            var tableHtml = document.DocumentNode.Descendants("table").LastOrDefault().InnerHtml;
            if (tableHtml.Contains("ip138.com查询结果"))
            {
                Guishudi guishudi = new Guishudi();
                document = LoadHtml(tableHtml);
                foreach (var htmlNode in document.DocumentNode.Descendants("tr"))
                {
                    string text = HtmlUtilities.ConvertToText(htmlNode.InnerText);
                    if (text.Contains("卡号归属地"))
                        guishudi.Attribution = text.Replace("卡号归属地", "");
                    if (text.Contains("卡 类 型"))
                        guishudi.Type = text.Replace("卡 类 型", "");
                    if (text.Contains("区 号"))
                        guishudi.Code = text.Replace("区 号", "");
                    if (text.Contains("邮 编"))
                        guishudi.ZipCode = text.Replace("邮 编", "").Replace("更详细的..", "");
                }

                return guishudi;
            }
            return null;
        }
    }
}
