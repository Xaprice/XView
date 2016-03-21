using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace xview.UserControls
{
    public partial class ImagePara : UserControl
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ImagePara));
        private bool _isInit = false;

        public ImagePara()
        {
            InitializeComponent();
        }

        private void ColorPara_Load(object sender, EventArgs e)
        {
        }

        #region 消息
        public delegate void SetWBWindowEventHandler(object sender, EventArgs e);
        public event SetWBWindowEventHandler SetWBWindow;
        protected virtual void OnSetWBWindow(EventArgs e)
        {
            if(SetWBWindow != null)
            {
                SetWBWindow(this, e);
            }
        }
        #endregion

        #region 初始化、更新界面控件
        public void Init()
        {
            try
            {
                _isInit = true;
                tDSCameraCapability capbility;
                if (XCamera.GetInstance().GetCapability(out capbility))
                {
                    InitRGBGainControls(ref capbility);
                    InitAdvancedImageControls(ref capbility);
                    InitOtherControls(ref capbility);
                    UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                _isInit = false;
            }
        }

        private void InitAdvancedImageControls(ref tDSCameraCapability capbility)
        {
            try
            {
	            XCamera cam = XCamera.GetInstance();
	            //gamma
	            _trackBarGamma.SetRange(capbility.sGammaRange.iMin, capbility.sGammaRange.iMax);
	            byte gamma;
	            cam.GetGamma(out gamma);
	            _trackBarGamma.Value = Convert.ToInt32(gamma);
	            //contrast
	            _trackBarContrast.SetRange(capbility.sContrastRange.iMin, capbility.sContrastRange.iMax);
	            byte contrast;
	            cam.GetContrast(out contrast);
	            _trackBarContrast.Value = Convert.ToInt32(contrast);
	            //saturation
	            bool enableColorEnhance = false;
	            cam.GetColorEnhancement(out enableColorEnhance);
	            _checkEnableColorEnhance.Checked = enableColorEnhance;
	            _trackBarSaturation.Enabled = enableColorEnhance;
	            _trackBarSaturation.SetRange(capbility.sSaturationRange.iMin, capbility.sSaturationRange.iMax);
	            byte saturation;
	            cam.GetSaturation(out saturation);
	            _trackBarSaturation.Value = Convert.ToInt32(saturation);
	            //sharpness
	            _trackBarSharpness.SetRange(capbility.sSharpnessRange.iMin, capbility.sSharpnessRange.iMax);
	            bool enableSharp = false;
	            byte sharpness = 0;
	            cam.GetEdgeEnhancement(out enableSharp);
	            cam.GetEdgeGain(out sharpness);
	            _checkEnableSharp.Checked = enableSharp;
	            _trackBarSharpness.Enabled = enableSharp;
	            _trackBarSharpness.Value = Convert.ToInt32(sharpness);
	            //noise reduction
	            _trackBarAntiNoise.SetRange(capbility.sNoiseReductionRange.iMin, capbility.sNoiseReductionRange.iMax);
	            bool enableNoiseReduction = false;
	            cam.GetNoiseReductionState(out enableNoiseReduction);
	            _checkEnableAntiNoise.Checked = enableNoiseReduction;
	            _trackBarAntiNoise.Enabled = enableNoiseReduction;
	            int noiseReductionGain;
	            cam.GetNoiseReductionGain(out noiseReductionGain);
	            _trackBarAntiNoise.Value = noiseReductionGain;

                var cameraCamType = Convert.ToInt32(xview.utils.ConfigManager.GetAppConfig("CameraType"));
                if(cameraCamType == 2)
                {
                    //3D noise reduction，暂时未从文档中找到3D降噪参数范围值
                    //_trackBar3DAntiNoise.SetRange(capbility.sNoiseReductionRange.iMin, capbility.sNoiseReductionRange.iMax);
                    _trackBar3DAntiNoise.SetRange(0, 255);
                    bool enable3DNoiseReduction = false;
                    cam.Get3DNoiseReductionState(out enable3DNoiseReduction);
                    _checkEnable3DAntiNoise.Checked = enable3DNoiseReduction;
                    _trackBar3DAntiNoise.Enabled = enable3DNoiseReduction;
                    int noiseReductionGain3D;
                    cam.Get3DNoiseReductionGain(out noiseReductionGain3D);
                    _trackBar3DAntiNoise.Value = noiseReductionGain3D;
                }
                else
                {
                    _checkEnable3DAntiNoise.Visible = false;
                    emptySpaceItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    _label3DAntiNoise.Visible = false;
                    layoutControlItem28.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void InitRGBGainControls(ref tDSCameraCapability capbility)
        {
            try
            {
		        //R,G,B通道增益
		        float minRGain = capbility.sRgbGainRange.fRGainMin;
		        float maxRGain = capbility.sRgbGainRange.fRGainMax;
		        _trackBarRGain.SetRange((int)(minRGain * 100), (int)(maxRGain * 100));
		        float minGGain = capbility.sRgbGainRange.fGGainMin;
		        float maxGGain = capbility.sRgbGainRange.fGGainMax;
		        _trackBarGGain.SetRange((int)(minGGain * 100), (int)(maxGGain * 100));
		        float minBGain = capbility.sRgbGainRange.fBGainMin;
		        float maxBGain = capbility.sRgbGainRange.fBGainMax;
		        _trackBarBGain.SetRange((int)(minBGain * 100), (int)(maxBGain * 100));
		        float rGain, gGain, bGain;
		        XCamera.GetInstance().GetGain(out rGain, out gGain, out bGain);
		        _trackBarRGain.Value = (int)(rGain * 100);
		        _trackBarGGain.Value = (int)(gGain * 100);
		        _trackBarBGain.Value = (int)(bGain * 100);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void InitOtherControls(ref tDSCameraCapability capbility)
        {
            try
            {
	            XCamera cam = XCamera.GetInstance();
	            bool isHMirror, isVMirror, isMono, isInverse;
	            cam.GetHorizontalMirrorState(out isHMirror);
	            cam.GetVerticalMirrorState(out isVMirror);
	            cam.GetMonochromeState(out isMono);
	            cam.GetInverseState(out isInverse);
	            _checkHMirror.Checked = isHMirror;
	            _checkVMirror.Checked = isVMirror;
	            _checkMono.Checked = isMono;
	            _checkInverse.Checked = isInverse;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void UpdateLabels()
        {
            try
            {
	            float rGain =Convert.ToSingle(_trackBarRGain.Value)/100;
	            _labelRGain.Text = rGain.ToString("0.00");
	            float gGain = Convert.ToSingle(_trackBarGGain.Value) / 100;
	            _labelGGain.Text = gGain.ToString("0.00");
	            float bGain = Convert.ToSingle(_trackBarBGain.Value) / 100;
	            _labelBGain.Text = bGain.ToString("0.00");
	
	            _labelGamma.Text = _trackBarGamma.Value.ToString();
	            _labelContrast.Text = _trackBarContrast.Value.ToString();
	            _labelSaturation.Text = _trackBarSaturation.Value.ToString();
	
	            _labelSharpness.Text = _trackBarSharpness.Value.ToString();
	            _labelAntiNoise.Text = _trackBarAntiNoise.Value.ToString();
                _label3DAntiNoise.Text = _trackBar3DAntiNoise.Value.ToString();
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region RGB通道
        private void _trackBarRGain_Scroll(object sender, EventArgs e)
        {
            SetGain();
        }

        private void _trackBarGGain_Scroll(object sender, EventArgs e)
        {
            SetGain();
        }

        private void _trackBarBGain_Scroll(object sender, EventArgs e)
        {
            SetGain();
        }

        private void SetGain()
        {
            try
            {
                if (!_isInit)
                {
                    float rGain = Convert.ToSingle(_trackBarRGain.Value) / 100;
                    float gGain = Convert.ToSingle(_trackBarGGain.Value) / 100;
                    float bGain = Convert.ToSingle(_trackBarBGain.Value) / 100;
                    XCamera.GetInstance().SetGain(rGain, gGain, bGain);
                    UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _btnOnceWB_Click(object sender, EventArgs e)
        {
            try
            {
                if (XCamera.GetInstance().SetOnceWB())
                {
                    this.Init();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _btnSetWBWindow_Click(object sender, EventArgs e)
        {
            try
            {
	            OnSetWBWindow(new EventArgs());
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 伽马、对比对、饱和度
        private void _trackBarGamma_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
	            {
		            byte gamma = Convert.ToByte(_trackBarGamma.Value);
		            XCamera.GetInstance().SetGamma(gamma);
		            UpdateLabels();
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _trackBarContrast_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                byte contrast = Convert.ToByte(_trackBarContrast.Value);
	                XCamera.GetInstance().SetContrast(contrast);
	                UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _trackBarSaturation_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                byte saturation = Convert.ToByte(_trackBarSaturation.Value);
	                XCamera.GetInstance().SetSaturation(saturation);
	                UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkEnableColorEnhance_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                if (XCamera.GetInstance().SetColorEnhancement(_checkEnableColorEnhance.Checked))
	                {
	                    _trackBarSaturation.Enabled = _checkEnableColorEnhance.Checked;
	                }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 锐度和平滑度
        private void _trackBarSharpness_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                byte edgeGain = Convert.ToByte(_trackBarSharpness.Value);
	                XCamera.GetInstance().SetEdgeGain(edgeGain);
	                UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _trackBarAntiNoise_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                XCamera.GetInstance().SetNoiseReductionGain(_trackBarAntiNoise.Value);
	                UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _trackBar3DAntiNoise_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
                    XCamera.GetInstance().Set3DNoiseReductionGain(_trackBar3DAntiNoise.Value);
                    UpdateLabels();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkEnableSharp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
                    if (XCamera.GetInstance().SetEdgeEnhancement(_checkEnableSharp.Checked))
                    {
                        _trackBarSharpness.Enabled = _checkEnableSharp.Checked;
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkEnableAntiNoise_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                if (XCamera.GetInstance().SetNoiseReductionState(_checkEnableAntiNoise.Checked))
	                {
	                    _trackBarAntiNoise.Enabled = _checkEnableAntiNoise.Checked;
	                }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkEnable3DAntiNoise_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (XCamera.GetInstance().Set3DNoiseReductionState(_checkEnable3DAntiNoise.Checked))
	                {
	                    _trackBar3DAntiNoise.Enabled = _checkEnable3DAntiNoise.Checked;
	                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        #region 镜像、单色、反色
        private void _checkHMirror_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
            	{
	            	XCamera.GetInstance().SetHorizontalMirrorState(_checkHMirror.Checked);
            	}
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkVMirror_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                XCamera.GetInstance().SetVerticalMirrorState(_checkVMirror.Checked);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkMono_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                XCamera.GetInstance().SetMonochromeState(_checkMono.Checked);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _checkInverse_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                XCamera.GetInstance().SetInverseState(_checkInverse.Checked);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion






    }
}
