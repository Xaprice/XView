using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xview
{
    public partial class ImageConfigPanelUC : UserControl
    {
        public delegate void SetImageParaEventHandler(object sender, SingleDataEventArgs<ImageConfigPara> e);
        public event SetImageParaEventHandler SetImagePara;
        private void EmitSetImageParaEvent(SingleDataEventArgs<ImageConfigPara> e)
        {
            if (SetImagePara != null)
            {
                SetImagePara(this, e);
            }
        }

        public ImageConfigPanelUC()
        {
            InitializeComponent();
        }

        private void trackBarRedGain_Scroll(object sender, EventArgs e)
        {
            EmitSetImageParaEvent(GenSetParaEventArgs(ImagePropTypeDef.RED_CHANNEL, CalcValue(sender as TrackBar)));
        }

        private void trackBarSaturation_Scroll(object sender, EventArgs e)
        {
            EmitSetImageParaEvent(GenSetParaEventArgs(ImagePropTypeDef.SATURATION, CalcValue(sender as TrackBar)));
        }

        private void trackBarGreenGain_Scroll(object sender, EventArgs e)
        {
            EmitSetImageParaEvent(GenSetParaEventArgs(ImagePropTypeDef.GREEN_CHANNEL, CalcValue(sender as TrackBar)));
        }

        private void trackBarBlueGain_Scroll(object sender, EventArgs e)
        {
            EmitSetImageParaEvent(GenSetParaEventArgs(ImagePropTypeDef.BLUE_CHANNEL, CalcValue(sender as TrackBar)));
        }

        private void trackBarGamma_Scroll(object sender, EventArgs e)
        {
            EmitSetImageParaEvent(GenSetParaEventArgs(ImagePropTypeDef.GAMMA, CalcValue(sender as TrackBar)));
        }

        private void trackBarContrast_Scroll(object sender, EventArgs e)
        {
            EmitSetImageParaEvent(GenSetParaEventArgs(ImagePropTypeDef.CONTRAST, CalcValue(sender as TrackBar)));
        }

        private SingleDataEventArgs<ImageConfigPara> GenSetParaEventArgs(int type, double value)
        {
            ImageConfigPara para = new ImageConfigPara() { Type = type, Value = value };
            return new SingleDataEventArgs<ImageConfigPara>() { Data = para};
        }

        private double CalcValue(TrackBar trackBar)
        {
            return Convert.ToDouble(trackBar.Value) / 10.0;
        }
    }

    public class ImageConfigPara
    {
        public int Type{get;set;}

        public double Value{get;set;}

        public bool IsColorChannel()
        {
            return (Type == ImagePropTypeDef.RED_CHANNEL || Type == ImagePropTypeDef.GREEN_CHANNEL || Type == ImagePropTypeDef.BLUE_CHANNEL);
        }

        public bool IsGamma()
        {
            return Type == ImagePropTypeDef.GAMMA;
        }
    }
}
