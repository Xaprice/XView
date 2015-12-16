using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;

namespace xview.Image
{
    /// <summary>
    /// 还缺少do/undo的支持
    /// </summary>
    public class XImageProcessor
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(XImageProcessor));

        public Image<Bgr, Byte> Image { get; set; }

        /// <summary>
        /// 灰度化
        /// </summary>
        /// <returns></returns>
        public bool Gray()
        {
            try
            {
            	Image = Image.Convert<Gray, Byte>().Convert<Bgr, Byte>();
                return true;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 直方均衡化
        /// </summary>
        /// <returns></returns>
        public bool HistogramEqualize()
        {
            XMessageDialog.Debug("未实现...");
            return true;
        }
    }
}
