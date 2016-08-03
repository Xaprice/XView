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
    /// <summary>
    /// 枚举图像的属性值
    /// </summary>
    public enum ImagePropType
    {
        RED_CHANNEL = 0,
        GREEN_CHANNEL,
        BLUE_CHANNEL,
        GAMMA,
        CONTRAST,
        SATURATION
    }

    /// <summary>
    /// 图像参数设置面板
    /// </summary>
    public partial class ImageParaPanel : UserControl
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ImageParaPanel));

        //TODO: 这里用接口比较好
        //当前被激活的图像窗口引用
        private ImageForm activeImageForm = null;

        public ImageParaPanel()
        {
            InitializeComponent();
            initPanel();
        }

        private void ImageParaSetPanel_Load(object sender, EventArgs e)
        {
        }

        private void initPanel()
        {
            try
            {
                //this.trackBarRedGain.ParaName = "R偏移";
                //this.trackBarRedGain.ParaValue = 0;
                this.trackBarRedGain.SetRange(-128, 128);
                this.trackBarRedGain.ParaValueChangeEvent += trackBarRedGain_ParaValueChangeEvent;

                //this.trackBarGreenGain.ParaName = "G偏移";
                //this.trackBarGreenGain.ParaValue = 0;
                this.trackBarGreenGain.SetRange(-128, 128);
                this.trackBarGreenGain.ParaValueChangeEvent += trackBarGreenGain_ParaValueChangeEvent;

                //this.trackBarBlueGain.ParaName = "B偏移";
                //this.trackBarBlueGain.ParaValue = 0;
                this.trackBarBlueGain.SetRange(-128, 128);
                this.trackBarBlueGain.ParaValueChangeEvent += trackBarBlueGain_ParaValueChangeEvent;


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void SetActiveImageForm(ImageForm imageForm)
        {
            this.activeImageForm = imageForm;
        }

        //响应调整红色增益
        private void trackBarRedGain_ParaValueChangeEvent(SingleDataEventArgs<int> e)
        {
            try
            {
                if (activeImageForm != null)
                {
                    activeImageForm.SetImageProps(ImagePropType.RED_CHANNEL, (double)(e.Data));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        void trackBarGreenGain_ParaValueChangeEvent(SingleDataEventArgs<int> e)
        {
            try
            {
                if (activeImageForm != null)
                {
                    activeImageForm.SetImageProps(ImagePropType.GREEN_CHANNEL, (double)(e.Data));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        void trackBarBlueGain_ParaValueChangeEvent(SingleDataEventArgs<int> e)
        {
            try
            {
                if (activeImageForm != null)
                {
                    activeImageForm.SetImageProps(ImagePropType.BLUE_CHANNEL, (double)(e.Data));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        //还原图像
        private void btnRestoreImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (activeImageForm != null)
                {
                    activeImageForm.RestoreImage();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void trackBarGamma_Load(object sender, EventArgs e)
        {

        }

        private void trackBarContrast_Load(object sender, EventArgs e)
        {

        }

        private void trackBarSaturation_Load(object sender, EventArgs e)
        {

        }



    }
}
