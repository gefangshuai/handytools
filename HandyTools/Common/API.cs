using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyTools.Common
{
    public class API
    {
        public static string HaomaUrl = "http://www.ip138.com:8080/search.asp?action=mobile&mobile={0}";
        public static string GuhuaUrl = "http://tel.51240.com/{0}__tel/";
        public static string JiXiong = "http://jx.ip138.com/default.asp?k={0}";
        public static string ChangYongData = "http://git.oschina.net/gefangshuai/pushdata/raw/master/handytools/changyongdata.json";
        public static string ZipCode = "http://quhao.51240.com/{0}__quhao/";
        public static string StarDay = "http://api.uihoo.com/astro/astro.http.php?fun=day&id={0}&format=json";
        public static string StarTomorrow = "http://api.uihoo.com/astro/astro.http.php?fun=tomorrow&id={0}&format=json";
        public static string StarWeek = "http://api.uihoo.com/astro/astro.http.php?fun=week&id={0}&format=json";
        public static string StarMonth = "http://api.uihoo.com/astro/astro.http.php?fun=month&id={0}&format=json";
        public static string StarYear = "http://api.uihoo.com/astro/astro.http.php?fun=year&id={0}&format=json";
        public static string StarAQ = "http://api.uihoo.com/astro/astro.http.php?fun=love&id={0}&format=json";

        public static string LocalTime = "http://gb.weather.gov.hk/cgi-bin/hko/localtime.pl";
    }
}
