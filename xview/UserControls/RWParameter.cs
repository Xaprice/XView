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
    public partial class RWParameter : UserControl
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RWParameter));

        public RWParameter()
        {
            InitializeComponent();
        }

        public void Init()
        {
            try
            {
	            XCamera cam = XCamera.GetInstance();
	            emDSParameterTeam paraTeam = emDSParameterTeam.PARAMETER_TEAM_LAST;
	            if (cam.GetCurrentParameterTeam(out paraTeam))
	            {
	                SetRadioGroupSelectInnerParas(paraTeam);
	            }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void SetRadioGroupSelectInnerParas(emDSParameterTeam paraTeam)
        {
            switch (paraTeam)
            {
                case emDSParameterTeam.PARAMETER_TEAM_A:
                    _radioGroupSelectInnerParas.SelectedIndex = 0;
                    break;
                case emDSParameterTeam.PARAMETER_TEAM_B:
                    _radioGroupSelectInnerParas.SelectedIndex = 1;
                    break;
                case emDSParameterTeam.PARAMETER_TEAM_C:
                    _radioGroupSelectInnerParas.SelectedIndex = 2;
                    break;
                case emDSParameterTeam.PARAMETER_TEAM_D:
                    _radioGroupSelectInnerParas.SelectedIndex = 3;
                    break;
                default:
                    break;
            }
        }

        private emDSParameterTeam GetRadioGroupSelectInnerParas()
        {
            emDSParameterTeam paraTeam = emDSParameterTeam.PARAMETER_TEAM_DEFAULT;
            switch(_radioGroupSelectInnerParas.SelectedIndex)
            {
                case 0:
                    paraTeam = emDSParameterTeam.PARAMETER_TEAM_A;
                    break;
                case 1:
                    paraTeam = emDSParameterTeam.PARAMETER_TEAM_B;
                    break;
                case 2:
                    paraTeam = emDSParameterTeam.PARAMETER_TEAM_C;
                    break;
                case 3:
                    paraTeam = emDSParameterTeam.PARAMETER_TEAM_D;
                    break;
                default:
                    break;
            }
            return paraTeam;
        }

        private void _btnSaveToInnerPara_Click(object sender, EventArgs e)
        {
            try
            {
	            emDSParameterTeam paraTeam = GetRadioGroupSelectInnerParas();
	            if (XCamera.GetInstance().SaveParameter(paraTeam))
	            {
	                XMessageDialog.Info("保存参数成功！");
	            }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _btnLoadInnerPara_Click(object sender, EventArgs e)
        {
            try
            {
                emDSParameterTeam paraTeam = GetRadioGroupSelectInnerParas();
                if (XCamera.GetInstance().LoadParameter(paraTeam))
                {
                    XMessageDialog.Info("加载参数成功！");
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _btnLoadDefaultInnerPara_Click(object sender, EventArgs e)
        {
            try
            {
                if (XCamera.GetInstance().LoadParameter(emDSParameterTeam.PARAMETER_TEAM_DEFAULT))
                {
                    XMessageDialog.Info("加载默认参数成功！");
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _btnLoadFromIni_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDlg = new OpenFileDialog();
            //openFileDlg.InitialDirectory = ProgramConstants.DEFAULT_INI_PATH + "\\";
            //openFileDlg.Filter = "ini files |*.ini";
            //openFileDlg.RestoreDirectory = true;
            //openFileDlg.Multiselect = false;
            //if (openFileDlg.ShowDialog() == DialogResult.OK)
            //{
            //    //OpenImageFromFile(openFileDlg.FileName);
            //}
            //XCamera.GetInstance().LoadParameterFromIni();
        }

        private void _btnSaveToIni_Click(object sender, EventArgs e)
        {
            //XCamera.GetInstance().SaveParameterToIni();
        }


    }
}
