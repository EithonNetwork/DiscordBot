using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic
{
    class ConfigurationHandler
    {
        public static string GetStringOption(string configOptionName)
        {
            var configOption = ConfigurationManager.AppSettings[configOptionName];
            return configOption;
        }
        public static ulong GetUlongOption(string configOptionName)
        {
            var configOption = ConfigurationManager.AppSettings[configOptionName];
            ulong ulongOption = ulong.Parse(configOption);
            return ulongOption;
        }
    }
}
