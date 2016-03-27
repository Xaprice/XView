using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawTools
{
    public class MeasureListItem
    {
        public string ToolType { get; set; }

        public double Length { get; set; }

        public double Area { get; set; }

        public double Perimeter { get; set; }

        public double Radius { get; set; }

        public double Angle { get; set; }

        public string Unit { get; set; }
    }
}
