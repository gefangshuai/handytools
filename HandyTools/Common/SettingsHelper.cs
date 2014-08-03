using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HandyTools.Common
{
    public class SettingsHelper
    {
        private static ApplicationDataContainer GetLocalSettings()
        {
             return ApplicationData.Current.LocalSettings;
        }

        public static IEnumerable<string> GetShoujiHistory()
        {
            ApplicationDataContainer settings = GetLocalSettings();
            if (settings.Values.ContainsKey("shoujiHistory"))
            {
                string history = settings.Values["shoujiHistory"] as string;
                if (history != null)
                {
                    if (history.EndsWith(","))
                        history = history.Remove(history.Length - 1);
                    return history.Split(',');
                }
            }
            return null;
        }

        public static void AddShoujiHistory(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return;
            ApplicationDataContainer settings = GetLocalSettings();
            string history = "";
            if (settings.Values.ContainsKey("shoujiHistory") && settings.Values["shoujiHistory"] != null)
            {
                history = settings.Values["shoujiHistory"] as string;
                history += str + ",";
            }
            else
            {
                history = str + ",";
            }
            settings.Values["shoujiHistory"] = history;
        }

        public static void ClearShoujiHistory()
        {
            ApplicationDataContainer settings = GetLocalSettings();
            if (settings.Values.ContainsKey("shoujiHistory"))
            {
                settings.Values["shoujiHistory"] = null;
            }
        }
    }
}
