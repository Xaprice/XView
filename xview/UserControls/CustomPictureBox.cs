using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace xview.UserControls
{
    public class CustomPictureBox: System.Windows.Forms.PictureBox
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;
            g.DrawLine(System.Drawing.Pens.Red, this.Left, this.Top, this.Right, this.Bottom);
        }
    }
}
