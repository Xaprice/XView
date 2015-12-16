using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using log4net;

namespace xview
{
    public class XCamera
    {
        #region 静态变量
        /// <summary>
        /// 日志记录器
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(XCamera));

        private static readonly object lockHelper = new object();

        /// <summary>
        /// 全局唯一的相机实例
        /// </summary>
        private volatile static XCamera _instance = null;
        #endregion

        #region 构造方法
        private XCamera()
        {
            CapturePara = new XCameraCapturePara();
            CapturePara.ImageFileType = emDSFileType.FILE_JPG;
            CapturePara.ImageQuality = 80;
            CapturePara.ImageSavePath = ProgramConstants.DEFAULT_PICTURE_PATH + "\\";
            CapturePara.IsMutiCapture = false;
            CapturePara.MutiCaptureCount = 5;
            CapturePara.MutiCaptureTimeStep = 1000;
            CapturePara.FluModeAccuFrameCnt = 30;
            CapturePara.FluModeFrameStep = 2;
            CapturePara.VideoSavePath = ProgramConstants.DEFAULT_VIDEO_PATH + "\\";

            InitErrorInfoDictionary();
        }
        #endregion

        #region 私有变量和属性
        private int _cameraID = -1;

        private  emDSRunMode _runMode = emDSRunMode.RUNMODE_STOP;
        public emDSRunMode RunMode
        {
            get { return _runMode; }
        }

        public XCameraCapturePara CapturePara { get; set; }

        private Dictionary<emDSCameraStatus, string> _errorInfoDictionary = null;
        #endregion

        #region 静态函数
        /// <summary>
        /// 获取可用的相机设备列表
        /// </summary>
        /// <returns></returns>
        public static List<XCameraDevInfo> GetDevList()
        {
            try
            {
                tDSCameraDevInfo[] pCameraInfo = new tDSCameraDevInfo[10];
                for (int yy = 0; yy < pCameraInfo.Length; yy++)
                {
                    pCameraInfo[yy] = new tDSCameraDevInfo();
                    pCameraInfo[yy].acVendorName = new Byte[64];
                    pCameraInfo[yy].acProductSeries = new Byte[64];
                    pCameraInfo[yy].acProductName = new char[64];
                    pCameraInfo[yy].acFriendlyName = new char[64];
                    pCameraInfo[yy].acDevFileName = new Byte[64];
                    pCameraInfo[yy].acFileName = new Byte[64];
                    pCameraInfo[yy].acFirmwareVersion = new Byte[64];
                    pCameraInfo[yy].acSensorType = new Byte[64];
                    pCameraInfo[yy].acPortType = new Byte[64];
                }
                IntPtr[] ptArray = new IntPtr[1];
                ptArray[0] = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(tDSCameraDevInfo)) * 10);
                IntPtr pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(tDSCameraDevInfo)));
                Marshal.Copy(ptArray, 0, pt, 1);
                int iNum = 0;
                if (CameraGetDevList(pt, ref iNum) != emDSCameraStatus.STATUS_OK)
                    return new List<XCameraDevInfo>();
                List<XCameraDevInfo> retVal = new List<XCameraDevInfo>();
                for (int i = 0; i < iNum; i++)
                {
                    string friendlyName = "";
                    //string sensorType = "";
                    //string portType = "";
                    pCameraInfo[i] = (tDSCameraDevInfo)Marshal.PtrToStructure((IntPtr)((UInt32)pt + i * Marshal.SizeOf(typeof(tDSCameraDevInfo))), typeof(tDSCameraDevInfo));
                    for (int j = 0; pCameraInfo[i].acFriendlyName[j] != '\0'; j++)
                    {
                        friendlyName += pCameraInfo[i].acFriendlyName[j];
                    }
                    //for (int j1 = 0; pCameraInfo[i].acSensorType[j1] != '\0'; j1++)
                    //{
                    //    sensorType += pCameraInfo[i].acSensorType[j1];
                    //}
                    //for (int j2 = 0; pCameraInfo[i].acPortType[j2] != '\0'; j2++)
                    //{
                    //    portType += pCameraInfo[i].acPortType[j2];
                    //}
                    retVal.Add(
                        new XCameraDevInfo()
                        {
                            VendorID = 0,
                            VendorName = "未获取",
                            ProductSeries = "未获取",
                            ProductName = "未获取",
                            FriendlyName = friendlyName,
                            DevFileName = "未获取",
                            FileName = "未获取",
                            FirmwareVersion = "未获取",
                            SensorType = "未获取",//sensorType,
                            PortType = "未获取"//portType
                        });
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                XMessageDialog.Warning("读取可用的相机列表失败！请确认是否已经成功安装相机的驱动...");
                _logger.Error(ex.Message);
                return new List<XCameraDevInfo>();
            }
        }

        /// <summary>
        /// 获取唯一的相机实例
        /// </summary>
        /// <returns></returns>
        public static XCamera GetInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if(_instance == null)
                        _instance = new XCamera();
                }
            }
            return _instance;
        }
        #endregion

        #region 获取相机参数和状态
        public bool IsActive()
        {
            return _cameraID > 0;
        }

        public bool GetCapability(out tDSCameraCapability capability)
        {
            capability = new tDSCameraCapability();
            try
            {
	            bool retVal = true;
                emDSCameraStatus status = CameraGetCapability(_cameraID, ref capability);
	            if ( status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
            	_logger.Error(ex.Message);
                return false;
            }
        }

        private bool IsCameraRunning()
        {
            return (_cameraID > 0 && _runMode != emDSRunMode.RUNMODE_STOP);
        }

        public bool GetResolutionDescriptionList(out List<string> resolutionDescription)
        {
            resolutionDescription = new List<string>();
            try
            {
	            tDSCameraCapability capability = new tDSCameraCapability();
                if (!GetCapability(out capability))
                {
                    return false;
                }
	            tDSImageSize[] pImagesize = new tDSImageSize[8];
	            for (int i = 0; i < 8; i++)
	            {
	                pImagesize[i].acDescription = new byte[64];
	            }
	            int pAddress = capability.pImageSizeDesc + 4;
	            for (int j = 0; j < capability.iImageSizeDec; j++)
	            {
	                CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(pImagesize[j].acDescription, 0), pAddress, 32);
	                resolutionDescription.Add(System.Text.Encoding.GetEncoding("GB2312").GetString(pImagesize[j].acDescription));
	                pAddress = pAddress + Marshal.SizeOf(pImagesize[j]);
	            }
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 相机基本操作
        /// <summary>
        /// 初始化（打开）相机
        /// </summary>
        /// <param name="callbackFunction"></param>
        /// <param name="friendlyName"></param>
        /// <param name="WinHandle"></param>
        /// <returns></returns>
        public bool Init(DelegateProc callbackFunction, String friendlyName, IntPtr WinHandle)
        {
            try
            {
	            bool retVal = true;
                emDSCameraStatus status = CameraInit(callbackFunction, friendlyName, WinHandle, ref _cameraID);
	            if ( status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
            	_logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 反初始化（关闭）相机
        /// </summary>
        /// <returns></returns>
        public bool UnInit()
        {
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = emDSCameraStatus.STATUS_OK;
	            if (_cameraID > 0)
	            {
	                status = CameraUnInit(_cameraID);
	                if (status != emDSCameraStatus.STATUS_OK)
	                {
	                    retVal = false;
	                    string errorStr = _errorInfoDictionary[status];
	                    _logger.Error(errorStr);
	                    XMessageDialog.Error(errorStr);
	                }
	            }
	            if (retVal)
	            {
	                _cameraID = -1;
	                _runMode = emDSRunMode.RUNMODE_STOP;
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 开始预览
        /// </summary>
        /// <returns></returns>
        public bool Play()
        {
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = CameraPlay(_cameraID);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            if (retVal)
	            {
	                _runMode = emDSRunMode.RUNMODE_PLAY;
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 停止预览
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            try
            {
	            bool retVal = true;
	            if (_runMode != emDSRunMode.RUNMODE_STOP)
	            {
	                emDSCameraStatus status = CameraStop(_cameraID);
                    if (status != emDSCameraStatus.STATUS_OK)
                    {
                        retVal = false;
                        string errorStr = _errorInfoDictionary[status];
                        _logger.Error(errorStr);
                        XMessageDialog.Error(errorStr);
                    }
                    else
                    {
                        _runMode = emDSRunMode.RUNMODE_STOP;
                    }
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 分辨率与ROI
        public bool GetPreviewSizeSel(out int sel)
        {
            sel = -1;
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = CameraGetImageSizeSel(_cameraID, ref sel, false);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetPreviewSizeSel(int sel)
        {
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = CameraSetImageSizeSel(_cameraID, sel, false);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetCaptureSizeSel(out int sel)
        {
            sel = -1;
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = CameraGetImageSizeSel(_cameraID, ref sel, true);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetCaptureSizeSel(int sel)
        {
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = CameraSetImageSizeSel(_cameraID, sel, true);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetPreviewSize(out int width, out int height)
        {
            bool isROI = false;
            int iOff, vOff;
            return GetPreviewROI(out isROI, out iOff, out vOff, out width, out height);
        }

        public bool GetPreviewROI(out bool isROI, out int iOff, out int vOff, out int width, out int height)
        {
            isROI = false;
            iOff = vOff = width = height = -1;
            try
           {
	           bool retVal = true;
	            emDSCameraStatus status = CameraGetImageSize(_cameraID, ref isROI, ref iOff, ref vOff, ref width, ref height, false);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                string errorStr = _errorInfoDictionary[status];
	                _logger.Error(errorStr);
	                XMessageDialog.Error(errorStr);
	            }
	            return retVal;
           }
           catch (System.Exception ex)
           {
               _logger.Error(ex.Message);
               return false;
           }
        }

        public bool SetPreviewROI(int hOff, int vOff, int width, int height)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetImageSize(_cameraID, false, hOff, vOff, width, height, false);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetCaptureROI(out bool isROI, out int hOff, out int vOff, out int width, out int height)
        {
            isROI = false;
            hOff = vOff = width = height = -1;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetImageSize(_cameraID, ref isROI, ref hOff, ref vOff, ref width, ref height, true);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetCaptureROI(int hOff, int vOff, int width, int height)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetImageSize(_cameraID, false, hOff, vOff, width, height, true);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetAEWindow(int hOff, int vOff, int width, int height)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetAEWindow(_cameraID, (ushort)hOff, (ushort)vOff, (ushort)width, (ushort)height);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetWBWindow(int hOff, int vOff, int width, int height)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetWBWindow(_cameraID, (ushort)hOff, (ushort)vOff, (ushort)width, (ushort)height);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 设置相机的显示窗口的大小
        /// </summary>
        /// <param name="width">窗口宽度</param>
        /// <param name="height">窗口高度</param>
        /// <returns></returns>
        public bool SetDisplaySize(int width, int height)
        {
            try
            {
	            bool retVal = true;
	            emDSCameraStatus status = CameraSetDisplaySize(_cameraID, width, height);
	            if (status != emDSCameraStatus.STATUS_OK)
	            {
	                retVal = false;
	                logAndShowErrMsg(status);
	            }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        private void logAndShowErrMsg(emDSCameraStatus status)
        {
            string errorStr = _errorInfoDictionary[status];
            _logger.Error(errorStr);
            XMessageDialog.Error(errorStr);
        }
        #endregion

        #region 曝光
        public bool GetAeState(out bool aeState)
        {
            aeState = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetAeState(_cameraID, ref aeState);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetAeState(bool aeState)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetAeState(_cameraID, aeState);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetAeTarget(out Byte aeTarget)
        {
            aeTarget = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetAeTarget(_cameraID, ref aeTarget);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetAeTarget(byte aeTarget)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetAeTarget(_cameraID, aeTarget);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetExposureTime(out ulong expTime, out ulong maxExpTime, out ulong minExpTime)
        {
            expTime = 0;
            maxExpTime = 0;
            minExpTime = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetExposureTime(_cameraID, ref expTime, ref maxExpTime, ref minExpTime);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetExposureTime(ulong expTime)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetExposureTime(_cameraID, expTime);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetAnalogGain(out float analogGain)
        {
            analogGain = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetAnalogGain(_cameraID, ref analogGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetAnalogGain(float analogGain)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetAnalogGain(_cameraID, analogGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetAePara(out XCameraAePara AePara)
        {
            AePara = new XCameraAePara();
            try
            {
                tDSCameraCapability dsCapbility = new tDSCameraCapability();
                if (!GetCapability(out dsCapbility))
                {
                    return false;
                }
                //ae state
                bool aeState = false;
                if (!GetAeState(out aeState))
                {
                    return false;
                }
                //ae mode
                int aeMode = XAeModeDefine.AdjustExpGainAndTime;
                if (!GetAeMode(out aeMode))
                    return false;
                //target
                byte curAeTarget = 0;
                if (!GetAeTarget(out curAeTarget))
                    return false;
                Byte[] tmparr1 = new Byte[8];
                CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(tmparr1, 0), dsCapbility.pExposeDesc + 40, 8);
                ulong aeTargetMin = BitConverter.ToUInt32(tmparr1, 0);
                CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(tmparr1, 0), dsCapbility.pExposeDesc + 48, 8);
                ulong aeTargetMax = BitConverter.ToUInt32(tmparr1, 0);
                //expTime
                ulong curExpTime = 0, maxExpTime = 0, minExpTime = 0;
                if (!GetExposureTime(out curExpTime, out maxExpTime, out minExpTime))
                {
                    return false;
                }
                //analogGain
                float curAnalogGain = 0;
                if (!GetAnalogGain(out curAnalogGain))
                {
                    return false;
                }
                Byte[] tmparr2 = new Byte[4];
                CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(tmparr2, 0), dsCapbility.pExposeDesc + 64, 4);
                float analogGainMin = BitConverter.ToSingle(tmparr2, 0);
                CopyMemory(Marshal.UnsafeAddrOfPinnedArrayElement(tmparr2, 0), dsCapbility.pExposeDesc + 68, 4);
                float analogGainMax = BitConverter.ToSingle(tmparr2, 0);

                AePara = new XCameraAePara()
                {
                    AeState = aeState,
                    AeMode = aeMode,
                    CurAeTarget = curAeTarget,
                    AeTargetParaRange = new XParaRange<byte>()
                    {
                        MinValue = Convert.ToByte(aeTargetMin),
                        MaxValue = Convert.ToByte(aeTargetMax)
                    },
                    CurExposureTime = curExpTime,
                    ExposureTimeParaRange = new XParaRange<ulong>()
                    {
                        MinValue = minExpTime,
                        MaxValue = maxExpTime
                    },
                    CurAnalogGain = curAnalogGain,
                    AnalogGainParaRange = new XParaRange<float>()
                    {
                        MinValue = analogGainMin,
                        MaxValue = analogGainMax
                    }
                };
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetAeMode(int mode)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetAeMode(_cameraID, mode);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetAeMode(out int mode)
        {
            mode = XAeModeDefine.AdjustExpGainAndTime;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetAeMode(_cameraID, ref mode);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 抗频闪
        public bool GetAntiFlickState(out bool isAntiFlick)
        {
            isAntiFlick = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetAntiFlick(_cameraID, ref isAntiFlick);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetAntiFlickState(bool isAntiFlick)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetAntiFlick(_cameraID, isAntiFlick);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetLightFrequency(out emDSLightFrequency frequency)
        {
            frequency = emDSLightFrequency.LIGHT_FREQUENCY_50HZ;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetLightFrequency(_cameraID, ref frequency);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetLightFrequency(emDSLightFrequency frequency)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetLightFrequency(_cameraID, frequency);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 帧率设置
        public bool GetFrameSpeed(out emDSFrameSpeed frameSpeed)
        {
            frameSpeed = emDSFrameSpeed.FRAME_SPEED_NORMAL;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetFrameSpeed(_cameraID, ref frameSpeed);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetFrameSpeed(emDSFrameSpeed frameSpeed)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetFrameSpeed(_cameraID, frameSpeed);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 图像参数
        public bool SetOnceWB()
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetOnceWB(_cameraID);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetColorEnhancement(out bool enable)
        {
            enable = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetColorEnhancement(_cameraID, ref enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetColorEnhancement(bool enable)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetColorEnhancement(_cameraID, enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetGain(out float rGain, out float gGain, out float bGain)
        {
            rGain = gGain = bGain = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetGain(_cameraID, ref rGain, ref gGain, ref bGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetGain(float rGain, float gGain, float bGain)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetGain(_cameraID, rGain, gGain, bGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetGamma(out byte gamma)
        {
            gamma = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetGamma(_cameraID, ref gamma);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetGamma(byte gamma)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetGamma(_cameraID, gamma);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetContrast(out byte contrast)
        {
            contrast = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetContrast(_cameraID, ref contrast);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetContrast(byte contrast)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetContrast(_cameraID, contrast);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetSaturation(out byte saturation)
        {
            saturation = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetSaturation(_cameraID, ref saturation);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetSaturation(byte saturation)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetSaturation(_cameraID, saturation);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 锐度和平滑度
        public bool GetEdgeEnhancement(out bool enable)
        {
            enable = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetEdgeEnhance(_cameraID, ref enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetEdgeEnhancement(bool enable)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetEdgeEnhance(_cameraID, enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetEdgeGain(out byte edgeGain)
        {
            edgeGain = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetEdgeGain(_cameraID, ref edgeGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetEdgeGain(byte edgeGain)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetEdgeGain(_cameraID, edgeGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetNoiseReductionState(out bool enable)
        {
            enable = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetNoiseReductionState(_cameraID, ref enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetNoiseReductionState(bool enable)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetNoiseReductionState(_cameraID, enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetNoiseReductionGain(out int noiseReductionGain)
        {
            noiseReductionGain = 1;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetNoiseReductionGain(_cameraID, ref noiseReductionGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetNoiseReductionGain(int noiseReductionGain)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetNoiseReductionGain(_cameraID, noiseReductionGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool Get3DNoiseReductionState(out bool enable)
        {
            enable = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGet3DNoiseReductionState(_cameraID, ref enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool Set3DNoiseReductionState(bool enable)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSet3DNoiseReductionState(_cameraID, enable);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool Get3DNoiseReductionGain(out int noiseReductionGain)
        {
            noiseReductionGain = 1;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGet3DNoiseReduction(_cameraID, ref noiseReductionGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool Set3DNoiseReductionGain(int noiseReductionGain)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSet3DNoiseReduction(_cameraID, noiseReductionGain);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 镜像
        public bool GetHorizontalMirrorState(out bool isHMirror)
        {
            isHMirror = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetMirror(_cameraID, emDSMirrorDirection.MIRROR_DIRECTION_HORIZONTAL, ref isHMirror);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetHorizontalMirrorState(bool isHMirror)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetMirror(_cameraID, emDSMirrorDirection.MIRROR_DIRECTION_HORIZONTAL, isHMirror);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetVerticalMirrorState(out bool isVMirror)
        {
            isVMirror = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetMirror(_cameraID, emDSMirrorDirection.MIRROR_DIRECTION_VERTICAL, ref isVMirror);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetVerticalMirrorState(bool isVMirror)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetMirror(_cameraID, emDSMirrorDirection.MIRROR_DIRECTION_VERTICAL, isVMirror);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 单色和反色
        public bool GetMonochromeState(out bool isMono)
        {
            isMono = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetMonochrome(_cameraID, ref isMono);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetMonochromeState(bool isMono)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetMonochrome(_cameraID, isMono);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetInverseState(out bool isInverse)
        {
            isInverse = false;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetInverse(_cameraID, ref isInverse);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetInverseState(bool isInverse)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetInverse(_cameraID, isInverse);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 拍照与录像
        public bool CaptureImageToFile(string savePath, emDSFileType fileType, byte quality)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraCaptureFile(_cameraID, savePath, (byte)(fileType), quality);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取录像的质量
        /// </summary>
        /// <param name="quality">1：低，  2：中，  3：高</param>
        /// <returns></returns>
        public bool GetRecordQuality(out int quality)
        {
            quality = 2;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetRecordAVIQuality(_cameraID, ref quality);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 设置录像的质量
        /// </summary>
        /// <param name="quality">1：低，  2：中，  3：高</param>
        /// <returns></returns>
        public bool SetRecordQuality(int quality)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetRecordAVIQuality(_cameraID, quality);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool StartRecord()
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraStartRecordVideo(_cameraID, CapturePara.VideoSavePath);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool StopRecord()
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraStopRecordVideo(_cameraID);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取录像文件最大占用的空间
        /// </summary>
        /// <param name="maxVideoFileSize">单位：MB</param>
        /// <returns></returns>
        public bool GetMaxVideoFileSize(out int maxVideoFileSize)
        {
            maxVideoFileSize = 0;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetRecordFileSize(_cameraID, ref maxVideoFileSize);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SetMaxVideoFileSize(int maxVideoFileSize)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSetRecordFileSize(_cameraID, maxVideoFileSize);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool RecordFrame(int pbyRGB, ref tDSFrameInfo psFrInfo)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraRecordFrame(_cameraID, pbyRGB, ref psFrInfo);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 参数保存和读取
        public bool SaveParameter(emDSParameterTeam team)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSaveParameter(_cameraID, team);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool LoadParameter(emDSParameterTeam team)
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraReadParameter(_cameraID, team);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool GetCurrentParameterTeam(out emDSParameterTeam team)
        {
            team = emDSParameterTeam.PARAMETER_TEAM_A;
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraGetCurrentParameterTeam(_cameraID, ref team);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool LoadDefaultParameter()
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraLoadDefaultParameter(_cameraID);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool SaveParameterToIni()
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraSaveParameterToIni(_cameraID);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool LoadParameterFromIni()
        {
            try
            {
                bool retVal = true;
                emDSCameraStatus status = CameraLoadParameterFromIni(_cameraID);
                if (status != emDSCameraStatus.STATUS_OK)
                {
                    retVal = false;
                    string errorStr = _errorInfoDictionary[status];
                    _logger.Error(errorStr);
                    XMessageDialog.Error(errorStr);
                }
                return retVal;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region 辅助函数

        private void InitErrorInfoDictionary()
        {
            try
            {
	            _errorInfoDictionary = new Dictionary<emDSCameraStatus, string>();
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_IN_PROCESS, "正在通信");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_INTERNAL_ERROR, "内部错误");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_NOT_SUPPORTED, "摄像头不支持该功能");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_NOT_INITIALIZED, "初始化未完成");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_PARAMETER_INVALID, "参数无效");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_TIME_OUT, "通信超时错误");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_IO_ERROR, "硬件IO错误");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_NO_DEVICE_FOUND, "没有发现相机");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_NO_LOGIC_DEVICE_FOUND, "未找到逻辑设备");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_DEVICE_IS_OPENED, "摄像头已经打开");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_MEMORY_NOT_ENOUGH, "没有足够系统内存");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_FILE_CREATE_FAILED, "创建文件失败");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_FILE_INVALID, "文件格式无效");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_WRITE_PROTECTED, "写保护，不可写");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_GRAB_FRAME_ERROR, "数据捕捉失败");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_LOST_DATA, "帧数据部分丢失，不完整");
	            _errorInfoDictionary.Add(emDSCameraStatus.STATUS_EOF_ERROR, "未接收到帧结束符");
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        //private bool HandleReturnVal(emDSCameraStatus status)
        //{
        //    try
        //    {
        //        if (status != emDSCameraStatus.STATUS_OK)
        //        {
        //            string errorStr = _errorInfoDictionary[status];
        //            logger.Error(errorStr);
        //            XMessageDialog.Error(errorStr);
        //        }
        //        return (status == emDSCameraStatus.STATUS_OK);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        return false;
        //    }
        //}
        #endregion

        #region 相机API
        public delegate int DelegateProc(int m_iCam, ref Byte pbyBuffer, ref tDSFrameInfo sFrInfo);
        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr Destination, int Source, int Length);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetDevList(IntPtr pCameraInfo, ref int piNums);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CameraISP(int m_iCameraID, ref Byte pbyRAW, ref tDSFrameInfo psFrInfo);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern emDSCameraStatus CameraDisplayRGB24(int m_iCameraID, int pbyRGB24, ref tDSFrameInfo psFrInfo);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern emDSCameraStatus CameraSetDisplaySize(int m_iCameraID, int iWidth, int iHeight);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraInit(DelegateProc pCallbackFunction, String lpszFriendlyName, IntPtr hWndDisplay, ref int piCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraPlay(int m_iCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraStop(int m_iCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraUnInit(int m_iCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern emDSCameraStatus CameraGetCapability(int m_iCameraID, ref tDSCameraCapability sDSCameraCap);


        #region 曝光参数设置API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetAeMode(int m_iCameraID, int iMode);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetAeMode(int m_iCameraID, ref int iMode);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetAeState(int m_iCameraID, ref Boolean pbAeState);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetAeState(int m_iCameraID, Boolean byAeState);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetExposureTime(int m_iCameraID, ref UInt64 puExposureTime, ref UInt64 puExpTimeMax, ref UInt64 puExpTimeMin);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetExposureTime(int m_iCameraID, UInt64 uExposureTime);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetAeTarget(int m_iCameraID, ref Byte pbyAeTarget);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetAeTarget(int m_iCameraID, Byte byAeTarget);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetAnalogGain(int m_iCameraID, ref float pfAnalogGain);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetAnalogGain(int m_iCameraID, float fAnalogGain);
        #endregion

        #region 图像参数设置API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetColorEnhancement(int m_iCameraID, ref Boolean pbEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetColorEnhancement(int m_iCameraID, Boolean bEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetGain(int m_iCameraID, ref float pfRGain, ref float pfGGain, ref float pfBGain);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetGain(int m_iCameraID, float fRGain, float fGGain, float fBGain);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetGamma(int m_iCameraID, ref Byte pbyGamma);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetGamma(int m_iCameraID, Byte byGamma);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetContrast(int m_iCameraID, ref Byte pbyContrast);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetContrast(int m_iCameraID, Byte byContrast);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetSaturation(int m_iCameraID, ref Byte pbySaturation);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetSaturation(int m_iCameraID, Byte bySaturation);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetOnceWB(int m_iCameraID);
        #endregion

        #region 锐度和抗噪声API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetEdgeEnhance(int m_iCameraID, ref Boolean pbEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetEdgeEnhance(int m_iCameraID, Boolean bEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetEdgeGain(int m_iCameraID, ref Byte pbyEdgeGain);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetEdgeGain(int m_iCameraID, Byte byEdgeGain);
        //[DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern emDSCameraStatus CameraGetNoiseReduction(int m_iCameraID, ref Boolean pbReduction, ref int piReduction);
        //[DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern emDSCameraStatus CameraSetNoiseReduction(int m_iCameraID, Boolean bReduction, int iReduction);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetNoiseReductionState(int iCameraID, Boolean bReduction);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetNoiseReductionState(int iCameraID, ref Boolean pbReduction);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetNoiseReductionGain(int iCameraID, int iReduction);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetNoiseReductionGain(int iCameraID, ref int piReduction);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSet3DNoiseReductionState(int iCameraID, Boolean b3DReduction);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGet3DNoiseReductionState(int iCameraID,  ref Boolean pb3DReduction);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSet3DNoiseReduction(int iCameraID, int i3DReduction);

        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGet3DNoiseReduction(int iCameraID, ref int pi3DReduction);
        #endregion

        #region 相机参数保存读取API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSaveParameter(int m_iCameraID, emDSParameterTeam emTeam);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraReadParameter(int m_iCameraID, emDSParameterTeam emTeam);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetCurrentParameterTeam(int m_iCameraID, ref emDSParameterTeam pemTeam);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraLoadDefaultParameter(int m_iCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSaveParameterToIni(int iCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraLoadParameterFromIni(int iCameraID);
        #endregion

        #region 其他相机参数设置API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetMirror(int m_iCameraID, emDSMirrorDirection emDir, ref Boolean pbEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetMirror(int m_iCameraID, emDSMirrorDirection emDir, Boolean bEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetMonochrome(int m_iCameraID, ref Boolean pbEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetMonochrome(int m_iCameraID, Boolean bEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetInverse(int m_iCameraID, ref Boolean pbEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetInverse(int m_iCameraID, Boolean bEnable);
        #endregion

        #region 抗频闪设置API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetAntiFlick(int m_iCameraID, ref Boolean pbEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetAntiFlick(int m_iCameraID, Boolean bEnable);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetLightFrequency(int m_iCameraID, ref emDSLightFrequency pemFrequency);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetLightFrequency(int m_iCameraID, emDSLightFrequency emFrequency);
        #endregion

        #region 帧率设置API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetFrameSpeed(int m_iCameraID, ref emDSFrameSpeed pemFrameSpeed);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetFrameSpeed(int m_iCameraID, emDSFrameSpeed emFrameSpeed);
        #endregion

        #region 截图
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraCaptureFile(int m_iCameraID, string lpszFileName, Byte byFileType, Byte byQuality);
        #endregion

        #region 分辨率与ROI
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetAEWindow(int m_iCameraID, ushort usHOff, ushort usVOff, ushort usWidth, ushort usHeight);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetWBWindow(int m_iCameraID, ushort usHOff, ushort usVOff, ushort usWidth, ushort usHeight);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern emDSCameraStatus CameraGetImageSizeSel(int m_iCameraID, ref int piResel, Boolean bCaputer);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetImageSizeSel(int m_iCameraID, int iResel, Boolean bCaputer);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetImageSize(int m_iCameraID, bool bReserve, int iHOff, int iVOff, int iWidth, int iHeight, bool bCapture = false);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetImageSize(int iCameraID, ref bool pbROI, ref int piHOff, ref int piVOff, ref int piWidth, ref int piHeight, bool bCapture = false);
        #endregion

        #region 视频录制API
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraStartRecordVideo(int m_iCameraID, string lpszRecordPath);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraStopRecordVideo(int m_iCameraID);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetRecordAVIQuality(int m_iCameraID, ref int piQuality);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetRecordAVIQuality(int m_iCameraID, int iQuality);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern emDSCameraStatus CameraSetRecordEncoder(int m_iCameraID, int iCodeType);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern emDSCameraStatus CameraRecordFrame(int m_iCameraID, int pbyRGB, ref tDSFrameInfo psFrInfo);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraGetRecordFileSize(int iCameraID, ref int piFileSize);
        [DllImport("DVP_CAMSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern emDSCameraStatus CameraSetRecordFileSize(int iCameraID, int iFileSize);
        #endregion

        #endregion
    }
}
