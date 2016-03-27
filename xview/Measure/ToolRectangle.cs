using System;
using System.Windows.Forms;
using System.Drawing;
using xview.UserControls;


namespace DrawTools
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : DrawTools.ToolObject
	{

		public ToolRectangle()
		{
            //Cursor = new Cursor(GetType(), "Rectangle.cur");
            Cursor = Cursors.Cross;
		}

        public override void OnMouseDown(ImageDrawBox drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);

            AddNewObject(drawArea, new DrawRectangle(pointscroll.X, pointscroll.Y, 1, 1));
        }

        public override void OnMouseMove(ImageDrawBox drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                drawArea.GraphicsList[0].MoveHandleTo(pointscroll, 5);
                drawArea.Refresh();
                drawArea.GraphicsList.Dirty = true;
            }
        }
	}
}
