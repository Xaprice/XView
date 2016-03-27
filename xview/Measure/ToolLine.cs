using System;
using System.Windows.Forms;
using System.Drawing;
using xview.UserControls;

namespace DrawTools
{
	/// <summary>
	/// Line tool
	/// </summary>
	class ToolLine : DrawTools.ToolObject
	{
        public ToolLine()
        {
            //Cursor = new Cursor(GetType(), "Line.cur");
            Cursor = Cursors.Cross;
        }

        public override void OnMouseDown(ImageDrawBox drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);
            AddNewObject(drawArea, new DrawLine(pointscroll.X, pointscroll.Y, pointscroll.X + 1, pointscroll.Y + 1));
        }

        public override void OnMouseMove(ImageDrawBox drawArea, MouseEventArgs e)
        {
            Point pointscroll = GetEventPointInArea(drawArea, e);
            drawArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                drawArea.GraphicsList[0].MoveHandleTo(pointscroll, 2);
                drawArea.Refresh();
                drawArea.GraphicsList.Dirty = true;
            }
        }
    }
}
