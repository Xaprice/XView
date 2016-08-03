using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using xview.utils;

namespace xview.UserControls
{
    /// <summary>
    ///  相机采集面板
    /// </summary>
    public partial class CaptureParaPanel : UserControl
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CaptureParaPanel));

        public CaptureParaPanel()
        {
            InitializeComponent();
        }

        public void Init()
        {
            try
            {
                XCamera cam = XCamera.GetInstance();
                XCameraCapturePara capturePara = cam.CapturePara;

                if (capturePara.ImageFileType == emDSFileType.FILE_JPG)
                    _radioGroupCaptureFileType.SelectedIndex = 0;
                else if (capturePara.ImageFileType == emDSFileType.FILE_BMP)
                    _radioGroupCaptureFileType.SelectedIndex = 1;
                else
                    _radioGroupCaptureFileType.SelectedIndex = 2;

	            _trackBarJpgFileQulity.Enabled = (_radioGroupCaptureFileType.SelectedIndex == 0);
                _trackBarJpgFileQulity.Value = capturePara.ImageQuality;
                _buttonEditPicSavePath.Text = capturePara.ImageSavePath;

                _spinEditMutiCaptureCnt.Value = capturePara.MutiCaptureCount;
                _textEditMutiCaptureTime.Text = ((double)(capturePara.MutiCaptureTimeStep) / 1000.0).ToString();

                _spinEditAccuFrameCnt.Value = capturePara.FluModeAccuFrameCnt;
                _spinEditFrameStep.Value = capturePara.FluModeFrameStep;
	
	            int videoQuality = 2;
                var cameraCamType = Convert.ToInt32(ConfigManager.GetAppConfig("CameraType"));
                if (cameraCamType == 1)
                {
                    cam.GetRecordQuality(out videoQuality);
                    _radioGroupAVIQuality.SelectedIndex = videoQuality - 1;
                }
                _buttonEditVideoSavePath.Text = capturePara.VideoSavePath;
                int maxVideoSize;
                cam.GetMaxVideoFileSize(out maxVideoSize);
                _spinEditMaxVideoSize.Value = maxVideoSize;
                if (!cam.IsActive())
                {
                    _radioGroupAVIQuality.Enabled = false;
                    _spinEditMaxVideoSize.Enabled = false;
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _buttonEditPicSavePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
	            FolderBrowserDialog folderDig = new FolderBrowserDialog();
	            if (folderDig.ShowDialog() == DialogResult.OK)
	            {
	                _buttonEditPicSavePath.Text = folderDig.SelectedPath;
                    XCamera.GetInstance().CapturePara.ImageSavePath = folderDig.SelectedPath; 
	            }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _buttonEditVideoSavePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDig = new FolderBrowserDialog();
                if (folderDig.ShowDialog() == DialogResult.OK)
                {
                    _buttonEditVideoSavePath.Text = folderDig.SelectedPath;
                    XCamera.GetInstance().CapturePara.VideoSavePath = folderDig.SelectedPath + "\\";
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _radioGroupAVIQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var cameraCamType = Convert.ToInt32(ConfigManager.GetAppConfig("CameraType"));
                if(cameraCamType == 1)
                {
                    XCamera.GetInstance().SetRecordQuality(_radioGroupAVIQuality.SelectedIndex + 1);
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _spinEditMaxVideoSize_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
	            int maxVideoSize = Convert.ToInt32(_spinEditMaxVideoSize.Value);
	            XCamera.GetInstance().SetMaxVideoFileSize(maxVideoSize);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _radioGroupCaptureFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
	            if (_radioGroupCaptureFileType.SelectedIndex == 0)
	                XCamera.GetInstance().CapturePara.ImageFileType = emDSFileType.FILE_JPG;
	            else if(_radioGroupCaptureFileType.SelectedIndex == 1)
	                XCamera.GetInstance().CapturePara.ImageFileType = emDSFileType.FILE_BMP;
	            else
	                XCamera.GetInstance().CapturePara.ImageFileType = emDSFileType.FILE_PNG;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _trackBarJpgFileQulity_Scroll(object sender, EventArgs e)
        {
            try
            {
            	XCamera.GetInstance().CapturePara.ImageQuality = _trackBarJpgFileQulity.Value;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _spinEditMutiCaptureCnt_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
            	XCamera.GetInstance().CapturePara.MutiCaptureCount = Convert.ToInt32(_spinEditMutiCaptureCnt.Value);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _textEditMutiCaptureTime_EditValueChanged(object sender, EventArgs e)
        {
            try
            {            
	            double sec = Convert.ToDouble(_textEditMutiCaptureTime.Text) * 1000;
	            int miliSec = Convert.ToInt32(sec);
	            XCamera.GetInstance().CapturePara.MutiCaptureTimeStep = miliSec;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                XMessageDialog.Warning("请填入正确的值！");
                _textEditMutiCaptureTime.Text = "1";
            }
        }

        private void _spinEditAccuFrameCnt_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
	            XCamera.GetInstance().CapturePara.FluModeAccuFrameCnt = Convert.ToInt32(_spinEditAccuFrameCnt.Value);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _spinEditFrameStep_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                XCamera.GetInstance().CapturePara.FluModeFrameStep = Convert.ToInt32(_spinEditFrameStep.Value);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
