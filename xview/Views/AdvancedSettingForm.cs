using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using log4net;
using xview.utils;

namespace xview.Views
{
    public partial class AdvancedSettingForm : Form
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AdvancedSettingForm));

        public AdvancedSettingForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
	            //工作目录子页面初始化
                string workPathImage = "WorkPathImage";
                string workPathVideo = "WorkPathVideo";
                string workPathConfig = "WorkPathConfig";
	            this.ButtonEdit_ImagePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(WorkPathConfigButtonClicked);
	            this.ButtonEdit_VideoPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(WorkPathConfigButtonClicked);
	            this.ButtonEdit_ConfigPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(WorkPathConfigButtonClicked);
                this.ButtonEdit_ImagePath.Tag = workPathImage;
                this.ButtonEdit_VideoPath.Tag = workPathVideo;
                this.ButtonEdit_ConfigPath.Tag = workPathConfig;
                this.ButtonEdit_ImagePath.Text = ConfigManager.GetAppConfig(workPathImage);
                this.ButtonEdit_VideoPath.Text = ConfigManager.GetAppConfig(workPathVideo);
                this.ButtonEdit_ConfigPath.Text = ConfigManager.GetAppConfig(workPathConfig);


// 	            this.ButtonEdit_ImagePath.EditValueChanged += new EventHandler(WorkPathEditValueChanged);
// 	            this.ButtonEdit_VideoPath.EditValueChanged += new EventHandler(WorkPathEditValueChanged);
// 	            this.ButtonEdit_ConfigPath.EditValueChanged += new EventHandler(WorkPathEditValueChanged);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void WorkPathConfigButtonClicked(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
	            ButtonEdit edit = sender as ButtonEdit;
                string tag = edit.Tag as string;
	            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.ShowNewFolderButton = true;
                folderBrowserDialog.SelectedPath = ConfigManager.GetAppConfig("WorkPath");
	            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
	            {
	                edit.Text = folderBrowserDialog.SelectedPath;
                    ConfigManager.SetAppConfig(tag, edit.Text);
	            }
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private void WorkPathEditValueChanged(object sender, EventArgs e)
        {
            ButtonEdit edit = sender as ButtonEdit;
            MessageBox.Show("WorkPathEditValueChanged!" + edit.Text);
        }
    }
}
