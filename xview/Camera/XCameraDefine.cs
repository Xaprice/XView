using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview
{
    /// <summary>
    /// 相机设备信息类
    /// tDSCameraDevInfo结构的包装器类
    /// </summary>
    public class XCameraDevInfo
    {
        /// <summary>
        /// 厂商编号
        /// </summary>
        public UInt32 VendorID { get; set; }
        /// <summary>
        /// 厂商名称
        /// </summary>
        public string VendorName { get; set; }
        /// <summary>
        /// 产品系列
        /// </summary>
        public string ProductSeries { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品昵称
        /// </summary>
        public string FriendlyName { get; set; }
        /// <summary>
        /// 逻辑驱动文件名称
        /// </summary>
        public string DevFileName { get; set; }
        /// <summary>
        /// 内核驱动逻辑设备名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 产品固件版本
        /// </summary>
        public string FirmwareVersion { get; set; }
        /// <summary>
        /// 图像传感器类型
        /// </summary>
        public string SensorType { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public string PortType { get; set; }
    }

    public class XParaRange<T>
    {
        public T MinValue{get;set;}
        public T MaxValue{get;set;}
    }

    public class XCameraAePara
    {
        public bool AeState { get; set; }
        public int AeMode { get; set; }
        public XParaRange<byte> AeTargetParaRange { get; set; }
        public byte CurAeTarget { get; set; }
        public XParaRange<ulong> ExposureTimeParaRange { get; set; }
        public ulong CurExposureTime { get; set; }
        public XParaRange<float> AnalogGainParaRange { get; set; }
        public float CurAnalogGain { get; set; }
    }

    public class XCameraCapturePara
    {
        /// <summary>
        /// 图片文件的格式
        /// </summary>
        public emDSFileType ImageFileType { get; set; }
        /// <summary>
        /// 采集图片的质量，1~100
        /// </summary>
        public int ImageQuality { get; set; }
        /// <summary>
        /// 图片保存目录
        /// </summary>
        public string ImageSavePath { get; set; }
        /// <summary>
        /// 是否是连续采集
        /// </summary>
        public bool IsMutiCapture { get; set; }
        /// <summary>
        /// 连续采集的数量
        /// </summary>
        public int MutiCaptureCount { get; set; }
        /// <summary>
        /// 连续采集的时间间隔，单位ms
        /// </summary>
        public int MutiCaptureTimeStep { get; set; }
        /// <summary>
        /// 荧光采集积累的帧数
        /// </summary>
        public int FluModeAccuFrameCnt { get; set; }
        /// <summary>
        /// 荧光采集的帧间隔
        /// </summary>
        public int FluModeFrameStep { get; set; }
        /// <summary>
        /// 视频保存路径
        /// </summary>
        public string VideoSavePath { get; set; }
    }

    public class XImagePara
    {
        public float RGain { get; set; }
        public float GGain { get; set; }
        public float BGain { get; set; }
        public float Gamma { get; set; }
        public float Contrast { get; set; }
        public float Saturation { get; set; }
        public float Sharpness { get; set; }
        public float AntiNoise { get; set; }
        public bool isHMirror { get; set; }
        public bool isVMirror { get; set; }
        public bool isInverse { get; set; }
        public bool isMono { get; set; }
    }

    public class XAeModeDefine
    {
        public const int AdjustExpGainAndTime = 0;

        public const int AdjustExpTime = 1;

        public const int AdjustExpGain = 2;

        public const int None = -1;//无效参数
    }
}
