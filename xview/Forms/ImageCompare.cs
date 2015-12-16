using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;

namespace xview.Forms
{
    public partial class ImageCompare : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ImageCompare));

        private System.Drawing.Image _controlImg1;
        private System.Drawing.Image _controlImg2;

        public ImageCompare()
        {
            InitializeComponent();
        }

        private void _pictureEdit1_LoadCompleted(object sender, EventArgs e)
        {
            try
            {
	            _controlImg1 = _pictureEdit1.Image;
	            _pictureEdit2.Image = _controlImg2;
                //_checkEditShowDiffImg.Enabled = (_pictureEdit1.Image != null && _pictureEdit2.Image != null);
                //_checkEditShowDiffImg.Checked = false;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _pictureEdit2_LoadCompleted(object sender, EventArgs e)
        {
            try
            {
                _controlImg2 = _pictureEdit2.Image;
                _pictureEdit1.Image = _controlImg1;
                //_checkEditShowDiffImg.Enabled = (_pictureEdit1.Image != null && _pictureEdit2.Image != null);
                //_checkEditShowDiffImg.Checked = false;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _checkEditShowDiffImg_CheckedChanged(object sender, EventArgs e)
        {
            //if (_checkEditShowDiffImg.Checked)
            //{
            //    CoverDiffImg();
            //}
            //else
            //{
            //    _pictureEdit1.Image = _controlImg1;
            //    _pictureEdit2.Image = _controlImg2;
            //}
        }

        private void CoverDiffImg()
        {
            try
            {
                Bitmap bitmap1 = new Bitmap(_pictureEdit1.Image);
                Image<Bgr, Byte> img1 = new Image<Bgr, byte>(bitmap1);
                Bitmap bitmap2 = new Bitmap(_pictureEdit2.Image);
                Image<Bgr, Byte> img2 = new Image<Bgr, byte>(bitmap2);

                Image<Bgr, Byte> img2Resized = img2.Resize(img1.Width, img1.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                Image<Bgr, Byte> diffImg = img1.AbsDiff(img2Resized);
                diffImg = diffImg.ThresholdBinary(new Bgr(5, 5, 5), new Bgr(255, 255, 255));
                img1 += diffImg;
                img2Resized += diffImg;
                img2 = img2Resized.Resize(img2.Width, img2.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);

                _pictureEdit1.Image = img1.ToBitmap();
                _pictureEdit2.Image = img1.ToBitmap();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
