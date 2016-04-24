using System;
using System.Windows.Forms;
using System.Drawing;
using xview.UserControls;
using xview.Forms;


namespace DrawTools
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>
	abstract class ToolObject : DrawTools.Tool
	{
        private Cursor cursor;

        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }


        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(ImageDrawBox drawArea, MouseEventArgs e)
        {
            drawArea.GraphicsList[0].Normalize();
            drawArea.AddCommandToHistory(new CommandAdd(drawArea.GraphicsList[0]));
            drawArea.ActiveTool = ImageDrawBox.DrawToolType.Pointer;

            //zhoujin: 如果是正在定标则删除图形，并弹出定标窗口
            if (drawArea.DrawMode == ImageDrawBox.DrawingMode.SetUnit)
            {
                DrawObject drawObj = drawArea.GraphicsList[0];
                if (drawObj is DrawLine)
                {
                    DrawLine drawLine = drawObj as DrawLine;
                    double pxLen = CalcLenght(drawLine.StartPoint, drawLine.EndPoint);

                    SetUnitForm setUnitForm = new SetUnitForm(pxLen, drawArea);
                    setUnitForm.ShowDialog();

                    drawArea.GraphicsList.DeleteLastAddedObject();
                }
                else
                {

                }
            }

            drawArea.Capture = false;
            drawArea.Refresh();
            drawArea.GraphicsList.Dirty = true;
        }

        private double CalcLenght(Point p1, Point p2)
        {
            double length = 0;
            double w = Math.Abs(p1.X - p2.X);
            double h = Math.Abs(p1.Y - p1.Y);
            length = Math.Sqrt(w * w + h * w);
            return length;
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="o"></param>
        protected void AddNewObject(ImageDrawBox drawArea, DrawObject o)
        {
            drawArea.GraphicsList.UnselectAll();

            o.Selected = true;
            drawArea.GraphicsList.Add(o);

            drawArea.Capture = true;
            drawArea.Refresh();

            drawArea.SetDirty();
        }
	}
}
