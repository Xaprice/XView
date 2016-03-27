using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;
using Emgu.CV.UI;
using xview.UserControls;
using DevExpress.XtraTabbedMdi;
using xview.Forms;
using System.IO;
using System.Runtime.InteropServices;
using xview.common;
using xview.Views;
using xview.utils;
using DrawTools;
using xview.Draw;

namespace xview
{
    public partial class MainForm : Form, IMessageFilter
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));

        private CameraPara _camParaUC;
        private CapturePara _captureParaUC;
        private ImagePara _imageParaUC;
        private RWParameter _rwParaUC;
        private ImageConfigPanelUC imageConfig = null;
        private GalleryUC galleryUC = null;
        private MeasurePanel measureUC = null;
        //private Form measurePanelContainerForm = null;

        private string _selectedCameraName;
        private readonly int _checkCameraInterval = 1000;

        #region 窗体函数
        public MainForm()
        {
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this,typeof(SplashScreen1),true,false);
            InitializeComponent();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(false, 0, this);
        }

        /// <summary>
        /// 加载窗体时的操作
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Application.AddMessageFilter(this);

	            if (this != null && IsHandleCreated)
                {
                    PreviewForm.VideoCaptureStartedEvent += new PreviewForm.VideoCaptureStartedHandler(_childForm_VideoCaptureStarted);
                    PreviewForm.VideoCaptureStoppedEvent += new PreviewForm.VideoCaptureStoppedHandler(_childForm_VideoCaptureStopped);

                    //_barCheckItemSimpleMode.Checked = true;

                    InitWorkSpace();
                    InitBars();
                    InitCameraPanels();
                    SetDockPanelVisibility(true);

                    //图像模式面板
                    imageConfig = new ImageConfigPanelUC();
                    imageConfig.Dock = DockStyle.Fill;
                    imageConfig.SetImagePara += imageConfig_SetImagePara;
                    tabImageSet.Controls.Add(imageConfig);

                    galleryUC = new GalleryUC();
                    galleryUC.Dock = DockStyle.Fill;
                    galleryUC.OpenImage += new GalleryUC.OpenImageEventHandler(galleryUC_OpenImage);
                    tabGally.Controls.Add(galleryUC);

                    bool hasUsableCamera = SelectDefaultCamera();
                    UpdataWindowBySelectCameraResult(hasUsableCamera);
                    if (hasUsableCamera)
                    {
                        //如果有可用相机，自动打开
                        HandlePreviewCamera();
                        StartCheckCameraTimer();
                    }
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }



        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 522)
            {
                short zDelta = WMMSGHelper.HIWORD(m.WParam.ToInt32());
                if (zDelta > 0)
                {
                    ZoomInActiveWindow();
                }
                if (zDelta < 0)
                {
                    ZoomOutActiveWindow();
                }
                return true;
            }
            return false;
        }

        void imageConfig_SetImagePara(object sender, SingleDataEventArgs<ImageConfigPara> e)
        {
            Form form = GetActiveChildForm();
            if(form is ImageForm)
            {
                ImageForm imgForm = form as ImageForm;
                imgForm.SetImageProps(e.Data);
            }
        }

        private void galleryUC_OpenImage(object sender, string fullName)
        {
            if(!IsImageOpened(fullName))
                OpenImageFromFile(fullName);
        }

        private bool IsImageOpened(string fullName)
        {
            try
            {
	            foreach (XtraMdiTabPage page in _xtraTabbedMdiManager.Pages)
	            {
                    ImageForm imageForm = page.MdiChild as ImageForm;
                    if (imageForm != null && imageForm.FullImageFileName == fullName)
                    {
                        return true;
                    }
	            }
	            return false;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 初始化工作目录
        /// </summary>
        private void InitWorkSpace()
        {
            try
            {
	            if (!Directory.Exists(ProgramConstants.DEFAULT_WORK_PATH))
	            {
	                Directory.CreateDirectory(ProgramConstants.DEFAULT_WORK_PATH);
	            }
	            if (!Directory.Exists(ProgramConstants.DEFAULT_VIDEO_PATH))
	            {
	                Directory.CreateDirectory(ProgramConstants.DEFAULT_VIDEO_PATH);
	            }
	            if (!Directory.Exists(ProgramConstants.DEFAULT_PICTURE_PATH))
	            {
	                Directory.CreateDirectory(ProgramConstants.DEFAULT_PICTURE_PATH);
	            }
	            if (!Directory.Exists(ProgramConstants.DEFAULT_INI_PATH))
	            {
	                Directory.CreateDirectory(ProgramConstants.DEFAULT_INI_PATH);
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 初始化控制面板
        /// </summary>
        private void InitCameraPanels()
        {
            //“相机”停靠面板
            _camParaUC = new CameraPara();
            _camParaUC.Enabled = false;
            _camParaUC.Dock = DockStyle.Fill;
            tabCameraPara.Controls.Add(_camParaUC);
            _camParaUC.SetAEROI += new CameraPara.SetAEROIEventHandler(_camParaUC_SetAEROI);
            _camParaUC.SetPreviewROI += new CameraPara.SetPreviewROIEventHandler(_camParaUC_SetPreviewROI);
            _camParaUC.SwitchPreviewResolution += new CameraPara.SwitchPreviewResolutionHandler(_camParaUC_SwitchPreviewResolution);
            //“采集”停靠面板
            _captureParaUC = new CapturePara();
            _captureParaUC.Enabled = false;
            _captureParaUC.Dock = DockStyle.Fill;
            tabCapturePara.Controls.Add(_captureParaUC);
            //“图像”停靠面板（预览）
            _imageParaUC = new ImagePara();
            _imageParaUC.Enabled = false;
            _imageParaUC.Dock = DockStyle.Fill;
            _imageParaUC.SetWBWindow += new ImagePara.SetWBWindowEventHandler(_imageParaUC_SetWBWindow);
            tabVideoPara.Controls.Add(_imageParaUC);
            //“参数”停靠面板
            _rwParaUC = new RWParameter();
            _rwParaUC.Enabled = false;
            _rwParaUC.Dock = DockStyle.Fill;
            tabSavePara.Controls.Add(_rwParaUC);

            
        }

        public void PreviewForm_ImageSavedEvent(XTwoExtraValueEventArgs<string, string> e)
        {
            try
            {
                galleryUC.RefreshGallery();
                //xtraTabControl1.SelectedTabPage = tabGally;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 初始化菜单和工具栏
        /// </summary>
        private void InitBars()
        {
            try
            {
                //相机菜单
                _barButtonItemSelectCamera.Enabled = true;
                _barButtonItemPreview.Enabled = false;
                _barButtonItemCloseCamera.Enabled = false;
                _barButtonItemCapture.Enabled = false;
                _barButtonItemMenuMutiCapture.Enabled = false;
                _barButtonItemMenuFluCapture.Enabled = false;
                _barButtonItemMenuStartRecord.Enabled = false;
                _barButtonItemMenuStopRecord.Enabled = false;
                //相机工具栏
                _barButtonItemSelectToolCamera.Enabled = true;
                _barButtonItemToolPreviewCamera.Enabled = false;
                _barButtonItemToolClosePreview.Enabled = false;
                _barButtonCapture.Enabled = false;
                _barButtonItemToolMutiCapture.Enabled = false;
                _barButtonCaptureFluMode.Enabled = false;
                _barButtonToolStartRecord.Enabled = false;
                _barButtonItemToolStopRecord.Enabled = false;
                //图像菜单
                _barButtonItemMenuSaveImage.Enabled = false;
                _barButtonItemMenuImageSaveAs.Enabled = false;
                _barButtonItemMenuHistogram.Enabled = false;
                //图像工具栏
                _barButtonItemSaveImage.Enabled = false;
                _barButtonItemShowHistogram.Enabled = false;
                //缩放工具栏
                _barButtonItemZoomIn.Enabled = false;
                _barButtonItemZoomOut.Enabled = false;
                _barButtonItemRealSize.Enabled = false;
                _barButtonItemFitScreen.Enabled = false;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 关闭窗体时的操作
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopCheckCameraTimer();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 精简模式（全屏）切换事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _barCheckItemSimpleMode_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_barCheckItemSimpleMode.Checked)
                {
                    _statusBar.Visible = false;
                    _mainMenuBar.Visible = false;
                    //this.ShowInTaskbar = false;
                    //this.TopMost = true;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    _statusBar.Visible = true;
                    _mainMenuBar.Visible = true;
                    //this.ShowInTaskbar = true;
                    //this.TopMost = false;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _xtraTabbedMdiManager_SelectedPageChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateControls();
                UpdateMeasureData();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _xtraTabbedMdiManager_PageRemoved(object sender, MdiTabPageEventArgs e)
        {
            try
            {
                UpdateControls();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public Form GetActiveChildForm()
        {
            try
            {
                XtraMdiTabPage selectedPage = _xtraTabbedMdiManager.SelectedPage;
                if (selectedPage == null)
                    return null;
                return selectedPage.MdiChild;
            }
            catch (System.Exception ex)
            {
                _logger.Error("获取当前激活子页面时发生异常" + ex.Message);
                return null;
            }
        }

        private IZoomable GetActiveZoomableForm()
        {
            XtraMdiTabPage selectedPage = _xtraTabbedMdiManager.SelectedPage;
            if (selectedPage == null)
                return null;
            return selectedPage.MdiChild as IZoomable;
        }

        private IDrawForm GetActiveDrawForm()
        {
            XtraMdiTabPage selectedPage = _xtraTabbedMdiManager.SelectedPage;
            if (selectedPage == null)
                return null;
            return selectedPage.MdiChild as IDrawForm;
        }

        private PreviewForm GetPreviewChildForm()
        {
            try
            {
                foreach (XtraMdiTabPage page in _xtraTabbedMdiManager.Pages)
                {
                    if (page.MdiChild is PreviewForm)
                    {
                        return page.MdiChild as PreviewForm;
                    }
                }
                return null;
            }
            catch (System.Exception ex)
            {
                _logger.Error("获取预览子页面时发生异常" + ex.Message);
                return null;
            }
        }

        #endregion

        #region 缩放
        private void ZoomInActiveWindow()
        {
            try
            {
                IZoomable childForm = GetActiveZoomableForm();
                if (childForm != null)
                {
                    childForm.ZoomIn();
                    UpdateZoomFactorLabel(childForm.GetZoomFactor());
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void ZoomOutActiveWindow()
        {
            try
            {
                IZoomable childForm = GetActiveZoomableForm();
                if (childForm != null)
                {
                    childForm.ZoomOut();
                    UpdateZoomFactorLabel(childForm.GetZoomFactor());
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemZoomIn_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZoomInActiveWindow();
        }

        private void _barButtonItemZoomOut_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZoomOutActiveWindow();
        }

        private void _barButtonItemRealSize_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                IZoomable childForm = GetActiveZoomableForm();
	            if (childForm != null)
	            {
	                childForm.RealSize();
                    UpdateZoomFactorLabel(childForm.GetZoomFactor());
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemFitScreen_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                IZoomable childForm = GetActiveZoomableForm();
	            if (childForm != null)
	            {
	                childForm.FitScreen();
                    UpdateZoomFactorLabel(childForm.GetZoomFactor());
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void UpdateZoomFactorLabel(double zoomFactor)
        {
            try
            {
                int factor = (int)Math.Round(zoomFactor * 100);
                if (factor < 0)
                    factor = 0;
                if (factor > 1000)
                    factor = 1000;
                this.zoomFactorLabel.Text = factor.ToString() + "%";
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        private void _barButtonItemQuit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.TryToClose();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        
        #region 相机菜单
        private void _barButtonItemSelectCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                HandleSelectCamera();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                HandlePreviewCamera();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemClosePreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                HandleCloseCamera();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemCapture_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartSingleCapture();
        }

        private void _barButtonItemMenuMutiCapture_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartMutiCapture();
        }

        private void _barButtonItemMenuFluCapture_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartFluCapture();
        }

        private void _barButtonItemMenuStartRecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartVideoCapture();
        }

        private void _barButtonItemMenuStopRecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStopVideoCapture();
        }
        #endregion

        #region 相机工具栏
        private void _barButtonItemSelectToolCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleSelectCamera();
        }

        private void _barButtonItemToolPreviewCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                HandlePreviewCamera();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemToolClosePreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                HandleCloseCamera();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonItemToolCapture_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartSingleCapture();
        }

        private void _barButtonItemToolMutiCapture_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartMutiCapture();
        }

        private void _barButtonCaptureToolFluMode_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartFluCapture();
        }

        private void _barButtonToolStartRecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStartVideoCapture();
        }

        private void _barButtonItemToolStopRecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleStopVideoCapture();
        }

        /// <summary>
        /// 标记过曝光区域开关切换事件
        /// </summary>
        private void _barCheckItem_MarkOverExp_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                PreviewForm.SetMarkOverExpFlag(_barCheckItem_MarkOverExp.Checked);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 相机操作
        /// <summary>
        /// 选择默认的相机（列表中的第一个）
        /// </summary>
        /// <returns>相机列表非空：true, 列表空：false</returns>
        private bool SelectDefaultCamera()
        {
            try
            {
                bool retVal = false;
                List<XCameraDevInfo> devList = XCamera.GetDevList();
                if (devList.Count > 0)
                {
                    _selectedCameraName = devList[0].FriendlyName;
                    retVal = true;
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        private void HandleSelectCamera()
        {
            XCamera cam = XCamera.GetInstance();
            if (cam.RunMode != emDSRunMode.RUNMODE_STOP)
            {
                XMessageDialog.Warning("请先关闭相机！");
            }
            else
            {
                SelectCameraForm form = new SelectCameraForm();
                UpdataWindowBySelectCameraResult(form.ShowDialog(this) == DialogResult.OK);
            }
        }

        private void HandlePreviewCamera()
        {
            if (string.IsNullOrEmpty(_selectedCameraName))
            {
                return;
            }
            PreviewForm newPage = new PreviewForm();

            newPage.Width = this.Width - (dockPanel.Width+ 126);
            newPage.Height = this.Height - 8;

            newPage.Text = _selectedCameraName;
            newPage.CameraClosed += new PreviewForm.CameraClosedHandler(OnCameraClosed);
            newPage.CameraOpened += new PreviewForm.CameraOpenedHandler(OnCameraOpened);
            newPage.ImageSavedEvent += PreviewForm_ImageSavedEvent;

            newPage.MdiParent = this;
            newPage.Show();
            newPage.StartPreview(_selectedCameraName);
            UpdateZoomFactorLabel(newPage.GetZoomFactor());
        }

        private void HandleCloseCamera()
        {
            PreviewForm previewForm = GetPreviewChildForm();
            if (previewForm != null)
            {
                previewForm.Close();
            }
        }

        private void HandleStartSingleCapture()
        {
            try
            {
                PreviewForm previewForm = this.GetPreviewChildForm();
                if(previewForm != null)
                {
                    previewForm.StartSingleCapture();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void HandleStartMutiCapture()
        {
            try
            {
                PreviewForm previewForm = GetPreviewChildForm();
                if (previewForm != null)
                {
                    //PreviewForm.StartMutiCapture();
                    previewForm.StartMutiCap();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void HandleStartFluCapture()
        {
            try
            {
                PreviewForm previewForm = GetPreviewChildForm();
                if (previewForm != null)
                {
                    PreviewForm.StartFluCapture();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void HandleStartVideoCapture()
        {
            try
            {
                PreviewForm previewForm = GetPreviewChildForm();
                if (previewForm != null)
                {
                    PreviewForm.StartVideoCapture();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void HandleStopVideoCapture()
        {
            try
            {
                PreviewForm previewForm = GetPreviewChildForm();
                if (previewForm != null)
                {
                    PreviewForm.StopVideoCapture();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 图像菜单
        private void _barButtonItemMenuOpenImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleOpenImage();
        }

        private void _barButtonItemMenuSaveImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleSaveImage();
        }

        private void _barButtonItemMenuImageSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            XMessageDialog.Debug("未实现！");
        }

        private void _barButtonItemMenuHistogram_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleShowHistogram();
        }
        #endregion

        #region 图像工具栏
        /// <summary>
        /// “打开图像”工具栏按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _barButtonItemOpenImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleOpenImage();
        }
        
        /// <summary>
        /// “保存图像”工具栏按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _barButtonItemSaveImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleSaveImage();
        }

        private void _barButtonItemShowHistogram_ItemClick(object sender, ItemClickEventArgs e)
        {
            HandleShowHistogram();
        }
        #endregion

        #region 图像操作
        /// <summary>
        /// 打开图片文件
        /// </summary>
        public void OpenImageFromFile(string fileName)
        {
            try
            {
                Image<Bgr, Byte> img = new Image<Bgr, Byte>(fileName);
                string title = GetImageTitle(fileName);
                if (string.IsNullOrEmpty(title))
                    title = "图像";
                ImageForm newPage = new ImageForm(title, fileName, img);
                newPage.Width = this.Width - (dockPanel.Width + 126);
                newPage.Height = this.Height - 8;
                newPage.MdiParent = this;
                newPage.Show();
                newPage.FitScreen();
                UpdateControls();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                XMessageDialog.Error("打开图片失败！");
            }
        }

        private string GetImageTitle(string fileName)
        {
            string title;
            try
            {
                title = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                if (title.Length > 10)
                    title = title.Substring(0, 10) + "...";
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                title = "";
            }
            return title;
        }

        private void HandleOpenImage()
        {
            try
            {
                OpenFileDialog openFileDlg = new OpenFileDialog();
                openFileDlg.InitialDirectory = ProgramConstants.DEFAULT_PICTURE_PATH + "\\";
                openFileDlg.Filter = "image files |*.jpg|*.bmp|*.png|All files (*.*)|*.*";
                openFileDlg.FilterIndex = 1;
                openFileDlg.RestoreDirectory = true;
                openFileDlg.Multiselect = false;
                if (openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    OpenImageFromFile(openFileDlg.FileName);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void HandleSaveImage()
        {
            try
            {
                Form childForm = GetActiveChildForm();
                if (childForm != null && childForm is ImageForm)
                {
                    (childForm as ImageForm).Save();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void HandleShowHistogram()
        {
            try
            {
                Form activeChildForm = GetActiveChildForm();
                if (activeChildForm != null && activeChildForm is ImageForm)
                {
                    (activeChildForm as ImageForm).ShowHistogramForm();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region “工具”菜单
        private void _barButtonItemMenuImageCompare_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
	            ImageCompare compareForm = new ImageCompare();
	            compareForm.Show();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 帮助菜单
        private void _barButtonItemAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                AboutBox aboutBox = new AboutBox();
                aboutBox.ShowDialog();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 订阅事件
        private void _camParaUC_SetAEROI(object sender, EventArgs e)
        {
            try
            {
                PreviewForm previewForm = GetPreviewChildForm();
	            if (previewForm != null)
	            {
                    previewForm.SettingROIType = SettingROI.AE_ROI;
                    previewForm.Cursor = Cursors.Cross;
                    Cursor.Clip = previewForm.GetCursorRect();
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _camParaUC_SetPreviewROI(object sender, EventArgs e)
        {
            try
            {
                PreviewForm previewForm = GetPreviewChildForm();
                if (previewForm != null)
                {
                    previewForm.SettingROIType = SettingROI.PVW_ROI;
                    previewForm.Cursor = Cursors.Cross;
                    Cursor.Clip = previewForm.GetCursorRect();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _camParaUC_SwitchPreviewResolution(object sender, EventArgs e, int resolutioSel)
        {
            try
            {
                PreviewForm form = GetPreviewChildForm();
                if (form != null)
                {
                    form.SwitchPreviewResolution(resolutioSel);
                    UpdateZoomFactorLabel(form.GetZoomFactor());
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _imageParaUC_SetWBWindow(object sender, EventArgs e)
        {
            try
            {
                //ChildForm activeForm = GetActiveChildForm();
                //if (activeForm != null && !activeForm.IsPreviewMode)
                //{
                //    SetROIForm setROIForm = new SetROIForm();
                //    setROIForm.FormTitle = "白平衡窗口设置：";
                //    setROIForm.Image = activeForm.Image;
                //    if (setROIForm.ShowDialog() == DialogResult.OK)
                //    {
                //        Rectangle roi = setROIForm.ROI;
                //        if (XCamera.GetInstance().SetWBWindow(roi.X, roi.Y, roi.Width, roi.Height))
                //        {
                //            XMessageDialog.Info("设置白平衡窗口成功！");
                //        }
                //    }
                //}
                try
                {
                    PreviewForm previewForm = GetPreviewChildForm();
                    if (previewForm != null)
                    {
                        previewForm.SettingROIType = SettingROI.WB_ROI;
                        previewForm.Cursor = Cursors.Cross;
                        Cursor.Clip = previewForm.GetCursorRect();
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void OnCameraOpened(object sender, EventArgs e)
        {
            try
            {
	            //SetCameraPanelEnableState(true);
	            _camParaUC.Init();
	            _imageParaUC.Init();
	            _captureParaUC.Init();
	            _rwParaUC.Init();

                UpdateControls();

                //状态栏
                _barStaticItemPreviewStatus.Glyph = xview.Properties.Resources.show_16x16;
                _barStaticItemPreviewStatus.Caption = "预览已激活";
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void OnCameraClosed(object sender, EventArgs e)
        {
            try
            {
                //SetCameraPanelEnableState(false);
                UpdateControls();
                //状态栏
                _barStaticItemPreviewStatus.Glyph = xview.Properties.Resources.hide_16x16;
                _barStaticItemPreviewStatus.Caption = "预览未激活";
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _childForm_VideoCaptureStarted(EventArgs e)
        {
            try
            {
                UpdateControls();
	            _barStaticItemVideoCaptureStatus.Glyph = xview.Properties.Resources.videocamera_run_16_16;
                _barStaticItemVideoCaptureStatus.Caption = "录像已激活";
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _childForm_VideoCaptureStopped(EventArgs e)
        {
            try
            {
                UpdateControls();
                _barStaticItemVideoCaptureStatus.Glyph = xview.Properties.Resources.videocamera_stop_16_16;
                _barStaticItemVideoCaptureStatus.Caption = "录像未激活";
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 更新控件
        /// </summary>
        private void UpdateControls()
        {
            Form childForm = GetActiveChildForm();
            if (childForm != null)
            {
                IZoomable zoomable = GetActiveZoomableForm();
                UpdateZoomFactorLabel(zoomable.GetZoomFactor());
                this.zoomFactorLabel.Visible = true;
                //SetDockPanelVisibility(childForm.IsPreviewMode);
                //缩放工具栏
                _barButtonItemZoomIn.Enabled = true;
                _barButtonItemZoomOut.Enabled = true;
                _barButtonItemRealSize.Enabled = true;
                _barButtonItemFitScreen.Enabled = true;

                if (childForm is PreviewForm)//预览模式
                {
                    SetDockPanelVisibility(true);
                    SetCameraPanelEnableState(true);
                    //相机菜单
                    _barButtonItemPreview.Enabled = false;
                    _barButtonItemCloseCamera.Enabled = true;
                    _barButtonItemCapture.Enabled = true;
                    _barButtonItemMenuMutiCapture.Enabled = true;
                    _barButtonItemMenuFluCapture.Enabled = true;
                    if (PreviewForm.GetCaptureState() == CaptureState.VIDEO_CAPTURING)
                    {
                        _barButtonItemMenuStartRecord.Enabled = false;
                        _barButtonItemMenuStopRecord.Enabled = true;
                    }
                    else
                    {
                        _barButtonItemMenuStartRecord.Enabled = true;
                        _barButtonItemMenuStopRecord.Enabled = false;
                    }
                    //相机工具栏
                    _barButtonItemToolPreviewCamera.Enabled = false;
                    _barButtonItemToolClosePreview.Enabled = true;
                    _barButtonCapture.Enabled = true;
                    _barButtonItemToolMutiCapture.Enabled = true;
                    _barButtonCaptureFluMode.Enabled = true;
                    if (PreviewForm.GetCaptureState() == CaptureState.VIDEO_CAPTURING)
                    {
                        _barButtonToolStartRecord.Enabled = false;
                        _barButtonItemToolStopRecord.Enabled = true;
                    }
                    else
                    {
                        _barButtonToolStartRecord.Enabled = true;
                        _barButtonItemToolStopRecord.Enabled = false;
                    }
                    //图像菜单
                    _barButtonItemMenuSaveImage.Enabled = false;
                    _barButtonItemMenuImageSaveAs.Enabled = false;
                    _barButtonItemMenuHistogram.Enabled = false;
                    //图像工具栏
                    _barButtonItemSaveImage.Enabled = false;
                    _barButtonItemShowHistogram.Enabled = false;
                }
                else //图像模式
                {
                    SetDockPanelVisibility(false);
                    SetCameraPanelEnableState(false);
                    if (GetPreviewChildForm() == null && !string.IsNullOrEmpty(_selectedCameraName))
                    {
                        _barButtonItemPreview.Enabled = true;
                        _barButtonItemToolPreviewCamera.Enabled = true;
                    }
                    else
                    {
                        _barButtonItemPreview.Enabled = false;
                        _barButtonItemToolPreviewCamera.Enabled = false;
                    }
                    //相机菜单
                    _barButtonItemCloseCamera.Enabled = false;
                    _barButtonItemCapture.Enabled = false;
                    _barButtonItemMenuMutiCapture.Enabled = false;
                    _barButtonItemMenuFluCapture.Enabled = false;
                    _barButtonItemMenuStartRecord.Enabled = false;
                    _barButtonItemMenuStopRecord.Enabled = false;
                    //相机工具栏
                    _barButtonItemToolClosePreview.Enabled = false;
                    _barButtonCapture.Enabled = false;
                    _barButtonItemToolMutiCapture.Enabled = false;
                    _barButtonCaptureFluMode.Enabled = false;
                    _barButtonToolStartRecord.Enabled = false;
                    _barButtonItemToolStopRecord.Enabled = false;
                    //图像菜单
                    _barButtonItemMenuSaveImage.Enabled = true;
                    _barButtonItemMenuImageSaveAs.Enabled = true;
                    _barButtonItemMenuHistogram.Enabled = true;
                    //图像工具栏
                    _barButtonItemSaveImage.Enabled = true;
                    _barButtonItemShowHistogram.Enabled = true;
                }
            }
            else//无激活窗口
            {
                this.zoomFactorLabel.Visible = false;
                SetDockPanelVisibility(true);
                if (!string.IsNullOrEmpty(_selectedCameraName))
                {
                    _barButtonItemPreview.Enabled = true;
                    _barButtonItemToolPreviewCamera.Enabled = true;
                }
                else
                {
                    _barButtonItemPreview.Enabled = false;
                    _barButtonItemToolPreviewCamera.Enabled = false;
                }
                //缩放工具栏
                _barButtonItemZoomIn.Enabled = false;
                _barButtonItemZoomOut.Enabled = false;
                _barButtonItemRealSize.Enabled = false;
                _barButtonItemFitScreen.Enabled = false;
                //相机菜单
                //_barButtonItemPreview.Enabled = false;
                _barButtonItemCloseCamera.Enabled = false;
                _barButtonItemCapture.Enabled = false;
                _barButtonItemMenuMutiCapture.Enabled = false;
                _barButtonItemMenuFluCapture.Enabled = false;
                _barButtonItemMenuStartRecord.Enabled = false;
                _barButtonItemMenuStopRecord.Enabled = false;
                //相机工具栏
                //_barButtonItemToolPreviewCamera.Enabled = false;
                _barButtonItemToolClosePreview.Enabled = false;
                _barButtonCapture.Enabled = false;
                _barButtonItemToolMutiCapture.Enabled = false;
                _barButtonCaptureFluMode.Enabled = false;
                _barButtonToolStartRecord.Enabled = false;
                _barButtonItemToolStopRecord.Enabled = false;
                //图像菜单
                _barButtonItemMenuSaveImage.Enabled = false;
                _barButtonItemMenuImageSaveAs.Enabled = false;
                _barButtonItemMenuHistogram.Enabled = false;
                //图像工具栏
                _barButtonItemSaveImage.Enabled = false;
                _barButtonItemShowHistogram.Enabled = false;
            }
        }

        private void SetDockPanelVisibility(bool isPreviewMode)
        {
            tabCameraPara.PageVisible = isPreviewMode;
            tabVideoPara.PageVisible = isPreviewMode;
            tabCapturePara.PageVisible = isPreviewMode;
            tabSavePara.PageVisible = isPreviewMode;
            //tabImageSet.PageVisible = !isPreviewMode;
            tabImageSet.PageVisible = false; //未完成，暂时先隐藏
            tabGally.PageVisible = true;
        }

        public void SetCameraPanelEnableState(bool state)
        {
            try
            {
                _camParaUC.Enabled = state;
                _captureParaUC.Enabled = state;
                _imageParaUC.Enabled = state;
                _rwParaUC.Enabled = state;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private bool TryToClose()
        {
            PreviewForm previewForm = GetPreviewChildForm();
            if (previewForm != null)
            {
                previewForm.IsCloseWithMainForm = true;
            }
            this.Close();
            return true;
        }

        private void _toolBarButtonQuit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
            	this.TryToClose();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _toolBarItemAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
	            AboutBox aboutBox = new AboutBox();
                aboutBox.TopMost = true;
	            aboutBox.ShowDialog();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _toolBarButtonMinimize_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
            	this.WindowState = FormWindowState.Minimized;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void UpdataWindowBySelectCameraResult(bool hasUsableCamera)
        {
            if (hasUsableCamera)
            {
                _barStaticItemCameraName.Caption = _selectedCameraName;
                _barStaticItemCameraName.Glyph = xview.Properties.Resources.apply_16x16;
                _barButtonItemPreview.Enabled = true;
                _barButtonItemToolPreviewCamera.Enabled = true;
            }
            else
            {
                _barStaticItemCameraName.Caption = "无可用设备";
                _barStaticItemCameraName.Glyph = xview.Properties.Resources.cancel_16x16;
                _barButtonItemPreview.Enabled = false;
                _barButtonItemToolPreviewCamera.Enabled = false;
            }
        }

        private void StartCheckCameraTimer()
        {
            _timer4CheckCamera.Interval = _checkCameraInterval;
            _timer4CheckCamera.Enabled = true;
        }

        private void StopCheckCameraTimer()
        {
            _timer4CheckCamera.Enabled = false;
        }

        private void _timer4CheckCamera_Tick(object sender, EventArgs e)
        {
            try
            {
                if (XCamera.GetInstance().RunMode == emDSRunMode.RUNMODE_PLAY)
                    return;
	            List<XCameraDevInfo> devList = XCamera.GetDevList();
	            UpdataWindowBySelectCameraResult(devList.Count != 0);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 【高级设置】工具栏按钮
        /// </summary>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
	            AdvancedSettingForm setForm = new AdvancedSettingForm();
	            setForm.ShowDialog();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void barCheckItemShowFactorLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.zoomFactorLabel.Visible = barCheckItemShowFactorLabel.Checked;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        //mesure
        private void SetActiveDrawTool(ImageDrawBox.DrawToolType drawToolType)
        {
            try
            {
                IDrawForm drawForm = GetActiveDrawForm();
                if (drawForm != null)
                {
                    drawForm.SetActiveDrawTool(drawToolType);
                }

                //Form form = GetActiveChildForm();
                //if (form != null)
                //{
                //    if (form is ImageForm)
                //    {
                //        ImageForm imageForm = form as ImageForm;
                //        imageForm.SetActiveDrawTool(drawToolType);
                //    }
                //    else
                //    {
                //        PreviewForm previewForm = form as PreviewForm;
                //        previewForm.SetActiveDrawTool(drawToolType);
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonLine_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetActiveDrawTool(ImageDrawBox.DrawToolType.Line);
        }

        private void _barButtonRectangle_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetActiveDrawTool(ImageDrawBox.DrawToolType.Rectangle);
        }

        private void _barButtonEllipse_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetActiveDrawTool(ImageDrawBox.DrawToolType.Ellipse);
        }

        private void barButtonPolyline_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetActiveDrawTool(ImageDrawBox.DrawToolType.Polygon);
        }

        private void _barButtonDeleteDrawObject_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                IDrawForm drawForm = GetActiveDrawForm();
                if (drawForm != null)
                {
                    //delete selected drawobjects
                    drawForm.DeleteDrawObjects(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonClearDrawObjects_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                IDrawForm drawForm = GetActiveDrawForm();
                if (drawForm != null)
                {
                    //delete all drawobjects
                    drawForm.DeleteDrawObjects(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void barButtonSelectAllDrawObjects_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                IDrawForm drawForm = GetActiveDrawForm();
                if (drawForm != null)
                {
                    //select all drawobjects
                    drawForm.SelectAllDrawObjects();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _barButtonShowMeasurePanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                measureUC = new MeasurePanel();
                measureUC.Dock = DockStyle.Fill;
                UpdateMeasureData();
                Form measurePanelContainerForm = new Form();
                measurePanelContainerForm.Text = "测量";
                measurePanelContainerForm.Controls.Add(measureUC);
                measurePanelContainerForm.Width = 600;
                measurePanelContainerForm.Height = 400;
                measurePanelContainerForm.Show();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 更新当前激活的页面的对应的测量数据
        /// </summary>
        private void UpdateMeasureData()
        {
            IDrawForm drawForm = GetActiveDrawForm();
            if (drawForm != null && measureUC!=null)
            {
                measureUC.SetMeasureData(drawForm.GetMeasureListData(), drawForm.GetMeasureStatisticData());
            }
        }


    }
}
