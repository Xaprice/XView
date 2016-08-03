using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using xview.Constants;
using xview.Measure;
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

        private List<MeasureScale> scaleList = new List<MeasureScale>();

        public SetUnitForm(ImageDrawBox drawArea, double pixels)
        {
            InitializeComponent();

            try
            {
                this.drawArea = drawArea;
                if (pixels < 0)
                {
                    this.groupBoxCreateNewScale.Enabled = false;
                }
                init(pixels);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void init(double pxs)
        {
            try
            {
                this.pixels = pxs;
                pixelLabel.Text = String.Format("{0} 像素 = ", pixels);
                
                pixelsPerUnit = 1.0;
                UnitType = UnitTypeDef.Micrometer;
                cbUnits.SelectedIndex = (int)UnitType;
                cbUnits.SelectedIndexChanged += cbUnits_SelectedIndexChanged;

                this.menuItemDeleteScale.Click += menuItemDeleteScale_Click;
                this.menuItemClearScales.Click += menuItemClearScales_Click;
                this.menuItemApplyScale.Click += menuItemApplyScale_Click;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        void menuItemApplyScale_Click(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = this.gvScales.FocusedRowHandle;
                if (this.gvScales.IsValidRowHandle(rowHandle))
                {
                    this.drawArea.SetScale(this.scaleList[rowHandle]);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        void menuItemClearScales_Click(object sender, EventArgs e)
        {
            try
            {
                this.scaleList.Clear();
                this.gcScales.RefreshDataSource();
                updateAllScalesToXml();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        void menuItemDeleteScale_Click(object sender, EventArgs e)
        {
            try
            {
                int focusedRow = this.gvScales.FocusedRowHandle;
                if (this.gvScales.IsValidRowHandle(focusedRow))
                {
                    this.scaleList.RemoveAt(focusedRow);
                    updateAllScalesToXml();
                    this.gcScales.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void SetUnitForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.scaleList = loadAllScalesFromXml();
                this.gcScales.DataSource = this.scaleList;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 从xml文件中读取所有的scale
        /// </summary>
        /// <returns></returns>
        private List<MeasureScale> loadAllScalesFromXml()
        {
            List<MeasureScale> scaleList = new List<MeasureScale>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ProgramConstants.DEFAULT_PROGRAM_CONFIG_FILE_PATH);
            XmlElement root = xmldoc.DocumentElement;
            XmlNode scaleRootNode = root.SelectSingleNode("Scales");
            foreach (XmlNode scaleNode in scaleRootNode.ChildNodes)
            {
                if (scaleNode is XmlElement)
                {
                    XmlElement scaleElement = scaleNode as XmlElement;
                    string scaleName = scaleElement.GetAttribute("Name");
                    double pixels = double.Parse(scaleElement.GetAttribute("Pixels"));
                    double unitValue = double.Parse(scaleElement.GetAttribute("UnitValue"));
                    int unitType = int.Parse(scaleElement.GetAttribute("UnitType"));
                    MeasureScale measureScale = new MeasureScale();
                    measureScale.Name = scaleName;
                    measureScale.Pixels = pixels;
                    measureScale.UnitValue = unitValue;
                    measureScale.UnitType = (UnitTypeDef)UnitType;
                    scaleList.Add(measureScale);
                }
            }
            return scaleList;
        }

        /// <summary>
        /// 将所有scale同步到xml文件中
        /// </summary>
        private void updateAllScalesToXml()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(ProgramConstants.DEFAULT_PROGRAM_CONFIG_FILE_PATH);
            XmlElement root = xmldoc.DocumentElement;
            XmlNode scaleRootNode = root.SelectSingleNode("Scales");
            scaleRootNode.RemoveAll();
            foreach(MeasureScale scale in this.scaleList)
            {
                XmlElement scaleNode = xmldoc.CreateElement("Scale");
                scaleNode.SetAttribute("Name", scale.Name);
                scaleNode.SetAttribute("Pixels", scale.Pixels.ToString());
                scaleNode.SetAttribute("UnitValue", scale.UnitValue.ToString());
                scaleNode.SetAttribute("UnitType", ((int)(scale.UnitType)).ToString());
                scaleRootNode.AppendChild(scaleNode);
            }
            xmldoc.Save(ProgramConstants.DEFAULT_PROGRAM_CONFIG_FILE_PATH);
        }

        /// <summary>
        /// 添加新的标尺
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!double.TryParse(tbUnit.Text, out this.unitNum))
                {
                    XMessageDialog.Warning("请输入数字！");
                }
                if (unitNum == 0)
                {
                    XMessageDialog.Warning("请输入大于0的值！");
                    return;
                }
                if (String.IsNullOrEmpty(tbNewScaleName.Text))
                {
                    XMessageDialog.Warning("请输入标尺名！");
                    return;
                }
                if (this.scaleList.Exists(x => x.Name == tbNewScaleName.Text))
                {
                    XMessageDialog.Warning("无法添加，存在重名标尺！");
                    return;
                }

                //保存标尺到配置文件
                MeasureScale newScale = new MeasureScale()
                {
                    Name = tbNewScaleName.Text,
                    Pixels = pixels,
                    UnitValue = unitNum,
                    UnitType = (UnitTypeDef)(this.cbUnits.SelectedIndex)
                };

                scaleList.Add(newScale);
                updateAllScalesToXml();
                this.gcScales.RefreshDataSource();

                //pixelsPerUnit = pixels / unitNum;
                //设置当前的标尺
                if (drawArea != null)
                    drawArea.SetScale(newScale);
                    //drawArea.SetUnit(pixelsPerUnit);
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

    }
}
