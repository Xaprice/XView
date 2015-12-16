using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;

namespace xview.Forms
{
    /// <summary>
    /// 暂时不用了，后面可以考虑改成鹰眼图
    /// </summary>
    public partial class SetROIForm : Form
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SetROIForm));
        private ToolTip _toolTip = new ToolTip();
        private Point _startPoint = Point.Empty;
        private Point _endPoint = Point.Empty;
        private bool _isDrawing = false;

        private Image<Bgr, Byte> _originImage;
        public Image<Bgr, Byte> Image
        {
            //get { return _imageBox.Image as Image<Bgr, Byte>; }
            set 
            {
                _originImage = value;
                _imageBox.Image = _originImage.Clone();
            }
        }

        public string FormTitle { get; set; }

        public Rectangle ROI { get; set; }

        public SetROIForm()
        {
            InitializeComponent();
            FormTitle = "ROI设置";
        }

        private void _imageBox_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
	            if (this.GetImageBoxData() == null)
	                return;
                _isDrawing = true;
	            _btnOK.Enabled = false;
	            _startPoint = Cursor.Position;
	            ROI = Rectangle.Empty;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _imageBox_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.GetImageBoxData() == null)
	                return;
	            if (_startPoint != Point.Empty)
	            {
	                _endPoint = Cursor.Position;
	                Point loc = new Point(Math.Min(_startPoint.X, _endPoint.X), Math.Min(_startPoint.Y, _endPoint.Y));
	                Size sz = new Size(Math.Abs(_endPoint.X - _startPoint.X), Math.Abs(_endPoint.Y - _startPoint.Y));
                    if (_isDrawing)
                    {
                        //Point loc = new Point(Math.Min(_startPoint.X, _endPoint.X), Math.Min(_startPoint.Y, _endPoint.Y));
                        //Size sz = new Size(Math.Abs(_endPoint.X - _startPoint.X), Math.Abs(_endPoint.Y - _startPoint.Y));
                        ControlPaint.DrawReversibleFrame(new Rectangle(loc, sz), Color.AntiqueWhite, FrameStyle.Thick);
                    }
	                Rectangle selectedRect = _imageBox.RectangleToClient(new Rectangle(loc, sz));

	                double wScale, hScale;
                    wScale = (double)(_imageBox.ClientRectangle.Width) / (double)(this.GetImageBoxData().Width);
	                hScale = (double)(_imageBox.ClientRectangle.Height)/(double)(this.GetImageBoxData().Height);
	                ROI = new Rectangle((int)(selectedRect.X / wScale), (int)(selectedRect.Y / hScale), 
	                    (int)(selectedRect.Width / wScale), (int)(selectedRect.Height / hScale));

                    ResetImageBoxData();
                    this.GetImageBoxData().Draw(ROI, new Bgr(Color.AliceBlue), 2);

	                ShowROIInfoInTitle(ROI);
                    if (sz.Width != 0 && sz.Height != 0)
                    {
                        _btnOK.Enabled = true;
                    }
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                _startPoint = Point.Empty;
                _isDrawing = false;
            }
        }

        private void _imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.GetImageBoxData() == null)
                    return;
                Point pt = _imageBox.PointToClient(Cursor.Position);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            try
            {
            	this.DialogResult = DialogResult.OK;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
            	this.DialogResult = DialogResult.Cancel;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void ShowROIInfoInTitle(Rectangle roi)
        {
            if (roi.IsEmpty)
            {
                this.Text = FormTitle;
            }
            else
            {
                this.Text = FormTitle + string.Format(": X-{0}, Y-{1}, W-{2}, H-{3}", roi.X, roi.Y, roi.Width, roi.Height);
            }
        }

        private void ShowMoveInfoInTitle(Point pt)
        {
            if (pt.IsEmpty)
            {
                this.Text = FormTitle;
            }
            else
            {
                this.Text = FormTitle + string.Format("X-{0}, Y-{1}", pt.X, pt.Y);
            }
        }

        private void ResetImageBoxData()
        {
            _imageBox.Image = _originImage.Clone();
        }

        private Image<Bgr, Byte> GetImageBoxData()
        {
            return _imageBox.Image as Image<Bgr, Byte>;
        }

        private void _btnWholeArea_Click(object sender, EventArgs e)
        {
            try
            {
	            ROI = new Rectangle(2, 2, _originImage.Width - 2, _originImage.Height - 2);
	            ResetImageBoxData();
	            this.GetImageBoxData().Draw(ROI, new Bgr(Color.AliceBlue), 2);
                ShowROIInfoInTitle(ROI);
                if (ROI.Width != 0 && ROI.Height != 0)
                {
                    _btnOK.Enabled = true;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}
