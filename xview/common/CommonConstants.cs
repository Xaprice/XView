using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xview
{
    public class ProgramConstants
    {
        //预置目录
        public const string DEFAULT_WORK_PATH = "C:\\XView";
        public const string DEFAULT_PICTURE_PATH = "C:\\XView\\Pictures";
        public const string DEFAULT_VIDEO_PATH = "C:\\XView\\Videos";
        public const string DEFAULT_INI_PATH = "C:\\XView\\Settings";
        public const string DEFAULT_PROGRAM_CONFIG_FILE_PATH = "C:\\XView\\xview.xml";
    }

    public class CameraListGridFieldNames
    {
        public const string FIELDNAME_CAMERA_NAME = "CameraName";
        public const string FIELDNAME_PORT_TYPE = "PortType";
        public const string FIELDNAME_CAMERA_STATUS = "CameraStatus";
    }

    /// <summary>
    /// 采集状态
    /// </summary>
    public enum CaptureState
    {
        NO_CAPTURE, 
        SINGLE_CAPTURE,
        MULT_CAPTURE_TRIGGER_ON,
        MULT_CAPTURE_TRIGGER_OFF,
        FLU_START_CAPTURE,
        FLU_CAPTURING,
        FLU_STOP_CAPTURE,
        VIDEO_START_CAPTURE,
        VIDEO_CAPTURING,
        VIDEO_STOP_CAPTURE
    }

    public enum SettingROI
    {
        NO_ROI,
        PVW_ROI,
        AE_ROI,
        WB_ROI
    }

}
