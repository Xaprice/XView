using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview.utils
{
    /// <summary>
    /// 日志助手类
    /// </summary>
    public class LogUtil
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(LogUtil));

        public ILog GetLogger()
        {
            return logger;
        }

    }
}
