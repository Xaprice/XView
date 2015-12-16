using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Emgu.CV;
using Emgu.CV.Structure;

namespace xview.Entities
{
    /// <summary>
    /// old
    /// </summary>
    public class XImage
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(XImage));

        public Image<Bgr, Byte> Image { get; set; }

        private static Dictionary<ImageChannelType, int> channelType2IndexDic = new Dictionary<ImageChannelType, int>();

        public XImage()
        {
            channelType2IndexDic.Add(ImageChannelType.Blue, 0);
            channelType2IndexDic.Add(ImageChannelType.Green, 1);
            channelType2IndexDic.Add(ImageChannelType.Red, 2);
        }

        public void SetImageChannelGain(ImageChannelType channelType, double gainScale)
        {
            try
            {
                Image<Gray, Byte>[] splits = Image.Split();
	            int index = channelType2IndexDic[channelType];
	            splits[index] = splits[index]*gainScale;
                CvInvoke.cvMerge(splits[0].Ptr, splits[1].Ptr, splits[2].Ptr, IntPtr.Zero, Image.Ptr);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        
    }

    /// <summary>
    /// old
    /// </summary>
    public enum ImageChannelType
    {
        Red = 1,
        Green,
        Blue
    }
}
