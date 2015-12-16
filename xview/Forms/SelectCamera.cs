using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace xview
{
    public partial class SelectCameraForm : Form
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SelectCameraForm));

        public SelectCameraForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择的相机名
        /// </summary>
        public string SelectedCameraName { get; set; }

        private void SelectCameraForm_Load(object sender, EventArgs e)
        {
            try
            {
	            if (this != null && IsHandleCreated)
	            {
                    LoadDevList();
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// 加载相机列表
        /// </summary>
        private void LoadDevList()
        {
            try
            {
	            List<XCameraDevInfo> devList = XCamera.GetDevList();
	
	            string fieldName1 = CameraListGridFieldNames.FIELDNAME_CAMERA_NAME;
	            string fieldName2 = CameraListGridFieldNames.FIELDNAME_PORT_TYPE;
	            string fieldName3 = CameraListGridFieldNames.FIELDNAME_CAMERA_STATUS;
	            DataTable dt = new DataTable();
	            dt.Columns.Add(fieldName1, typeof(string));
	            dt.Columns.Add(fieldName2, typeof(string));
	            dt.Columns.Add(fieldName3, typeof(string));
	            devList.ForEach(x =>
	                {
	                    DataRow dr = dt.NewRow();
	                    dr[fieldName1] = x.FriendlyName;
	                    dr[fieldName2] = "";
	                    dr[fieldName3] = "已连接";
	                    dt.Rows.Add(dr);
	                });
	            _gridControlCameraList.DataSource = dt;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
        /// <summary>
        /// 确定按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnSelectCamera_Click(object sender, EventArgs e)
        {
            try
            {
	            if (_gridViewCameraList.SelectedRowsCount <= 0 || _gridViewCameraList.SelectedRowsCount > 1)
	            {
                    XMessageDialog.Info("请选择一个相机设备！");
	            }
	            else
	            {
                    DataRow dr = _gridViewCameraList.GetFocusedDataRow();
                    SelectedCameraName = Convert.ToString(dr[CameraListGridFieldNames.FIELDNAME_CAMERA_NAME]);
                    if (string.IsNullOrEmpty(SelectedCameraName))
                    {
                        XMessageDialog.Warning("无效的相机名！");
                        this.DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                    }
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 取消按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnCancelSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 刷新按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnSelectCameraRefresh_Click(object sender, EventArgs e)
        {
            LoadDevList();
        }
    }
}
