using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview
{
    /// <summary>
    /// nouse
    /// </summary>
    public class ImageProps
    {
        public ImageProps()
        {
            RedGain = 1.0;
            GreenGain = 1.0;
            BlueGain = 1.0;
            Gamma = 1.0;
            Constrast = 1.0;
            Saturation = 1.0;
        }

        public double RedGain { get; set; }

        public double GreenGain { get; set; }

        public double BlueGain { get; set; }

        public double Gamma { get; set; }

        public double Constrast { get; set; }

        public double Saturation { get; set; }

        public double GetPropValueByType(int type)
        {
            double retVal = -1;
            switch(type)
            {
                case ImagePropTypeDef.BLUE_CHANNEL:
                    retVal = this.BlueGain;
                    break;
                case ImagePropTypeDef.GREEN_CHANNEL:
                    retVal = this.GreenGain;
                    break;
                case ImagePropTypeDef.RED_CHANNEL:
                    retVal = this.RedGain;
                    break;
                default:
                    break;
            }
            return retVal;
        }
    }
}
