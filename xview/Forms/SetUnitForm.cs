using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xview.Constants;
using xview.UserControls;

namespace xview.Forms
{
    public partial class SetUnitForm : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SetUnitForm));

        public UnitTypeDef UnitType { get; set; }

        private double pixels = 0;

        private double pixelsPerUnit = 1.0;
        public double PixelsPerUnit { get { return pixelsPerUnit; } }

        private double unitNum = 1.0;
        //public double UnitNum { get { return unitNum; } }

        private ImageDrawBox drawArea;

        public SetUnitForm(double pixels, ImageDrawBox drawArea)
        {
            InitializeComponent();

            this.drawArea = drawArea;
            init(pixels);
        }

        private void init(double pxs)
        {
            try
            {
                this.pixels = pxs;
                pixelLabel.Text = String.Format("{0} 像素 = ", pixels);
                
                pixelsPerUnit = 1.0;
                cbUnits.SelectedIndex = 0;
                UnitType = UnitTypeDef.Pixel;
                cbUnits.SelectedIndexChanged += cbUnits_SelectedIndexChanged;
                msgLabel.Text = "";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void SetUnitForm_Load(object sender, EventArgs e)
        {
            

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                pixelsPerUnit = pixels / unitNum;
                if (drawArea != null)
                    drawArea.SetUnit(pixelsPerUnit);
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void cbUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UnitType = (UnitTypeDef)(this.cbUnits.SelectedIndex);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void tbUnit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                msgLabel.Text = "";
                if (!double.TryParse(tbUnit.Text, out this.unitNum))
                {
                    msgLabel.Text = "请输入数字！";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }




    }
}
