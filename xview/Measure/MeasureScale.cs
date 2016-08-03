using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xview.Constants;

namespace xview.Measure
{
    public class MeasureScale
    {
        public string Name { get; set; }

        public double Pixels { get; set; }

        public double UnitValue { get; set; }

        private UnitTypeDef unitType = UnitTypeDef.Micrometer;
        public UnitTypeDef UnitType
        {
            get { return unitType; }
            set
            {
                unitType = value;
                switch (unitType)
                {
                    case UnitTypeDef.Pixel:
                        unitTypeDisplayName = "像素";
                        break;
                    case UnitTypeDef.Centimeter:
                        unitTypeDisplayName = "厘米";
                        break;
                    case UnitTypeDef.Milimeter:
                        unitTypeDisplayName = "毫米";
                        break;
                    case UnitTypeDef.Micrometer:
                        unitTypeDisplayName = "微米";
                        break;
                    case UnitTypeDef.Nanometer:
                        unitTypeDisplayName = "纳米";
                        break;
                    default:
                        unitTypeDisplayName = "";
                        break;
                }
            }
        }

        private string unitTypeDisplayName = "";
        public string UnitTypeDisplayName
        {
            get { return unitTypeDisplayName; }
        }

        public string ToString()
        {
            return String.Format("{0}{1}={2}像素", UnitValue, unitTypeDisplayName, Pixels);
        }
    }
}
