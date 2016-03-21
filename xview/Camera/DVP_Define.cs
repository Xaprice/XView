using System.Runtime.InteropServices;
using System;

public enum emDSRunMode
{
    RUNMODE_PLAY = 0,
    RUNMODE_PAUSE = 1,
    RUNMODE_STOP = 2
}

public enum emDSCameraStatus
{
    STATUS_OK = 1,       // �����ɹ�
    STATUS_IN_PROCESS = 2,       // ����ͨ��
    STATUS_INTERNAL_ERROR = -1,      // �ڲ�����
    STATUS_NOT_SUPPORTED = -2,      // ����ͷ��֧�ָù���
    STATUS_NOT_INITIALIZED = -3,      // ��ʼ��δ���
    STATUS_PARAMETER_INVALID = -4,      // ������Ч
    STATUS_TIME_OUT = -1000,   // ͨ�ų�ʱ����
    STATUS_IO_ERROR = -1001,   // Ӳ��IO����
    STATUS_NO_DEVICE_FOUND = -1100,   // û�з������
    STATUS_NO_LOGIC_DEVICE_FOUND = -1101,   // δ�ҵ��߼��豸
    STATUS_DEVICE_IS_OPENED = -1102,   // ����ͷ�Ѿ���
    STATUS_MEMORY_NOT_ENOUGH = -1200,   // û���㹻ϵͳ�ڴ�
    STATUS_FILE_CREATE_FAILED = -1300,   // �����ļ�ʧ��
    STATUS_FILE_INVALID = -1301,   // �ļ���ʽ��Ч
    STATUS_WRITE_PROTECTED = -1400,   // д����������д
    STATUS_GRAB_FRAME_ERROR = -1600,   // ���ݲ�׽ʧ��
    STATUS_LOST_DATA = -1601,   // ֡���ݲ��ֶ�ʧ��������
    STATUS_EOF_ERROR = -1602    // δ���յ�֡������
}


public enum emDSMirrorDirection
{
    MIRROR_DIRECTION_HORIZONTAL = 0,
    MIRROR_DIRECTION_VERTICAL = 1
}

public enum emDSFrameSpeed
{
    FRAME_SPEED_NORMAL,
    FRAME_SPEED_HIGH,
    FRAME_SPEED_SUPER
}

public enum emDSFileType
{
    FILE_JPG = 1,
    FILE_BMP = 2,
    FILE_RAW = 4,
    FILE_PNG = 8
}

public enum emDSDataType
{
    // mono ����
    DATA_TYPE_MONO1P = 0,
    DATA_TYPE_MONO2P = 1,
    DATA_TYPE_MONO4P = 2,
    DATA_TYPE_MONO8 = 3,
    DATA_TYPE_MONO8S = 4,
    DATA_TYPE_MONO10 = 5,
    DATA_TYPE_MONO10_PACKED = 6,
    DATA_TYPE_MONO12 = 7,
    DATA_TYPE_MONO12_PACKED = 8,
    DATA_TYPE_MONO14 = 9,
    DATA_TYPE_MONO16 = 10,

    // bayer ��������
    DATA_TYPE_BAYGR8 = 11,
    DATA_TYPE_BAYRG8 = 12,
    DATA_TYPE_BAYGB8 = 13,
    DATA_TYPE_BAYBG8 = 14,
    DATA_TYPE_BAYGR10 = 15,
    DATA_TYPE_BAYRG10 = 16,
    DATA_TYPE_BAYGB10 = 17,
    DATA_TYPE_BAYBG10 = 18,
    DATA_TYPE_BAYGR12 = 19,
    DATA_TYPE_BAYRG12 = 20,
    DATA_TYPE_BAYGB12 = 21,
    DATA_TYPE_BAYBG12 = 22,
    DATA_TYPE_BAYGR10_PACKED = 23,
    DATA_TYPE_BAYRG10_PACKED = 24,
    DATA_TYPE_BAYGB10_PACKED = 25,
    DATA_TYPE_BAYBG10_PACKED = 27,
    DATA_TYPE_BAYGR12_PACKED = 28,
    DATA_TYPE_BAYRG12_PACKED = 29,
    DATA_TYPE_BAYGB12_PACKED = 30,
    DATA_TYPE_BAYBG12_PACKED = 31,
    DATA_TYPE_BAYGR16 = 32,
    DATA_TYPE_BAYRG16 = 33,
    DATA_TYPE_BAYGB16 = 34,
    DATA_TYPE_BAYBG16 = 35,

    // RGB ��������
    DATA_TYPE_RGB8 = 36,
    DATA_TYPE_BGR8 = 37,
    DATA_TYPE_RGBA8 = 38,
    DATA_TYPE_BGRA8 = 39,
    DATA_TYPE_RGB10 = 40,
    DATA_TYPE_BGR10 = 41,
    DATA_TYPE_RGB12 = 42,
    DATA_TYPE_BGR12 = 43,
    DATA_TYPE_RGB16 = 44,
    DATA_TYPE_RGB10V1_PACKED = 45,
    DATA_TYPE_RGB10P32 = 46,
    DATA_TYPE_RGB12V1_PACKED = 47,
    DATA_TYPE_RGB565P = 48,
    DATA_TYPE_BGR565P = 49,

    // YUV YCbCr
    DATA_TYPE_YUV411_8_UYYVYY = 50,
    DATA_TYPE_YUV422_8_UYVY = 51,
    DATA_TYPE_YUV422_8 = 52,
    DATA_TYPE_YUV8_UYV = 53,
    DATA_TYPE_YCBCR8_CBYCR = 54,
    DATA_TYPE_YCBCR422_8 = 55,
    DATA_TYPE_YCBCR422_8_CBYCRY = 56,
    DATA_TYPE_YCBCR411_8_CBYYCRYY = 57,
    DATA_TYPE_YCBCR601_8_CBYCR = 58,
    DATA_TYPE_YCBCR601_422_8 = 59,
    DATA_TYPE_YCBCR601_422_8_CBYCRY = 60,
    DATA_TYPE_YCBCR601_411_8_CBYYCRYY = 61,
    DATA_TYPE_YCBCR709_8_CBYCR = 62,
    DATA_TYPE_YCBCR709_422_8 = 63,
    DATA_TYPE_YCBCR709_422_8_CBYCRY = 64,
    DATA_TYPE_YCBCR709_411_8_CBYYCRYY = 65,

    // planar
    DATA_TYPE_RGB8_PLANAR = 80,
    DATA_TYPE_RGB10_PLANAR = 81,
    DATA_TYPE_RGB12_PLANAR = 82,
    DATA_TYPE_RGB16_PLANAR = 83
}

public enum emDSLightFrequency
{
    LIGHT_FREQUENCY_50HZ = 0,
    LIGHT_FREQUENCY_60HZ = 1
}

public enum emDSParameterTeam
{
    PARAMETER_TEAM_A = 0,
    PARAMETER_TEAM_B = 1,
    PARAMETER_TEAM_C = 2,
    PARAMETER_TEAM_D = 3,
    PARAMETER_TEAM_LAST = 254,
    PARAMETER_TEAM_DEFAULT = 255
}

//����ͷӲ����Ϣ
 [StructLayout(LayoutKind.Sequential)]
public struct tDSCameraDevInfo
{
    public uint uVendorID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acVendorName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acProductSeries;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public char[] acProductName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public char[] acFriendlyName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acDevFileName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acFileName; 
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acFirmwareVersion;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acSensorType;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public Byte[] acPortType;
 }

public struct tDSROISize
{
    public int iWidth;
    public int iHeight;
    public int iHOffset;
    public int iVOffset;
}

public struct tDSImageSize
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public Byte[] acDescription;
    public Boolean bIsBIN;
    public Boolean bIsSkip;
    public Boolean bIsZoom;
    public int iResolutionMode;	
    public int iWidth;
    public int iHeight;
    public int iHOffset;
    public int iVOffset;
}

public struct tDSImageSizeRange
{
    public int iHeightMax;
    public int iHeightMin;
    public int iHeightDefault;
    public int iWidthMax;
    public int iWidthMin;
    public int iWidthDefault;
    public bool bRoi;
}

public struct tDSFrameSpeed
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acDescription;
}

public struct tDSTimeStamp
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acDescription;
}

public struct tDSExpose
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acDescription;
    public ulong uiTargetMin;
    public ulong uiTargetMax;
    public ulong uiTargetDefault;
    public float fAnalogGainMin;
    public float fAnalogGainMax;
    public float fAnalogGainDefault;
    public float fAnalogGainStep;
    public ulong uiExposeTimeMin;
    public ulong uiExposeTimeMax;
    public ulong uiExposeTimeDefault;
    public ulong uiExposeTimeStep;
}


public struct tDSExposeParam
{
    public bool bAutoEnable;
    public ulong iAutoExposeTarget;
    public float fAnalogGain;
    public ulong iExposeTime;
}

public struct tDSFrameCount
{
    public int iCap;
    public int iTotal;
    public int iLost;
}

public struct tDSMediaType
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acDescription;
    public int iMediaType;
}

public struct tDSResolutionMode
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acDescription;
    public bool bBin;
    public bool bSkip;
    public bool bZoom;
    public int iWidthMax;
    public int iHeightMax;
    public int iHSampling;                     // ˮƽ����ģʽ����ֵ�����ս����ɾ����豸����
    public int iVSampling;                     // ��ֱ����ģʽ����ֵ�����ս����ɾ����豸����
    public int iReadOut;   
}


public struct tGammaRange
{
    public int iMin;
    public int iMax;
    public int iDefault;
}

public struct tContrastRange
{
    public int iMin;
    public int iMax;
    public int iDefault;
}

public struct tRgbGainRange
{
    public float fRGainMin;
    public float fRGainMax;
    public float fRGainDefault;
    public float fGGainMin;
    public float fGGainMax;
    public float fGGainDefault;
    public float fBGainMin;
    public float fBGainMax;
    public float fBGainDefault;
}

public struct tSaturationRange
{
    public int iMin;
    public int iMax;
    public int iDefault;
}

public struct tSharpnessRange
{
    public int iMin;
    public int iMax;
    public int iDefault;
}

public struct tNoiseReductionRange
{
    public int iMin;
    public int iMax;
    public int iDefault;
}

public struct tDSCameraProcCap
{
    public bool bAF;
    public bool bAE;
    public bool bAWB;
    public bool bOnceAF;
    public bool bOnceWB;
    public bool bOnceBB;

    public bool bRgbSum;
    public bool bRgbGain;
    public bool bRgbCorrection;
    public bool bSaturation;

    public bool bRgbMapping;
    public bool bGamma;
    public bool bContrast;

    public bool bVFlip;
    public bool bHFlip;
    public bool bMono;
    public bool bInverse;
    public bool bSharpness;
    public bool bNoiseReduction;
}

public struct tDSModeDesc
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acModeName;
}

public struct tDSSwitchDesc
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acSwitchName;
    public bool bDefault;
}

public struct tDSValueDesc
{
    public int iIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] acValueName;
    public int iDefault;
}

public struct tDSSensorInfo
{
    public uint uSensorType;	// ����CMOS,CCD
    public uint uSensorCount;	// SENSORоƬ����
    public bool bMono;			// �ڰ�/��ɫ
    public uint uPixForm;		// �������У���RGRG,GRGR
    public int iFieldCount;
}

public struct tDSCameraCapability
{
    public tDSSensorInfo sSensorInfo;
    //public tDSCameraProcCap sCameraProcCap;

    /* һЩ�ɵ��ڲ�����Χ�޶���Ĭ��ֵ */
    public tDSImageSizeRange sImageSizeRange;		// ͼ��ߴ緶Χ��Ĭ��ֵ
    public tRgbGainRange sRgbGainRange;			// RGB������ڷ�Χ��Ĭ��ֵ
    public tSaturationRange sSaturationRange;		// ���Ͷȵ��ڷ�Χ��Ĭ��ֵ
    public tGammaRange sGammaRange;			// GAMMA���ڷ�Χ��Ĭ��ֵ
    public tContrastRange sContrastRange;			// �Աȶȵ��ڷ�Χ��Ĭ��ֵ
    public tSharpnessRange sSharpnessRange;		// ��ȵ��ڷ�Χ��Ĭ��ֵ
    public tNoiseReductionRange sNoiseReductionRange;	// �������Ƶ��ڷ�Χ��Ĭ��ֵ

    public int pExposeDesc;
    public int iExposeDesc;
    public int iExposeDefault;

    public int pTriggerDesc;			// ����
    public int iTriggerDesc;
    public int iTriggerDefault;

    public int pImageSizeDesc;
    public int iImageSizeDec;
    public int iImageSizeDefault;

    public int pMediaTypeDesc;
    public int iMediaTypdeDesc;
    public int iMediaTypeDefault;

    public int pResolutionModeDesc;
    public int iResolutionModeDesc;
    public int iResolutionModeDefault;

    public int pFrameSpeedDesc;
    public int iFrameSpeedDesc;
    public int iFrameSpeedDefault;

    public int pTimeStampDesc;
    public int iTimeStampDesc;
    public int iTimeStampDefault;

    public int pPackLenDesc;
    public int iPackLenDesc;
    public int iPackLenDefault;
   
    public Boolean bRomForSave;

    public int pModeDesc;
    public int iModeDesc;
    public int iModeDefault;

    public int pSwitchDesc;
    public int iSwitchDesc;

    public int pValueDesc;
    public int iValueDesc;
}

public struct tDSFrameInfo
{
    public uint uiMediaType;							// RAW/YUV/RGB������DS_DVP_DEFINE.h�ж���
    public uint uBytes;									// ͼ�������ֽ���
    public uint uiHeight;								// �߶�
    public uint uiWidth;								// ���
    public uint uHOff;									// ˮƽ��ʼλ��,ROIʱ������
    public uint uVOff;									// ��ֱ��ʵλ��,ROIʱ������
    public Boolean bHFlip;								// ָʾ�Ƿ�ˮƽ��ת
    public Boolean bVFlip;								// ָʾ�Ƿ�ֱ��ת
    public Boolean bTriggered;							// ָʾ�Ƿ�Ϊ����֡
    public Boolean bTimeStampValid;					// ָʾʱ�����Ч
    public Boolean bExpTimeValid;						// ָʾ�ع�ʱ����Ч
    public uint uFrameID;
    public UInt64 uiTimeStamp;				    		// ʱ�������λ���ɾ����豸ȷ��
    public UInt64 uiExpTime;							// �ع�ʱ�䣬��λ���ɾ����豸ȷ��
}



