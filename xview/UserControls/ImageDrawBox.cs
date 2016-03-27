using DocToolkit;
using DrawTools;
using Emgu.CV.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xview.Draw;

namespace xview.UserControls
{
    public class ImageDrawBox : ImageBox, DocumentDirtyObserver
    {
        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Polygon,
            NumberOfDrawTools
        };

        private static readonly ILog logger = LogManager.GetLogger(typeof(ImageDrawBox));

        private GraphicsList graphicsList;    // list of draw objects, (instances of DrawObject-derived classes)

        private DrawToolType activeTool;      // active drawing tool

        private Dictionary<DrawToolType, Tool> toolDic = new Dictionary<DrawToolType, Tool>();

        //private Tool[] tools;                 // array of tools
        //private MainForm owner;
        private DocManager docManager;
        private ContextMenuStrip m_ContextMenu;

        private UndoManager undoManager;

        private bool initialized;

        public Form Owner { get; set; }

        public DrawToolType ActiveTool { get; set; }

        public GraphicsList GraphicsList
        {
            get
            {
                return graphicsList;
            }
            set
            {
                graphicsList = value;
                if (this.GraphicsList != null)
                {
                    this.AdjustRendering();
                    this.graphicsList.AddObserver(this);
                }
            }
        }

        public double ZoomFactor { get; set; }

        //--------------init
        public void init()
        {
            //set modes
            this.FunctionalMode = FunctionalModeOption.Minimum;
            this.PanableAndZoomable = false;
            this.DoubleBuffered = true;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            ActiveTool = DrawToolType.Pointer;

            // create list of graphic objects
            GraphicsList = new GraphicsList();

            // Create undo manager
            undoManager = new UndoManager(GraphicsList);

            //init drawing tools
            initDrawTools();

            //init events
            initEvents();

            this.initialized = true;
        }

        private void initDrawTools()
        {
            // create array of drawing tools
            //tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            //tools[(int)DrawToolType.Pointer] = new ToolPointer();
            //tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
            //tools[(int)DrawToolType.Ellipse] = new ToolEllipse();
            //tools[(int)DrawToolType.Line] = new ToolLine();
            //tools[(int)DrawToolType.Polygon] = new ToolPolygon();

            toolDic.Add(DrawToolType.Pointer, new ToolPointer());
            toolDic.Add(DrawToolType.Rectangle, new ToolRectangle());
            toolDic.Add(DrawToolType.Ellipse, new ToolEllipse());
            toolDic.Add(DrawToolType.Line, new ToolLine());
            toolDic.Add(DrawToolType.Polygon, new ToolPolygon());
        }

        private void initEvents()
        {
            this.MouseDown += CustomImageBox_MouseDown;
            this.MouseMove += CustomImageBox_MouseMove;
            this.MouseUp += CustomImageBox_MouseUp;
        }

        private void OnContextMenu(MouseEventArgs e)
        {
            //TODO:
        }

        //---------------Drawing

        private void CustomImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //logger.Debug(String.Format("ImageDrawBox-MouseDown-X:{0}, Y:{1}", e.X, e.Y));
                if (e.Button == MouseButtons.Left)
                    this.GetActiveDrawTool().OnMouseDown(this, e);
                else if (e.Button == MouseButtons.Right)
                    OnContextMenu(e);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void CustomImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!initialized)
                    return;
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                    this.GetActiveDrawTool().OnMouseMove(this, e);
                else
                    this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void CustomImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //logger.Debug(String.Format("ImageDrawBox-MouseUp-X:{0}, Y:{1}", e.X, e.Y));
                if (e.Button == MouseButtons.Left)
                    this.GetActiveDrawTool().OnMouseUp(this, e);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        
        public Tool GetActiveDrawTool()
        {
            Tool activeDrawTool = null;
            toolDic.TryGetValue(this.ActiveTool, out activeDrawTool);
            return activeDrawTool;
        }

        /// <summary>
        /// TODO:可能需要移除
        /// </summary>
        public void IsDirty(GraphicsList gList)
        {
            AdjustRendering();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pe)
        {
            base.OnPaint(pe);

            //Graphics g = pe.Graphics;
            //g.DrawLine(System.Drawing.Pens.Red, this.Left, this.Top, this.Right, this.Bottom);

            if (graphicsList != null)
            {
                graphicsList.Draw(pe.Graphics, this.ZoomFactor);
            }
        }

        /// <summary>
        /// TODO:可能需要移除
        /// </summary>
        void AdjustRendering()
        {
            //Size docSize;

            //if (this.GraphicsList != null)
            //{
            //    docSize = this.GraphicsList.GetSize();
            //    AutoScrollMinSize = docSize;
            //}
            //else
            //{
            //    AutoScrollMinSize = new Size(0, 0);
            //}
            //Invalidate();
        }

        public void SetDirty()
        {
            //DocManager.Dirty = true;
        }

        //DO/UNDO
        /// <summary>
        /// Add command to history.
        /// </summary>
        public void AddCommandToHistory(Command command)
        {
            undoManager.AddCommandToHistory(command);
        }

        /// <summary>
        /// Clear Undo history.
        /// </summary>
        public void ClearHistory()
        {
            undoManager.ClearHistory();
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            undoManager.Undo();
            Refresh();
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            undoManager.Redo();
            Refresh();
        }

        //------------operation
        public void DeleteDrawObjects(bool deleteAll)
        {
            if (deleteAll)
            {
                CommandDeleteAll command = new CommandDeleteAll(this.GraphicsList);
                if (this.GraphicsList.Clear())
                {
                    SetDirty();
                    Refresh();
                    AddCommandToHistory(command);
                }
            }
            else
            {
                CommandDelete command = new CommandDelete(this.GraphicsList);
                if (this.GraphicsList.DeleteSelection())
                {
                    SetDirty();
                    Refresh();
                    AddCommandToHistory(command);
                }
            }
        }

        public void SelectAllDrawObjects()
        {
            this.GraphicsList.SelectAll();
            Refresh();
        }

        //--------------statistic
        public List<MeasureListItem> GetMeasureListData()
        {
            List<MeasureListItem> list = new List<MeasureListItem>();
            for (int i = 0; i < this.GraphicsList.Count; i++)
            {
                DrawObject drawObj = this.GraphicsList[i];
                MeasureListItem item = drawObj.GetMeasureListItem();
                if (item != null)
                {
                    item.Unit = "像素";//暂时单位都是像素
                    list.Add(item);
                }
            }
            return list;
        }

        public List<MeasureStatisticItem> GetMeasureStatisticData()
        {
            List<MeasureStatisticItem> list = new List<MeasureStatisticItem>();

            List<MeasureListItem> measureList = this.GetMeasureListData();
            List<MeasureListItem> lineList = new List<MeasureListItem>();
            List<MeasureListItem> rectangleList = new List<MeasureListItem>();
            List<MeasureListItem> polylineList = new List<MeasureListItem>();
            List<MeasureListItem> ellipseList = new List<MeasureListItem>();
            foreach (var listItem in measureList)
            {
                if (listItem.ToolType == "直线")
                {
                    lineList.Add(listItem);
                }
                if (listItem.ToolType == "曲折线")
                {
                    polylineList.Add(listItem);
                }
                if (listItem.ToolType == "矩形")
                {
                    rectangleList.Add(listItem);
                }
                if (listItem.ToolType == "椭圆")
                {
                    ellipseList.Add(listItem);
                }
            }
            string unit = "像素";
            if(lineList.Count != 0)
            {
                MeasureStatisticItem lineLenItem = new MeasureStatisticItem();
                lineLenItem.ToolType = "直线";
                lineLenItem.StatisticType = MeasureStatisticTypeDef.LENGTH;
                lineLenItem.Count = lineList.Count;
                lineLenItem.AverageValue = lineList.Sum(x => x.Length) / lineList.Count;
                lineLenItem.MinValue = lineList.Min(x => x.Length);
                lineLenItem.MaxValue = lineList.Max(x => x.Length);
                lineLenItem.Unit = unit;
                list.Add(lineLenItem);
            }

            if (rectangleList.Count != 0)
            {
                MeasureStatisticItem rectAreaItem = new MeasureStatisticItem();
                rectAreaItem.ToolType = "矩形";
                rectAreaItem.StatisticType = MeasureStatisticTypeDef.AREA;
                rectAreaItem.Count = rectangleList.Count;
                rectAreaItem.AverageValue = rectangleList.Sum(x => x.Area) / rectangleList.Count;
                rectAreaItem.MinValue = rectangleList.Min(x => x.Area);
                rectAreaItem.MaxValue = rectangleList.Max(x => x.Area);
                rectAreaItem.Unit = unit;
                list.Add(rectAreaItem);

                MeasureStatisticItem rectPerimeterItem = new MeasureStatisticItem();
                rectPerimeterItem.StatisticType = MeasureStatisticTypeDef.PERIMETER;
                rectPerimeterItem.Count = rectangleList.Count;
                rectPerimeterItem.AverageValue = rectangleList.Sum(x => x.Perimeter) / rectangleList.Count;
                rectPerimeterItem.MinValue = rectangleList.Min(x => x.Perimeter);
                rectPerimeterItem.MaxValue = rectangleList.Max(x => x.Perimeter);
                rectPerimeterItem.Unit = unit;
                list.Add(rectPerimeterItem);
            }

            if (polylineList.Count != 0)
            {
                MeasureStatisticItem polyLenItem = new MeasureStatisticItem();
                polyLenItem.ToolType = "曲折线";
                polyLenItem.StatisticType = MeasureStatisticTypeDef.AREA;
                polyLenItem.Count = polylineList.Count;
                polyLenItem.AverageValue = polylineList.Sum(x => x.Length) / polylineList.Count;
                polyLenItem.MinValue = polylineList.Min(x => x.Length);
                polyLenItem.MaxValue = polylineList.Max(x => x.Length);
                polyLenItem.Unit = unit;
                list.Add(polyLenItem);
            }

            if (ellipseList.Count != 0)
            {
                MeasureStatisticItem ellipseAreaItem = new MeasureStatisticItem();
                ellipseAreaItem.ToolType = "椭圆";
                ellipseAreaItem.StatisticType = MeasureStatisticTypeDef.AREA;
                ellipseAreaItem.Count = ellipseList.Count;
                ellipseAreaItem.AverageValue = ellipseList.Sum(x => x.Area) / ellipseList.Count;
                ellipseAreaItem.MinValue = ellipseList.Min(x => x.Area);
                ellipseAreaItem.MaxValue = ellipseList.Max(x => x.Area);
                ellipseAreaItem.Unit = unit;
                list.Add(ellipseAreaItem);
                MeasureStatisticItem ellipsePerimeterItem = new MeasureStatisticItem();
                ellipsePerimeterItem.StatisticType = MeasureStatisticTypeDef.PERIMETER;
                ellipsePerimeterItem.Count = ellipseList.Count;
                ellipsePerimeterItem.AverageValue = ellipseList.Sum(x => x.Perimeter) / ellipseList.Count;
                ellipsePerimeterItem.MinValue = ellipseList.Min(x => x.Perimeter);
                ellipsePerimeterItem.MaxValue = ellipseList.Max(x => x.Perimeter);
                ellipsePerimeterItem.Unit = unit;
                list.Add(ellipsePerimeterItem);
            }
            return list;
        } 

        

    }
}
