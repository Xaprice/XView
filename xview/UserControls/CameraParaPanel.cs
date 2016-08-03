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
    public partial class CameraParaPanel : UserControl
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CameraParaPanel));
        private static readonly ulong msPerHour = 3600000;
        private static readonly ulong msPerMin = 60000;
        private static readonly ulong msPerSec = 1000;

        private bool _isInit = false;

        /********************************************************************************
         * 事件定义
         ********************************************************************************/
        //设置AE ROI事件
        public delegate void SetAEROIEventHandler(object sender, EventArgs e);
        public event SetAEROIEventHandler SetAEROI;
        protected virtual void OnSetAEROI(EventArgs e)
        {
            if (SetAEROI != null)
            {
                SetAEROI(this, e);
            }
        }

        //设置预览ROI事件
        public delegate void SetPreviewROIEventHandler(object sender, EventArgs e);
        public event SetPreviewROIEventHandler SetPreviewROI;
        protected virtual void OnSetPreviewROI(EventArgs e)
        {
            if (SetPreviewROI != null)
            {
                SetPreviewROI(this, e);
            }
        }

        //切换分辨率事件
        public delegate void SwitchPreviewResolutionHandler(object sender, EventArgs e, int resolutioSel);
        public event SwitchPreviewResolutionHandler SwitchPreviewResolution;
        protected virtual void PublishSwitchPreviewResolutionEvent(EventArgs e, int resolutioSel)
        {
            if (SwitchPreviewResolution != null)
            {
                SwitchPreviewResolution(this, e, resolutioSel);
            }
        }

        /********************************************************************************
        * 构造与初始化
        ********************************************************************************/
        public CameraParaPanel()
        {
            InitializeComponent();
            //TODO: 该功能暂时不可用，将该功能按钮隐藏
            this.layoutControlItem15.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        public void Init()
        {
            try
            {
                _isInit = true;
                XCamera cam = XCamera.GetInstance();
                //cam.SetAeMode(XAeModeDefine.AdjustExpGainAndTime);
	            XCameraAePara aePara;
                if (cam.GetAePara(out aePara))
	            {
                    _checkBoxEnableAE.Checked = aePara.AeState;
                    if (aePara.AeMode == XAeModeDefine.AdjustExpGainAndTime)
                    {
                        _comboBoxEditAEMode.SelectedIndex = 0;
                    }
                    else if(aePara.AeMode == XAeModeDefine.AdjustExpGain)
                    {
                        _comboBoxEditAEMode.SelectedIndex = 1;
                    }
                    else if (aePara.AeMode == XAeModeDefine.AdjustExpGain)
                    {
                        _comboBoxEditAEMode.SelectedIndex = 2;
                    }

	                UpdateControlsByAePara(aePara);
                    InitExpTargetAndGainCtrls(aePara);
                    ulong minExpTime = aePara.ExposureTimeParaRange.MinValue / 10000; //将单位转换为ms
                    ulong maxExpTime = aePara.ExposureTimeParaRange.MaxValue / 10000;//将单位转换为ms
                    ulong curExpTime = aePara.CurExposureTime / 10000;//将单位转换为ms
	                InitExpTimeCtrls(minExpTime, maxExpTime, curExpTime);
                    InitAntiFlickCtrls();
                    InitFrameSpeedCtrls();
                    InitResolutionCombo();
	            }

                //在初始化之后绑定事件响应方法，
                _checkBoxEnableAE.CheckedChanged += _checkEditEnableAe_CheckedChanged;
                _comboBoxEditAEMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                _comboBoxEditAEMode.SelectedIndexChanged += _comboBoxEditAEMode_SelectedIndexChanged;
              

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


        private void InitExpTargetAndGainCtrls(XCameraAePara aePara)
        {
            _trackBarAeTarget.Minimum = Convert.ToInt32(aePara.AeTargetParaRange.MinValue);
            _trackBarAeTarget.Maximum = Convert.ToInt32(aePara.AeTargetParaRange.MaxValue);
            _trackBarAeTarget.Value = Convert.ToInt32(aePara.CurAeTarget);
            _labelTargetValue.Text = _trackBarAeTarget.Value.ToString();
            _trackBarControlAnalogGain.Minimum = Convert.ToInt32(aePara.AnalogGainParaRange.MinValue * 10);
            _trackBarControlAnalogGain.Maximum = Convert.ToInt32(aePara.AnalogGainParaRange.MaxValue * 10);
            _trackBarControlAnalogGain.Value = Convert.ToInt32(aePara.CurAnalogGain * 10);
            _labelAnalogGain.Text = _trackBarControlAnalogGain.Value.ToString();
        }

        private void InitExpTimeCtrls(ulong minExpTime, ulong maxExpTime, ulong curExpTime)
        {
            try
            {
	            if (curExpTime < minExpTime)
	                curExpTime = minExpTime;
	            if (curExpTime > maxExpTime)
	                curExpTime = maxExpTime;
	            _spinEditHour.Properties.MinValue = minExpTime / msPerHour;
	            _spinEditHour.Properties.MaxValue = maxExpTime / msPerHour;
	            _spinEditHour.Value = curExpTime / msPerHour;
	            _spinEditMinite.Properties.MinValue = (minExpTime % msPerHour) / msPerMin;
	            _spinEditMinite.Properties.MaxValue = (maxExpTime % msPerHour) / msPerMin;
	            _spinEditMinite.Value = (curExpTime % msPerHour) / msPerMin;
	            _spinEditSecond.Properties.MinValue = ((minExpTime % msPerHour) % msPerMin) / msPerSec;
	            _spinEditSecond.Properties.MaxValue = ((maxExpTime % msPerHour) % msPerMin) / msPerSec;
	            _spinEditSecond.Value = ((curExpTime % msPerHour) % msPerMin) / msPerSec;
	            _spinEditMilisecond.Properties.MinValue = ((minExpTime % msPerHour) % msPerMin) % msPerSec;
	            _spinEditMilisecond.Properties.MaxValue = ((maxExpTime % msPerHour) % msPerMin) % msPerSec;
	            _spinEditMilisecond.Value = ((curExpTime % msPerHour) % msPerMin) % msPerSec;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void InitAntiFlickCtrls()
        {
            try
            {
	            bool isAntiFlick = false;
	            XCamera.GetInstance().GetAntiFlickState(out isAntiFlick);
	            if (isAntiFlick)
	            {
	                emDSLightFrequency frequency;
	                XCamera.GetInstance().GetLightFrequency(out frequency);
	                if (frequency == emDSLightFrequency.LIGHT_FREQUENCY_50HZ)
	                    _radioGroupAntiFlick.SelectedIndex = 1;
	                else
	                    _radioGroupAntiFlick.SelectedIndex = 2;
	            }
	            else
	            {
	                _radioGroupAntiFlick.SelectedIndex = 0;
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void InitFrameSpeedCtrls()
        {
            try
            {
	            emDSFrameSpeed frameSpeed;
	            XCamera.GetInstance().GetFrameSpeed(out frameSpeed);
	            switch (frameSpeed)
	            {
	                case emDSFrameSpeed.FRAME_SPEED_NORMAL:
	                    _radioGroupFrameSpeed.SelectedIndex = 0;
	                    break;
	                case emDSFrameSpeed.FRAME_SPEED_HIGH:
	                    _radioGroupFrameSpeed.SelectedIndex = 1;
	                    break;
	                case emDSFrameSpeed.FRAME_SPEED_SUPER:
	                    _radioGroupFrameSpeed.SelectedIndex = 2;
	                    break;
	                default:
	                    break;
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void InitResolutionCombo()
        {
            try
            {
	            _comboBoxEditResolution.Properties.Items.Clear();
                comboBoxEditCaptureRes.Properties.Items.Clear();
	            List<string> resolutionDescription;
	            if (XCamera.GetInstance().GetResolutionDescriptionList(out resolutionDescription))
	            {
	                _comboBoxEditResolution.Properties.Items.AddRange(resolutionDescription.ToArray());
                    comboBoxEditCaptureRes.Properties.Items.AddRange(resolutionDescription.ToArray());
	            }
	            int sel = -1;
	            if (XCamera.GetInstance().GetPreviewSizeSel(out sel))
	            {
	                _comboBoxEditResolution.SelectedIndex = sel;
	            }
                if(XCamera.GetInstance().GetCaptureSizeSel(out sel))
                {
                    comboBoxEditCaptureRes.SelectedIndex = sel;
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private ulong GetExpTimeValueFromCtrls()
        {
            try
            {
	            decimal expTimeValue = _spinEditHour.Value * msPerHour + _spinEditMinite.Value * msPerMin +
	                _spinEditSecond.Value * msPerSec + _spinEditMilisecond.Value;
	            return Convert.ToUInt64(expTimeValue);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }
        }

        private void UpdateControlsByAePara(XCameraAePara aePara)
        {
            if(aePara.AeState)
            {
                _trackBarAeTarget.Enabled = true;
                _btnSetAEROI.Enabled = true;
                _comboBoxEditAEMode.Enabled = true;
                UpdateExpControlsByAeMode(aePara.AeMode);
            }
            else
            {
                _trackBarAeTarget.Enabled = false;
                _btnSetAEROI.Enabled = false;
                SetExpTimeControlEnableState(true);
                SetExpGainControlEnableState(true);
                _comboBoxEditAEMode.Enabled = false;
            }
        }

        private void _checkEditEnableAe_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
                    bool aeState = _checkBoxEnableAE.Checked;
                    if (XCamera.GetInstance().SetAeState(aeState))
                    {
                        XCameraAePara aePara;
                        if (XCamera.GetInstance().GetAePara(out aePara))
                        {
                            UpdateControlsByAePara(aePara);
                            InitExpTargetAndGainCtrls(aePara);
                            ulong minExpTime = aePara.ExposureTimeParaRange.MinValue / 10000; //将单位转换为ms
                            ulong maxExpTime = aePara.ExposureTimeParaRange.MaxValue / 10000;//将单位转换为ms
                            ulong curExpTime = aePara.CurExposureTime / 10000;//将单位转换为ms
                            InitExpTimeCtrls(minExpTime, maxExpTime, curExpTime);
                            InitAntiFlickCtrls();
                            InitFrameSpeedCtrls();
                            InitResolutionCombo();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _comboBoxEditAEMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(XCamera.GetInstance().SetAeMode(this._comboBoxEditAEMode.SelectedIndex))
                    UpdateExpControlsByAeMode(this._comboBoxEditAEMode.SelectedIndex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        #region 曝光时间
        private void _spinEditHour_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit)
            {
            	SetExpTime();
            }
        }

        private void _spinEditMinite_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit)
            {
                SetExpTime();
            }
        }

        private void _spinEditSecond_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit)
            {
                SetExpTime();
            }
        }

        private void _spinEditMilisecond_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isInit)
            {
                SetExpTime();
            }
        }

        private void SetExpTime()
        {
            try
            {
	            ulong expTime = GetExpTimeValueFromCtrls();
	            if (expTime != 0)
	            {
                    XCamera.GetInstance().SetExposureTime(expTime * 10000); //时间单位：0.1us, 1ms = 1000us
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        #endregion

        private void _trackBarAeTarget_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                _labelTargetValue.Text = _trackBarAeTarget.Value.ToString();
	                XCamera.GetInstance().SetAeTarget(Convert.ToByte(_trackBarAeTarget.Value));
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _trackBarControlAnalogGain_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                _labelAnalogGain.Text = _trackBarControlAnalogGain.Value.ToString();
	                XCamera.GetInstance().SetAnalogGain(Convert.ToSingle(_trackBarControlAnalogGain.Value) / 10.0f);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _radioGroupAntiFlick_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                XCamera cam = XCamera.GetInstance();
		            switch (_radioGroupAntiFlick.SelectedIndex)
		            {
		                case 0:  //直流
	                        cam.SetAntiFlickState(false);
		                    break;
		                case 1: //50Hz
	                        cam.SetAntiFlickState(true);
	                        cam.SetLightFrequency(emDSLightFrequency.LIGHT_FREQUENCY_50HZ);
		                    break;
		                case 2: //60Hz
	                        cam.SetAntiFlickState(true);
	                        cam.SetLightFrequency(emDSLightFrequency.LIGHT_FREQUENCY_60HZ);
		                    break;
		                default:
		                    break;
		            }
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _radioGroupFrameSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
	                XCamera cam = XCamera.GetInstance();
	                switch (_radioGroupFrameSpeed.SelectedIndex)
	                {
	                    case 0:  //普通
	                        cam.SetFrameSpeed(emDSFrameSpeed.FRAME_SPEED_NORMAL);
	                        break;
	                    case 1: //高速
	                        cam.SetFrameSpeed(emDSFrameSpeed.FRAME_SPEED_HIGH);
	                        break;
	                    case 2: //超高速
	                        cam.SetFrameSpeed(emDSFrameSpeed.FRAME_SPEED_SUPER);
	                        break;
	                    default:
	                        break;
	                }
	                Init();
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _btnSetAEROI_Click(object sender, EventArgs e)
        {
            try
            {
            	OnSetAEROI(new EventArgs());
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 切换相机预览分辨率
        /// </summary>
        private void _comboBoxEditResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
	            {
                    PublishSwitchPreviewResolutionEvent(new EventArgs(), _comboBoxEditResolution.SelectedIndex);
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void _btnSetPreviewROI_Click(object sender, EventArgs e)
        {
            try
            {
            	OnSetPreviewROI(new EventArgs());
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }



        private void UpdateExpControlsByAeMode(int aeMode)
        {
            bool enableExpGain = false;
            bool enableExpTime = false;
            if(aeMode == XAeModeDefine.AdjustExpGainAndTime)
            {
                enableExpGain = false;
                enableExpTime = false;
            }
            else if(aeMode == XAeModeDefine.AdjustExpGain)
            {
                enableExpTime = true;
            }
            else if(aeMode == XAeModeDefine.AdjustExpTime)
            {
                enableExpGain = true;   
            }
            else
            {
                enableExpGain = true;
                enableExpTime = true;
            }
            SetExpTimeControlEnableState(enableExpTime);
            SetExpGainControlEnableState(enableExpGain);
        }

        //private int GetAeModeByCheckEditState()
        //{
        //    int aeMode;
        //    //if (_checkBoxAdjustGain.Checked && _checkBoxAdjustExpTime.Checked)
        //    //{
        //    //    aeMode = XAeModeDefine.AdjustExpGainAndTime;
        //    //}
        //    //else if (_checkBoxAdjustGain.Checked && !_checkBoxAdjustExpTime.Checked)
        //    //{
        //    //    aeMode = XAeModeDefine.AdjustExpGain;
        //    //}
        //    //else if (!_checkBoxAdjustGain.Checked && _checkBoxAdjustExpTime.Checked)
        //    //{
        //    //    aeMode = XAeModeDefine.AdjustExpTime;
        //    //}
        //    //else
        //    //{
        //    //    aeMode = XAeModeDefine.None;
        //    //}

        //    return aeMode;
        //}

        private void SetExpTimeControlEnableState(bool enable)
        {
            _spinEditHour.Enabled = enable;
            _spinEditMinite.Enabled = enable;
            _spinEditSecond.Enabled = enable;
            _spinEditMilisecond.Enabled = enable;
        }

        private void SetExpGainControlEnableState(bool enable)
        {
            _trackBarControlAnalogGain.Enabled = enable;
        }

        private void comboBoxEditCaptureRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isInit)
                {
                    XCamera.GetInstance().SetCaptureSizeSel(comboBoxEditCaptureRes.SelectedIndex);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }



    }
}
