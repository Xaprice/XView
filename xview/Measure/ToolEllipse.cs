namespace DrawTools
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using xview.UserControls;
    using System.Resources;
    
    /// <summary>
	/// Ellipse tool
	/// </summary>
	class ToolEllipse : DrawTools.ToolRectangle
	{
		public ToolEllipse()
		{
            //Cursor = new Cursor(GetType(), "Ellipse.cur");
            Cursor = Cursors.Cross;
		}

        public override void OnMouseDown(ImageDrawBox drawArea, MouseEventArgs e)
        {
            Point p = GetEventPointInArea(drawArea, e);
            AddNewObject(drawArea, new DrawEllipse(p.X, p.Y, 1, 1));
        }

	}
}
