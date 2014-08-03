using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using Windows.Data.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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
            if (html.Contains("table"))
            {
                var tableHtml = document.DocumentNode.Descendants("table").LastOrDefault().InnerHtml;
                if (tableHtml != null && tableHtml.Contains("ip138.com查询结果"))
                {
                    Guishudi guishudi = new Guishudi();
                    document = LoadHtml(tableHtml);
                    foreach (var htmlNode in document.DocumentNode.Descendants("tr"))
                    {
                        string text = HtmlUtilities.ConvertToText(htmlNode.InnerText);
                        if (text.Contains("卡号归属地"))
                            guishudi.Attribution = text.Replace("卡号归属地", "").Trim();
                        if (text.Contains("卡 类 型"))
                            guishudi.Type = text.Replace("卡 类 型", "").Trim();
                        if (text.Contains("区 号"))
                            guishudi.Code = text.Replace("区 号", "").Trim();
                        if (text.Contains("邮 编"))
                            guishudi.ZipCode = text.Replace("邮 编", "").Replace("更详细的..", "").Trim();
                    }
                    return guishudi;
                }
            }
            return null;
        }

        internal static JiXiong ParseJiXiongResult(string html)
        {
            HtmlDocument document = LoadHtml(html);
            var table = document.DocumentNode.Descendants("table").FirstOrDefault(n => n.Attributes.Contains("class") &&
                                                                                           n.Attributes["class"].Value.Contains("t4"));
            if (table != null)
            {
                var tableHtml = table.InnerHtml;
                JiXiong jiXiong = new JiXiong();
                if (tableHtml != null && tableHtml.Contains("吉凶推理"))
                {
                    document = LoadHtml(tableHtml);
                    foreach (var htmlNode in document.DocumentNode.Descendants("tr"))
                    {
                        string text = HtmlUtilities.ConvertToText(htmlNode.InnerText);
                        if (text.Contains("吉凶推理："))
                            jiXiong.Tuili = text.Replace("吉凶推理：", "").Trim();
                        if (text.Contains("暗示的信息："))
                            jiXiong.AnShi = text.Replace("暗示的信息：", "").Trim();
                        if (text.Contains("诗云："))
                            jiXiong.ShiYun = text.Replace("诗云：", "").Trim();
                        if (text.Contains("个性系数："))
                            jiXiong.GeXingXiShu = text.Replace("个性系数：", "").Replace("性格类型：", "").Substring(0, 2).Trim();
                        if (text.Contains("性格类型："))
                            jiXiong.XingGeLeiXing = text.Replace("个性系数：", "").Replace("性格类型：", "").Substring(2).Trim();
                        if (text.Contains("具体表现："))
                            jiXiong.JuTiBiaoXian = text.Replace("具体表现：", "").Trim();
                        if (text.Contains("谚语："))
                            jiXiong.YanYu = text.Replace("谚语：", "").Trim();
                    }
                    return jiXiong;
                }
            }
            return null;
        }

        internal static IEnumerable<ChangyongCategory> ParseChangyong(string data)
        {
            try
            {
                JsonObject jsonObject = JsonObject.Parse(data);
                var categories = new List<ChangyongCategory>();
                string update = jsonObject["update"].GetString();
                JsonArray jsonArray = jsonObject["data"].GetArray();
                foreach (var item in jsonArray)
                {
                    var itemObject = item.GetObject();
                    ChangyongCategory category = new ChangyongCategory
                    {
                        UpdateTime = update,
                        Title = itemObject.Keys.First(),
                        Changyongs = new List<Changyong>(),
                        Width = Window.Current.Bounds.Width
                    };
                    var codeJsonArray = itemObject.Values.First().GetArray();
                    foreach (var codeItem in codeJsonArray)
                    {
                        var codeObject = codeItem.GetObject();
                        var changyong = new Changyong() { Name = codeObject.Keys.First(), Value = codeObject.Values.First().GetString(), Category = category.Title};
                        category.Changyongs.Add(changyong);
                    }
                    categories.Add(category);
                }
                return categories;
            }
            catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog(e.Message,"错误");
                dialog.ShowAsync();
            }
            return null;
        }
    }
}
