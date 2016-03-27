using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using log4net;
using xview.Forms;
using xview.UserControls;
using Emgu.CV.UI;
using System.Runtime.InteropServices;
using xview.common;
using DrawTools;

namespace xview
{
    /// <summary>
    /// 图像窗口类
    /// 负责图像的展示和交互
    /// </summary>
    public partial class ImageForm : Form, xview.common.IZoomable, xview.Draw.IDrawForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ImageForm));

        private static readonly double _minZoomScale = 0.1;
        private static readonly double _maxZoomScale = 10.0;
        private static readonly double _zoomStep = 0.05;
        private Func<double, double> _funcLimitedZoom = (x => Math.Max(Math.Min(x, _maxZoomScale), _minZoomScale));

        private Image<Bgr, Byte> _backupImage;

        private Image<Bgr, Byte> originalImage;
        private Image<Gray, Byte>[] originalRGBChannels;
        private ImageProps imageProps = new ImageProps();

        //private double _zoomFactor = 1.0;

        private ImageDrawBox _imageBox;

        public Image<Bgr, Byte> Image
        {
            get { return _imageBox.Image as Image<Bgr, Byte>; }
            set { _imageBox.Image = value; }
        }

        public string FullImageFileName { get; set; }

        public ImageForm(string title, string imgFileName, Image<Bgr, Byte> img)
        {
            InitializeComponent();

            originalImage = img;
            originalRGBChannels = img.Split();//BGR

            _imageBox = new ImageDrawBox();
            _imageBox.init();
            _imageBox.Dock = DockStyle.None;
            this.Controls.Add(_imageBox);
            _imageBox.Image = originalImage.Clone();
            //_imageBox.SetZoomScale(1.0, new Point(0, 0));
            this.Text = title;
            this.FullImageFileName = imgFileName;
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public void SetImageProps(ImageConfigPara para)
        {
            try
            {
                if (para.IsColorChannel())
                {
                    SetColorChannelProp(para);
                }
                else if(para.IsGamma())
                {

                }
            }
           catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void SetColorChannelProp(ImageConfigPara para)
        {
            Image<Bgr, Byte> newImg = this.Image.Clone();
            Image<Gray, Byte>[] splitedImgsNew = newImg.Split(); //BGR

            if (para.Type == ImagePropTypeDef.BLUE_CHANNEL)
            {
                splitedImgsNew[0] = originalRGBChannels[0].Mul(para.Value);
            }
            else if (para.Type == ImagePropTypeDef.GREEN_CHANNEL)
            {
                splitedImgsNew[1] = originalRGBChannels[1].Mul(para.Value);
            }
            else if (para.Type == ImagePropTypeDef.RED_CHANNEL)
            {
                splitedImgsNew[2] = originalRGBChannels[2].Mul(para.Value);
            }
            else
            {
                logger.Error("not color channel prop");
                return;
            }
            CvInvoke.cvMerge(splitedImgsNew[0].Ptr, splitedImgsNew[1].Ptr, splitedImgsNew[2].Ptr, IntPtr.Zero, newImg.Ptr);
            _imageBox.Image = newImg;
        }

        private void SetGammaProp(ImageConfigPara para)
        {

        }

        #region 缩放
        public double GetZoomFactor()
        {
            //return _imageBox.ZoomScale;
            //return _zoomFactor;

            return _imageBox.ZoomFactor;
        }

        public void ZoomIn()
        {
            try
            {
                //double zoomInScale = _imageBox.ZoomScale;
                //zoomInScale += _zoomStep;
                //_imageBox.SetZoomScale(_funcLimitedZoom(zoomInScale), new Point(0, 0));

                //_zoomFactor += _zoomStep;
                //ZoomWindow(_zoomFactor);

                _imageBox.ZoomFactor += _zoomStep;
                ZoomWindow(_imageBox.ZoomFactor);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void ZoomOut()
        {
            try
            {
                //double zoomInScale = _imageBox.ZoomScale;
                //zoomInScale -= _zoomStep;
                //_imageBox.SetZoomScale(_funcLimitedZoom(zoomInScale), new Point(0, 0));

                //_zoomFactor -= _zoomStep;
                //ZoomWindow(_zoomFactor);

                _imageBox.ZoomFactor -= _zoomStep;
                ZoomWindow(_imageBox.ZoomFactor);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void RealSize()
        {
            try
            {
                //Size imageSize = this.Image.Size;
                //Size boxSize = _imageBox.Size;
                //double widthFactor = (double)imageSize.Width / (double)boxSize.Width;
                //double heightFactor = (double)imageSize.Height / (double)boxSize.Height;
                //double scale = Math.Max(widthFactor, heightFactor);
                //_imageBox.SetZoomScale(scale, new Point(0, 0));
                //_imageBox.HorizontalScrollBar.Value = 0;
                //_imageBox.VerticalScrollBar.Value = 0;

                //_zoomFactor = 1.0;
                //ZoomWindow(_zoomFactor);

                _imageBox.ZoomFactor = 1.0;
                ZoomWindow(_imageBox.ZoomFactor);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void FitScreen()
        {
            try
            {
                //_imageBox.SetZoomScale(1.0, new Point(0, 0));
                //_imageBox.HorizontalScrollBar.Value = 0;
                //_imageBox.VerticalScrollBar.Value = 0;

                ZoomWindow(_imageBox.ZoomFactor, true);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void ZoomWindow(double factor, bool fitScreen = false)
        {
            int workAreaWidth = this.ClientRectangle.Width;
            int workAreaHeight = this.ClientRectangle.Height;
            Size realSize = _imageBox.Image.Size;
            if (fitScreen)
            {
                double widthFactor = Convert.ToDouble(workAreaWidth) / Convert.ToDouble(realSize.Width);
                double heightFactor = Convert.ToDouble(workAreaHeight) / Convert.ToDouble(realSize.Height);
                factor = Math.Min(widthFactor, heightFactor);
            }
            else
            {
                factor = _funcLimitedZoom(factor);
            }

            //_zoomFactor = factor;
            _imageBox.ZoomFactor = factor;
            _imageBox.Width = Convert.ToInt32(realSize.Width * factor);
            _imageBox.Height = Convert.ToInt32(realSize.Height * factor);
            _imageBox.Left = (workAreaWidth > _imageBox.Width) ? (workAreaWidth - _imageBox.Width) / 2 : 0;
            _imageBox.Top = (workAreaHeight > _imageBox.Height) ? (workAreaHeight - _imageBox.Height) / 2 : 0;
        }
        #endregion

        #region 图像(待重构)
        public void Save()
        {
            try
            {
	            //此处是否需要先验证FullImageFileName是一个有效的路径名
	            if (!string.IsNullOrEmpty(FullImageFileName))
	                this.Image.Save(FullImageFileName);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void ShowHistogramForm()
        {
            try
            {
	            BackUpImage();
	            XHistogramPanel histogramPanel = new XHistogramPanel();
	            histogramPanel.ImageForm = this;
	            histogramPanel.Init(_imageBox.Image as Image<Bgr, Byte>);
	            Form form = new Form();
	            form.Text = "直方图";
	            histogramPanel.Dock = DockStyle.Fill;
	            form.Controls.Add(histogramPanel);
	            form.Size = new System.Drawing.Size(500, 300);
	            form.StartPosition = FormStartPosition.WindowsDefaultLocation;
	            form.ShowDialog();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        //关于ChildForm和XHistogramPanel交互的部分需要重构，应使用事件驱动的方式
        public void BackUpImage()
        {
            _backupImage = this.Image.Clone();
        }

        public void RecoverImage()
        {
            this.Image = _backupImage.Clone();
        }

        //被XHistiogramPanel调用，ChildForm和XHistogramPanel互相依赖不好，有待重构
        public void SetThreshold(int th, bool isCutLow)
        {
            try
            {
	            Image<Ycc, Byte> yccImg = _backupImage.Convert<Ycc, Byte>();
	            Image<Gray, Byte>[] splitedImgs = yccImg.Split();
	            Image<Gray, Byte> grayImg = splitedImgs[0];
	            Image<Gray, Byte> cbImg = splitedImgs[1];
	            Image<Gray, Byte> crImg = splitedImgs[2];
	            if (isCutLow)
	                grayImg = grayImg.ThresholdToZero(new Gray(th));
	            else
	                grayImg = grayImg.ThresholdToZeroInv(new Gray(th));
	            CvInvoke.cvMerge(grayImg.Ptr, cbImg.Ptr, crImg.Ptr, IntPtr.Zero, yccImg.Ptr);
	            _imageBox.Image = yccImg.Convert<Bgr, Byte>();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        #endregion

        #region 鼠标事件
        private void _imageBox_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void _imageBox_MouseUp(object sender, MouseEventArgs e)
        {
        }
        #endregion

        //measure
        public void SetActiveDrawTool(ImageDrawBox.DrawToolType drawToolType)
        {
            _imageBox.ActiveTool = drawToolType;
        }

        public void DeleteDrawObjects(bool deleteAll)
        {
            _imageBox.DeleteDrawObjects(deleteAll);
        }

        public void SelectAllDrawObjects()
        {
            _imageBox.SelectAllDrawObjects();
        }

        public List<MeasureListItem> GetMeasureListData()
        {
            return _imageBox.GetMeasureListData();
        }

        public List<MeasureStatisticItem> GetMeasureStatisticData()
        {
            return _imageBox.GetMeasureStatisticData();
        }

    }
}
