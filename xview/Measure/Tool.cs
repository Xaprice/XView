using System;
using System.Windows.Forms;
using System.Drawing;
using xview.UserControls;
using log4net;


namespace DrawTools
{
	/// <summary>
	/// Base class for all drawing tools
	/// </summary>
	public abstract class Tool
	{
        private static readonly ILog logger = LogManager.GetLogger(typeof(Tool));

        /// <summary>
        /// Left nous button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(ImageDrawBox drawArea, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseMove(ImageDrawBox drawArea, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseUp(ImageDrawBox drawArea, MouseEventArgs e)
        {
        }

        protected Point GetEventPointInArea(ImageDrawBox drawArea, MouseEventArgs e)
        {
            //Point p = new Point(Math.Abs(drawArea.AutoScrollPosition.X) + e.X, Math.Abs(drawArea.AutoScrollPosition.Y) + e.Y);
            //string status = "X: " + p.X + ", Y: " + p.Y;
            //((MainForm)drawArea.Parent).SetStatusStrip(status);
            //return p;

            //logger.Debug(String.Format("Tool-X:{0}, Y:{1}", e.X, e.Y));

            Point p = new Point((int)(Math.Round(e.X / drawArea.ZoomFactor)), (int)(Math.Round(e.Y / drawArea.ZoomFactor)));
            return p;
        }
    }
}
