using System;
using System.Windows.Forms;
using System.Drawing;

namespace DrawTools
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>
	class DrawEllipse : DrawTools.DrawRectangle
	{
		public DrawEllipse() : this(0, 0, 1, 1)
		{
		}

        public DrawEllipse(int x, int y, int width, int height) : base()
        {
            Rectangle = new Rectangle(x, y, width, height);
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawEllipse drawEllipse = new DrawEllipse();
            drawEllipse.Rectangle = this.Rectangle;

            FillDrawObjectFields(drawEllipse);
            return drawEllipse;
        }


        public override void Draw(Graphics g, double zoomFactor = 1.0)
        {
            Pen pen = new Pen(Color, PenWidth);

            int zX = (int)(Math.Round(Rectangle.X * zoomFactor));
            int zY = (int)(Math.Round(Rectangle.Y * zoomFactor));
            int zW = (int)(Math.Round(Rectangle.Width * zoomFactor));
            int zH = (int)(Math.Round(Rectangle.Height * zoomFactor));
            Rectangle zoomedRect = new Rectangle(zX, zY, zW, zH);
            g.DrawEllipse(pen, DrawRectangle.GetNormalizedRectangle(zoomedRect));

            pen.Dispose();
        }

        public override MeasureListItem GetMeasureListItem()
        {
            MeasureListItem item = new MeasureListItem();
            item.ToolType = "Õ÷‘≤";

            int a = Math.Max(Rectangle.Width, Rectangle.Height); // Õ÷‘≤≥§÷·
            int b = Math.Min(Rectangle.Width, Rectangle.Height); //Õ÷‘≤∂Ã÷·

            item.Area = Math.PI * a * b; //Õ÷‘≤√Êª˝£∫s=pi*a*b
            item.Perimeter = 2*Math.PI * b + 4.0*(a-b); //Õ÷‘≤÷‹≥§£∫ c = 2*pi*b+4(a-b)
            item.Radius = (a + b) / 2.0;

            return item;
        }


	}
}
