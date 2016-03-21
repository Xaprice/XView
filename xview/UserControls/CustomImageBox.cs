using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace xview.UserControls
{
    public class CustomImageBox : ImageBox
    {
        public void init()
        {
            this.FunctionalMode = FunctionalModeOption.Minimum;
            this.PanableAndZoomable = false;
            this.DoubleBuffered = true;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;
            g.DrawLine(System.Drawing.Pens.Red, this.Left, this.Top, this.Right, this.Bottom);
        }
    }
}
