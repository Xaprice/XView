using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;

namespace xview.utils
{
    public class ConfigManager
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ConfigManager)); 

        public static string GetAppConfig(string key)
        {
            try
            {
            	return ConfigurationManager.AppSettings[key];
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return string.Empty;
            }
        }

        public static void SetAppConfig(string key, string value)
        {
            try
            {
	            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
	            cfa.AppSettings.Settings[key].Value = value;
                cfa.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }

    
}
