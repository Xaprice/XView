using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTools
{
    public class MeasureListItem
    {
        public string ToolType { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// 周长
        /// </summary>
        public double Perimeter { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle { get; set; }

        public string Unit { get; set; }

        public void AdjustByScale(double ratio)
        {
            Length *= ratio;
            Area = Area * ratio * ratio;
            Perimeter *= ratio;
            Radius *= ratio;
        }
    }
}
