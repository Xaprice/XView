using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview.common
{
    /// <summary>
    /// 缩放的接口
    /// </summary>
    public interface IZoomable
    {
        void ZoomIn();
        void ZoomOut();
        void RealSize();
        void FitScreen();
        double GetZoomFactor();
    }
}
