using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xview.UserControls
{
    public partial class TrackBarPanel : UserControl
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParaName 
        {
            get { return this.nameLabel.Text; }
            set { this.nameLabel.Text = value; }
        }

        /// <summary>
        /// 参数值
        /// </summary>
        public int ParaValue
        {
            get { return this.trackBar.Value; }
            set
            { 
                this.trackBar.Value = value;
            }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get { return this.unitLabel.Text; }
            set { this.unitLabel.Text = value; }
        }

        /// <summary>
        /// 参数发生变化事件
        /// </summary>
        /// <param name="e"></param>
        public delegate void ParaValueChangeHandler(SingleDataEventArgs<int> e);
        public event ParaValueChangeHandler ParaValueChangeEvent;
        private void OnParaValueChangeEvent(SingleDataEventArgs<int> e)
        {
            if (ParaValueChangeEvent != null)
            {
                ParaValueChangeEvent(e);
            }
        }

        public void SetRange(int min, int max)
        {
            this.trackBar.SetRange(min, max);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public TrackBarPanel()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //默认没有单位
            this.unitLabel.Text = "";
            UpdateValueLabel();
        }

        private void UpdateValueLabel()
        {
            this.valueLabel.Text = this.trackBar.Value.ToString();
        }

        private void TrackBarPanel_Load(object sender, EventArgs e)
        {
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            UpdateValueLabel();
            SingleDataEventArgs<int> arg = new SingleDataEventArgs<int>();
            arg.Data = this.trackBar.Value;
            OnParaValueChangeEvent(arg);
        }



    }
}
