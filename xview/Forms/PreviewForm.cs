using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using log4net;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Timers;
using xview.common;
using xview.utils;
using xview.UserControls;
using DrawTools;
using xview.Measure;

namespace xview.Forms
{

    //TODO: PreviewForm与ImageForm需要合并
    public partial class PreviewForm : Form, xview.common.IZoomable, xview.Draw.IDrawForm
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PreviewForm));

        private static int frameWidth = 0;
        private static int frameHeight = 0;

        /// <summary>
        /// 采集状态
        /// </summary>
        private static CaptureState _captureState = CaptureState.NO_CAPTURE;

        /// <summary>
        /// 视频帧计数值
        /// </summary>
        private static int _frameCnt = 0;

        private System.Timers.Timer _timerMutiCapture = new System.Timers.Timer();

        /// <summary>
        /// 是否标记过曝光区域
        /// </summary>
        private static bool _markOverExposureArea = false;

        private XCamera.DelegateProc _cameraCallbackProc;

        /// <summary>
        /// 待采集图像
        /// </summary>
        private static Image<Bgr, Int32> _captureImage;

        /// <summary>
        /// 采集帧计数值
        /// </summary>
        private static int _captureCnt = 0;

        private static bool _isCustomDrawing = false;
        private Rectangle _mouseRect = Rectangle.Empty;

        //private double _zoomFactor = 1.0;
        private static readonly double _minZoomScale = 0.1;
        private static readonly double _maxZoomScale = 10.0;
        private static readonly double _zoomStep = 0.05;
        private Func<double, double> _funcLimitedZoom = (x => Math.Max(Math.Min(x, _maxZoomScale), _minZoomScale));

        public MainForm mainForm = null;

        /*******************************************************************************
         * 拍照/录像
         *******************************************************************************/
        /// <summary>
        /// 拍照
        /// </summary>
        public void StartSingleCapture()
        {
            try
            {
                Snapshot();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                _captureState = CaptureState.NO_CAPTURE;
            }
        }

        /// <summary>
        /// 设置连拍定时器
        /// </summary>
        public  void StartMutiCap()
        {
            _captureCnt = 0;
            _timerMutiCapture.Interval = XCamera.GetInstance().CapturePara.MutiCaptureTimeStep;
            _timerMutiCapture.Elapsed += _timerMutiCapture_Tick;
            _timerMutiCapture.Start();
        }

        /// <summary>
        /// 停止连拍计时器
        /// </summary>
        public void StopMutiCap()
        {
            _timerMutiCapture.Stop();
            _timerMutiCapture.Elapsed -= _timerMutiCapture_Tick;
            _captureCnt = 0;
        }
     
        /// <summary>
        /// 定时器处理方法
        /// </summary>
        private void _timerMutiCapture_Tick(object sender, ElapsedEventArgs e)
        {
            //if (_captureState == CaptureState.MULT_CAPTURE_TRIGGER_OFF)
            //{
            //    _captureState = CaptureState.MULT_CAPTURE_TRIGGER_ON;
            //}
            try
            {
                if (_captureCnt < XCamera.GetInstance().CapturePara.MutiCaptureCount)
                {
                    Snapshot();
                    _captureCnt++;
                }
                else
                {
                    StopMutiCap();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void Snapshot()
        {
            string imgFileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-sss");
            string fullFileName = XCamera.GetInstance().CaptureImage(imgFileName);
            if (!String.IsNullOrEmpty(fullFileName))
            {
                XTwoExtraValueEventArgs<string, string> eventArgs = new XTwoExtraValueEventArgs<string, string>()
                {
                    Data1 = fullFileName,
                    Data2 = imgFileName
                };
                PublishImageSavedEvent(eventArgs);
            }
        }

        #region 接口
        public bool IsCloseWithMainForm { get; set; }

        public SettingROI SettingROIType { get; set; }
                
        /// <summary>
        /// 开始预览视频（同时只能打开一个相机）
        /// </summary>
        /// <param name="cameraName"></param>
        public bool StartPreview(string cameraName)
        {
            try
            {
                bool retVal = false;
                XCamera cam = XCamera.GetInstance();
                if (cam.IsActive())
                {
                    if (cam.RunMode != emDSRunMode.RUNMODE_STOP)
                    {
                        cam.Stop();
                    }
                    cam.UnInit();
                }
                _cameraCallbackProc = new XCamera.DelegateProc(SnapThreadCallback);

                //test

                if (cam.Init(_cameraCallbackProc, cameraName, _imageBox.Handle))
                //if (cam.Init(_cameraCallbackProc, cameraName, _imageBack.Handle))
                {
                    if (cam.Play())
                    {
                        retVal = true;
                        //this.RealSize();
                        this.FitScreen();
                        PublishCameraOpenedEvent(new EventArgs());
                    }
                }
                else
                {
                    retVal = false;
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SwitchPreviewResolution(int resolutionSel)
        {
            try
            {
                XCamera cam = XCamera.GetInstance();
                cam.SetPreviewSizeSel(resolutionSel);
                //cam.Play();
                //this.RealSize();
                this.FitScreen();
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public static void SetCaptureState(CaptureState state)
        {
            _captureState = state;
        }

        public static CaptureState GetCaptureState()
        {
            return _captureState;
        }

        public static void SetMarkOverExpFlag(bool flag)
        {
            _markOverExposureArea = flag;
        }

        public Rectangle GetCursorRect()
        {
            return _imageBox.RectangleToScreen(_imageBox.DisplayRectangle);
        }
        #endregion

        #region 构造方法

        private ImageDrawBox _imageBox;

        private PictureBox _imageBack;

        //private CustomPictureBox pictureBox;

        public PreviewForm()
        {
            InitializeComponent();

            //test code
            _imageBox = new ImageDrawBox();
            _imageBox.DrawForm = this;
            _imageBox.init();
            _imageBox.Dock = DockStyle.None;
            //_imageBox.BackColor = Color.Transparent;
            this.Controls.Add(_imageBox);
            
            //test
            //_imageBack = new PictureBox();
            ////_imageBack.init();
            //_imageBack.Dock = DockStyle.None;
            //this.Controls.Add(_imageBack);

            //_imageBox.BringToFront();

            //test code-尝试将imageBox设置为_imageBack
            PreviewForm.imageBox = _imageBox;
            //PreviewForm.imageBox = _imageBack;
        }
        #endregion

        #region 事件
        public delegate void CameraOpenedHandler(object sender, EventArgs e);
        public event CameraOpenedHandler CameraOpened;
        private void PublishCameraOpenedEvent(EventArgs e)
        {
            if (CameraOpened != null)
            {
                CameraOpened(this, e);
            }
        }

        public delegate void CameraClosedHandler(object sender, EventArgs e);
        public event CameraClosedHandler CameraClosed;
        protected virtual void OnCameraClosed(EventArgs e)
        {
            if (CameraClosed != null)
            {
                CameraClosed(this, e);
            }
        }

        public delegate void VideoCaptureStartedHandler(EventArgs e);
        public static event VideoCaptureStartedHandler VideoCaptureStartedEvent;
        private static void PublishVideoCaptureStartedEvent(EventArgs e)
        {
            if (VideoCaptureStartedEvent != null)
            {
                VideoCaptureStartedEvent(e);
            }
        }

        public delegate void VideoCaptureStoppedHandler(EventArgs e);
        public static event VideoCaptureStoppedHandler VideoCaptureStoppedEvent;
        private static void PublishVideoCaptureStoppedEvent(EventArgs e)
        {
            if (VideoCaptureStoppedEvent != null)
            {
                VideoCaptureStoppedEvent(e);
            }
        }

        public delegate void AEROISettedEventHandler(object sender, EventArgs e);
        public event AEROISettedEventHandler AEWindowSettedEvent;
        private void PublishAEROISettedEvent(EventArgs e)
        {
            if (AEWindowSettedEvent != null)
            {
                AEWindowSettedEvent(this, e);
            }
        }

        public delegate void ImageSavedEventHandler(XTwoExtraValueEventArgs<string, string> e);
        public event ImageSavedEventHandler ImageSavedEvent;
        private void PublishImageSavedEvent(XTwoExtraValueEventArgs<string, string> e)
        {
            if (ImageSavedEvent != null)
            {
                ImageSavedEvent(e);
            }
        }
        #endregion

        #region 回调
        private static Emgu.CV.UI.ImageBox imageBox = null;

        /// <summary>
        /// 相机回调函数
        /// </summary>
        /// <param name="m_iCam"></param>
        /// <param name="pbyBuffer"></param>
        /// <param name="sFrInfo"></param>
        /// <returns></returns>
        private static int SnapThreadCallback(int m_iCam, ref Byte pbyBuffer, ref tDSFrameInfo sFrInfo)
        {
            try
            {
                int pBmp24 = XCamera.CameraISP(m_iCam, ref pbyBuffer, ref sFrInfo);
                //--------

                int width = Convert.ToInt32(sFrInfo.uiWidth);
                int height = Convert.ToInt32(sFrInfo.uiHeight);

                frameWidth = width;
                frameHeight = height;

                int stride = width * 3;

                Image<Bgr, Byte>  _frameImage = new Image<Bgr, Byte>(width, height, stride, (IntPtr)(pBmp24));              
                HandleCapture(_frameImage, pBmp24, ref sFrInfo);
                MarkFrame(_frameImage);

                //---------
                if (!_isCustomDrawing)
                {
                    //test code 原先刷新帧图像的方法
                    emDSCameraStatus status = XCamera.CameraDisplayRGB24(m_iCam, pBmp24, ref sFrInfo);
                    //尝试新的方法
                    //imageBox.Image = _frameImage;
                }
                else
                {
                    imageBox.Image = _frameImage;
                }
                    
                return 0;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return -1;
            }
        }

        private static void MarkFrame(Image<Bgr, Byte> img)
        {
            if (_markOverExposureArea)
            {
                MarkOverExposureFlag(img);
            }
        }

        private static void MarkOverExposureFlag(Image<Bgr, Byte> img)
        {
            try
            {
                Image<Gray, Byte> grayImg = img.Convert<Gray, Byte>();
                grayImg = grayImg.Sub(new Gray(230));
                img.SetValue(new Bgr(0, 0, 255), grayImg);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 帧计数
        /// <summary>
        /// 视频帧计数值清零
        /// </summary>
        private static void ResetFrameCnt()
        {
            _frameCnt = 0;
        }

        private static void CountFrame()
        {
            if (_frameCnt == int.MaxValue)
            {
                _frameCnt = 0;
                _logger.Warn("视频帧计数值溢出，已经重新计数");
            }
            else
                _frameCnt++;
        }
        #endregion

        #region 定时器

        #endregion

        #region 相机采集
        private static void HandleCapture(Image<Bgr, Byte> img, int pBmp24, ref tDSFrameInfo sFrInfo)
        {
            if (_captureState == CaptureState.NO_CAPTURE)
                return;
            XCamera cam = XCamera.GetInstance();
            //HandleSingleCapture(img, cam);
            //HandleMutiCapture(img, cam);
            HandleFluCapture(img, cam);
            HandleVideoCapture(pBmp24, ref sFrInfo);
        }

        //private static void HandleMutiCapture(Image<Bgr, Byte> img, XCamera cam)
        //{
        //    try
        //    {
        //        if (_captureState == CaptureState.MULT_CAPTURE_TRIGGER_ON)
        //        {
        //            if (_captureCnt < cam.CapturePara.MutiCaptureCount)
        //            {
        //                SaveImageToDefaultPath(img, cam);
        //                _captureState = CaptureState.MULT_CAPTURE_TRIGGER_OFF;
        //                _captureCnt++;
        //            }
        //            else
        //            {
        //                //_timerMutiCapture.Enabled = false;
        //                _timerMutiCapture.Stop();
        //                _captureState = CaptureState.NO_CAPTURE;
        //                _captureCnt = 0;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.Error(ex.Message);
        //    }
        //}

        private static void HandleFluCapture(Image<Bgr, Byte> img, XCamera cam)
        {
            try
            {
                if (_captureState == CaptureState.FLU_START_CAPTURE)
                {
                    WaitForm.GetInstance().Show();
                    _captureImage = new Image<Bgr, int>(img.Cols, img.Rows);
                    for (int row = 0; row < _captureImage.Rows; row++)
                    {
                        for (int col = 0; col < _captureImage.Cols; col++)
                        {
                            _captureImage.Data[row, col, 0] = img.Data[row, col, 0];
                            _captureImage.Data[row, col, 1] = img.Data[row, col, 1];
                            _captureImage.Data[row, col, 2] = img.Data[row, col, 2];
                        }
                    }

                    if (cam.CapturePara.FluModeAccuFrameCnt == 1)
                        _captureState = CaptureState.FLU_STOP_CAPTURE;
                    else
                        _captureState = CaptureState.FLU_CAPTURING;
                    _captureCnt++;

                    WaitForm.GetInstance().Description = "进度：1/" + cam.CapturePara.FluModeAccuFrameCnt.ToString();
                }
                if (_captureState == CaptureState.FLU_CAPTURING)
                {
                    if (_frameCnt % cam.CapturePara.FluModeFrameStep != 0)
                        return;
                    if (_captureCnt <= cam.CapturePara.FluModeAccuFrameCnt)
                    {
                        for (int row = 0; row < _captureImage.Rows; row++)
                        {
                            for (int col = 0; col < _captureImage.Cols; col++)
                            {
                                _captureImage.Data[row, col, 0] += img.Data[row, col, 0];
                                _captureImage.Data[row, col, 1] += img.Data[row, col, 1];
                                _captureImage.Data[row, col, 2] += img.Data[row, col, 2];
                            }
                        }
                        _captureCnt++;
                        WaitForm.GetInstance().Description = "进度：" + _captureCnt.ToString() + " /" + cam.CapturePara.FluModeAccuFrameCnt.ToString();
                    }
                    else
                    {
                        _captureState = CaptureState.FLU_STOP_CAPTURE;
                        _captureCnt = 0;
                    }
                }
                if (_captureState == CaptureState.FLU_STOP_CAPTURE)
                {
                    WaitForm.GetInstance().Description = "保存中...";
                    _captureImage = _captureImage.Mul(1.0 / cam.CapturePara.FluModeAccuFrameCnt);
                    Image<Bgr, Byte> img2Save = _captureImage.Convert<Bgr, Byte>();
                    string fullFileName = SaveImageToDefaultPath(img2Save, cam);
                    _captureState = CaptureState.NO_CAPTURE;
                    WaitForm.GetInstance().Description = "";
                    WaitForm.GetInstance().Close();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                _captureState = CaptureState.NO_CAPTURE;
                WaitForm.GetInstance().Description = "";
                WaitForm.GetInstance().Close();
            }
        }

        private static void HandleVideoCapture(int pBmp24, ref tDSFrameInfo sFrInfo)
        {
            try
            {
                if (_captureState == CaptureState.VIDEO_CAPTURING)
                {
                    if (!XCamera.GetInstance().RecordFrame(pBmp24, ref sFrInfo))
                    {
                        StopVideoCapture();
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public static void StartFluCapture()
        {
            _captureState = CaptureState.FLU_START_CAPTURE;
        }

        //public static void StartMutiCapture()
        //{
        //    _timerMutiCapture.Interval = XCamera.GetInstance().CapturePara.MutiCaptureTimeStep;
        //    _timerMutiCapture.Elapsed += _timerMutiCapture_Tick;
        //    //_timerMutiCapture.AutoReset = false;
        //    _captureState = CaptureState.MULT_CAPTURE_TRIGGER_ON;
        //    //_timerMutiCapture.Enabled = true;
        //    _timerMutiCapture.Start();
        //}

        //public static void StopMutiCapture()
        //{
        //    //_timerMutiCapture.Enabled = false;
        //    _timerMutiCapture.Stop();
        //    _captureState = CaptureState.MULT_CAPTURE_TRIGGER_OFF;
        //}

        public static void StartVideoCapture()
        {
            try
            {
                if (XCamera.GetInstance().StartRecord())
                {
                    _captureState = CaptureState.VIDEO_CAPTURING;
                    PublishVideoCaptureStartedEvent(new EventArgs());
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public static void StopVideoCapture()
        {
            try
            {
                if (_captureState == CaptureState.VIDEO_CAPTURING)
                {
                    if (XCamera.GetInstance().StopRecord())
                    {
                        _captureState = CaptureState.NO_CAPTURE;
                        PublishVideoCaptureStoppedEvent(new EventArgs());
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 保存图像到默认的路径下
        /// </summary>
        /// <param name="img2Save">待保存图像</param>
        /// <param name="cam">相机实例</param>
        /// <returns>保存的图像名（包含完整路径）</returns>
        private static string SaveImageToDefaultPath(Image<Bgr, Byte> img2Save, XCamera cam)
        {
            try
            {
                string imgFileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-sss");
                //if (cam.CapturePara.ImageFileType == emDSFileType.FILE_JPG)
                //    imgFileName += ".jpg";
                //else if (cam.CapturePara.ImageFileType == emDSFileType.FILE_BMP)
                //    imgFileName += ".bmp";
                //else
                //    imgFileName += ".png";
                //string fullFileName = ConfigManager.GetAppConfig("WorkPathImage") + "\\" + imgFileName;
                string fullFileName = XCamera.GetInstance().CapturePara.ImageSavePath + "\\" + imgFileName;
                img2Save.Save(fullFileName);
                XTwoExtraValueEventArgs<string, string> eventArgs = new XTwoExtraValueEventArgs<string, string>()
                {
                    Data1 = fullFileName,
                    Data2 = imgFileName
                };
                //PublishImageSavedEvent(eventArgs);
                return fullFileName;
            }
            catch (System.Exception ex)
            {
                _logger.Error("保存图像时异常：" + ex.Message);
                return "";
            }
        }
        #endregion

        #region 缩放
        public double GetZoomFactor()
        {
            //return _zoomFactor;

            return _imageBox.ZoomFactor;
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void ZoomIn()
        {
            try
            {
                //_zoomFactor += _zoomStep;
                //ZoomWindow(_zoomFactor);

                _imageBox.ZoomFactor += _zoomStep;
                ZoomWindow(_imageBox.ZoomFactor);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void ZoomOut()
        {
            try
            {
                //_zoomFactor-=_zoomStep;
                //ZoomWindow(_zoomFactor);

                _imageBox.ZoomFactor -= _zoomStep;
                ZoomWindow(_imageBox.ZoomFactor);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 实际大小
        /// </summary>
        public void RealSize()
        {
            try
            {
                //_zoomFactor = 1.0;
                //ZoomWindow(_zoomFactor);

                _imageBox.ZoomFactor = 1.0;
                ZoomWindow(_imageBox.ZoomFactor);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 适合屏幕
        /// </summary>
        public void FitScreen()
        {
            try
            {
                //ZoomWindow(_zoomFactor, true);
                ZoomWindow(_imageBox.ZoomFactor, true);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void ZoomWindow(double factor, bool fitScreen = false)
        {
            int workAreaWidth = this.ClientRectangle.Width;
            int workAreaHeight = this.ClientRectangle.Height;
            int previewWidth, previewHeight;
            if(!XCamera.GetInstance().GetPreviewSize(out previewWidth, out previewHeight))
            {
                return;
            }
            Size realSize = new Size(previewWidth, previewHeight);
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
            XCamera.GetInstance().SetDisplaySize(_imageBox.Width, _imageBox.Height);

            SyncImageBack();
        }

        private void SyncImageBack()
        {
            ////_imageBack.ZoomFactor = _imageBox.ZoomFactor;
            //_imageBack.Width = _imageBox.Width;
            //_imageBack.Height = _imageBox.Height;
            //_imageBack.Left = _imageBox.Left;
            //_imageBack.Top = _imageBox.Top;

        }

        //private Size GetPreviewWindowRealSize()
        //{
        //    tDSImageSize tmpsize = new tDSImageSize();
        //    int m_iRelsel = 0;
        //    int nWidth = 640, nHeight = 480;
        //    tDSCameraCapability dscapability = new tDSCameraCapability();
        //    XCamera.CameraGetCapability(1, ref dscapability);
        //    if (XCamera.CameraGetImageSizeSel(1, ref m_iRelsel, false) == emDSCameraStatus.STATUS_OK)
        //    {
        //        Byte[] arrtmp = new Byte[4];
        //        XCamera.CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(arrtmp, 0), dscapability.pImageSizeDesc + m_iRelsel * Marshal.SizeOf(tmpsize) + 52, 4);
        //        nWidth = BitConverter.ToInt32(arrtmp, 0);
        //        XCamera.CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(arrtmp, 0), dscapability.pImageSizeDesc + m_iRelsel * Marshal.SizeOf(tmpsize) + 56, 4);
        //        nHeight = BitConverter.ToInt32(arrtmp, 0);
        //    }
        //    return new Size(nWidth, nHeight);
        //}
        #endregion

        //private void UpdateDisplayWindow()
        //{
        //    tDSImageSize tmpsize = new tDSImageSize();
        //    int m_iRelsel = 0;
        //    int nWidth = 640, nHeight = 480;
        //    tDSCameraCapability dscapability = new tDSCameraCapability();
        //    XCamera.CameraGetCapability(1, ref dscapability);
        //    if (XCamera.CameraGetImageSizeSel(1, ref m_iRelsel, false) == emDSCameraStatus.STATUS_OK)
        //    {
        //        Byte[] arrtmp = new Byte[4];
        //        XCamera.CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(arrtmp, 0), dscapability.pImageSizeDesc + m_iRelsel * Marshal.SizeOf(tmpsize) + 52, 4);
        //        nWidth = BitConverter.ToInt32(arrtmp, 0);
        //        XCamera.CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(arrtmp, 0), dscapability.pImageSizeDesc + m_iRelsel * Marshal.SizeOf(tmpsize) + 56, 4);
        //        nHeight = BitConverter.ToInt32(arrtmp, 0);
        //    }

        //    pictureBox.Width = nWidth;
        //    //pictureBox.Height = nHeight - 40;
        //    pictureBox.Height = nHeight;
        //    pictureBox.Left = 0;
        //    //pictureBox.Top = 24;
        //    pictureBox.Top = 0;
        //    //---------------------------------------------------------------------------------------------------------------------------------------
        //    XCamera.CameraSetDisplaySize(1, pictureBox.Width, pictureBox.Height);
        //}

        private void PreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!IsCloseWithMainForm)
                {
                    XCamera cam = XCamera.GetInstance();
                    bool isSuccessToStopCam = false;
                    if (cam.IsActive())
                    {
                        if (cam.RunMode != emDSRunMode.RUNMODE_STOP)
                        {
                            isSuccessToStopCam = cam.Stop();
                            System.Threading.Thread.Sleep(100);
                        }
                        if (isSuccessToStopCam && cam.UnInit())
                        {
                            System.Threading.Thread.Sleep(100);
                            OnCameraClosed(new EventArgs());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void PreviewForm_Load(object sender, EventArgs e)
        {
            try
            {
                //Application.AddMessageFilter(this);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        //ROI设置


        #region 鼠标事件
        //private ToolTip _toolTip = new ToolTip();
        //private Point _startPoint = Point.Empty;
        //private Point _movePoint = Point.Empty;
        //private Point _endPoint = Point.Empty;
       // private bool _isDrawing = false;

        //private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        if (!IsSettingROI())
        //            return;
        //        if (_startPoint != Point.Empty)
        //        {
        //            int vScroll = this.VerticalScroll.Value;
        //            int hScroll = this.HorizontalScroll.Value;
        //            double scale = _zoomFactor;

        //            _endPoint = Cursor.Position;
        //            Point loc = new Point(Math.Min(_startPoint.X, _endPoint.X), Math.Min(_startPoint.Y, _endPoint.Y));
        //            Size sz = new Size(Math.Abs(_endPoint.X - _startPoint.X), Math.Abs(_endPoint.Y - _startPoint.Y));
        //            if (_isDrawing)
        //            {
        //                ControlPaint.DrawReversibleFrame(new Rectangle(loc, sz), Color.AntiqueWhite, FrameStyle.Thick);
        //            }
        //            loc.X += hScroll;
        //            loc.Y += vScroll;
        //            loc.X = Convert.ToInt32(loc.X / scale);
        //            loc.Y = Convert.ToInt32(loc.Y / scale);
        //            sz.Width = Convert.ToInt32(sz.Width / scale);
        //            sz.Height = Convert.ToInt32(sz.Height / scale);

        //            Rectangle roi0 = pictureBox.RectangleToClient(new Rectangle(loc, sz));
        //            Rectangle roi = ConvertToLeftButtomStartROI(roi0);

        //            if (SettingROIType == SettingROI.AE_ROI)
        //            {
        //                //XCamera.GetInstance().SetAeState(false);
        //                System.Threading.Thread.Sleep(20);
        //                if (XCamera.GetInstance().SetAEWindow(roi.X, roi.Y, roi.Width, roi.Height))
        //                {
        //                    //bool aeState;
        //                    //XCamera.GetInstance().GetAeState(out aeState);
        //                    _logger.Debug(string.Format("设置自动曝光窗口成功！roi: X: {0}, Y: {1}, W: {2}, H: {3}, scale: {4}", roi.X, roi.Y, roi.Width, roi.Height, scale));
        //                    //_logger.Debug("aeState: " + aeState.ToString());
        //                    //XCamera.GetInstance().SetAeState(true);
        //                }
        //            }
        //            else if (SettingROIType == SettingROI.WB_ROI)
        //            {
        //                XCamera.GetInstance().SetWBWindow(roi.X, roi.Y, roi.Width, roi.Height);
        //                XCamera.GetInstance().SetOnceWB();
        //            }
        //            else if (SettingROIType == SettingROI.PVW_ROI)
        //            {
        //                XCamera.GetInstance().SetPreviewROI(roi.X, roi.Y, roi.Width, roi.Height);
        //                XCamera.GetInstance().Play();
        //            }

        //            _isCustomDrawing = false;
        //            _isDrawing = false;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.Error(ex.Message);
        //    }
        //    finally
        //    {
        //        Cursor.Clip = Rectangle.Empty;//RectangleToScreen(DisplayRectangle);
        //        this.Cursor = Cursors.Default;
        //        SettingROIType = SettingROI.NO_ROI;
        //    }
        //}

        //private Rectangle ConvertToLeftButtomStartROI(Rectangle roi)
        //{
        //    Rectangle newRoi =  new Rectangle()
        //    {
        //         X=roi.X, Y = (frameHeight-roi.Y-roi.Height), Width = roi.Width, Height=roi.Height
        //    };
        //    if (newRoi.X < 0)
        //        newRoi.X = 0;
        //    if (newRoi.Y < 0)
        //        newRoi.Y = 0;
        //    if(newRoi.X + newRoi.Width > frameWidth)
        //    {
        //        newRoi.Width = frameWidth - newRoi.X - 1;
        //    }
        //    if(newRoi.Y + newRoi.Height > frameHeight)
        //    {
        //        newRoi.Height = frameHeight - newRoi.Y - 1;
        //    }
        //    return newRoi;
        //}

        //private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        if (IsSettingROI())
        //        {
        //            _isCustomDrawing = true;
        //            _isDrawing = true;
        //            _startPoint = Cursor.Position;
        //            _mouseRect = new Rectangle(_startPoint.X, _startPoint.Y, 0, 0);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.Error(ex.Message);
        //    }
        //}

        //private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        if (IsSettingROI() && _isDrawing)
        //        {
        //            ControlPaint.DrawReversibleFrame(_mouseRect, Color.AntiqueWhite, FrameStyle.Thick);
        //            _movePoint = Cursor.Position;
        //            Point loc = new Point(Math.Min(_startPoint.X, _movePoint.X), Math.Min(_startPoint.Y, _movePoint.Y));
        //            Size sz = new Size(Math.Abs(_movePoint.X - _startPoint.X), Math.Abs(_movePoint.Y - _startPoint.Y));
        //            _mouseRect = new Rectangle(loc, sz);
        //            ControlPaint.DrawReversibleFrame(_mouseRect, Color.AntiqueWhite, FrameStyle.Thick);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.Error(ex.Message);
        //    }
        //}

        private bool IsSettingROI()
        {
            return !(SettingROIType == SettingROI.NO_ROI);
        }
        #endregion


        //measure

        public void SetDrawingMode(xview.UserControls.ImageDrawBox.DrawingMode drawingMode)
        {
            _imageBox.DrawMode = drawingMode;
        }

        public void SetActiveDrawTool(ImageDrawBox.DrawToolType drawToolType)
        {
            PreviewForm._isCustomDrawing = true;
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
            return _imageBox.GetMeasureListData(this.measureScale);
        }

        public List<MeasureStatisticItem> GetMeasureStatisticData()
        {
            return _imageBox.GetMeasureStatisticData(this.measureScale);
        }

        private MeasureScale measureScale = null;
        public void SetScale(MeasureScale scale)
        {
            this.measureScale = scale;
            if (mainForm != null)
            {
                mainForm.UpdateScaleLabel(scale.ToString());
            }
        }

        public void SetROIType(ImageDrawBox.ROIType roiType)
        {
            _imageBox.SetROIType = roiType;
        }

        public void ShowSetScaleForm()
        {
            _imageBox.ShowSetScaleForm();
        }

        public MeasureScale GetScale()
        {
            return measureScale;
        }

    }
}
