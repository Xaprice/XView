using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTools
{
    public class MeasureStatisticItem
    {
        /// <summary>
        /// 测量工具类型
        /// </summary>
        public string ToolType { get; set; }

        /// <summary>
        /// 统计项目类别
        /// </summary>
        public string StatisticType { get; set; }

        /// <summary>
        /// 统计数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 平均值
        /// </summary>
        public double AverageValue { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue{get;set;}

        /// <summary>
        /// 测量单位
        /// </summary>
        public string Unit { get; set; }

    }
}
