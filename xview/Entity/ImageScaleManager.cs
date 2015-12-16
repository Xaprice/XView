using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview.Entity
{
    public class ImageScaleManager
    {
        /// <summary>
        /// 视频/图像 窗口比例Map
        /// 窗口ID -> windowScale
        /// </summary>
        public Dictionary<int, ImageScale> ImageScaleMap { get; set; }

        /// <summary>
        /// 最小比例
        /// </summary>
        public double MinScale { get; set; }

        /// <summary>
        /// 最大比例
        /// </summary>
        public double MaxScale { get; set; }

        /// <summary>
        /// 缩放步长
        /// </summary>
        public double ZoomStep { get; set; }

        public ImageScale GetWindowScale(int winId)
        {
            ImageScale winScale = null;
            if (this.ImageScaleMap.TryGetValue(winId, out winScale))
                return winScale;
            else
                return null;
        }

        public void SetWindowScale(int winId, double scale)
        {
            if (this.ImageScaleMap.ContainsKey(winId))
            {
                this.ImageScaleMap[winId].Scale = scale;
            }
        }
    }
}
