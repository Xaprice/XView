using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xview.Forms
{
    //显示有问题
    public partial class WaitForm : Form
    {
        private static WaitForm _instance = null;

        private static readonly object lockHelper = new object();

        public static WaitForm GetInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new WaitForm();
                    }
                }
            }
            return _instance;
        }

        private WaitForm()
        {
            InitializeComponent();
        }

        public string Description
        {
            get { return _progressPanel.Description; }
            set { _progressPanel.Description = value; }
        }

        public string Caption
        {
            get { return _progressPanel.Caption; }
            set { _progressPanel.Caption = value; }
        }
    }
}
