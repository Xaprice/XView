namespace xview.UserControls
{
    partial class ImageParaPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.imageParaGroup = new DevExpress.XtraEditors.GroupControl();
            this.btnRestoreImage = new System.Windows.Forms.Button();
            this.trackBarBlueGain = new xview.UserControls.TrackBarPanel();
            this.trackBarGreenGain = new xview.UserControls.TrackBarPanel();
            this.trackBarRedGain = new xview.UserControls.TrackBarPanel();
            this.trackBarGamma = new xview.UserControls.TrackBarPanel();
            this.trackBarContrast = new xview.UserControls.TrackBarPanel();
            this.trackBarSaturation = new xview.UserControls.TrackBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageParaGroup)).BeginInit();
            this.imageParaGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.CustomizationFormText = "R增益";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(182, 30);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(182, 30);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(196, 30);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "R增益";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(39, 14);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // imageParaGroup
            // 
            this.imageParaGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.imageParaGroup.Controls.Add(this.trackBarSaturation);
            this.imageParaGroup.Controls.Add(this.trackBarContrast);
            this.imageParaGroup.Controls.Add(this.trackBarGamma);
            this.imageParaGroup.Controls.Add(this.btnRestoreImage);
            this.imageParaGroup.Controls.Add(this.trackBarBlueGain);
            this.imageParaGroup.Controls.Add(this.trackBarGreenGain);
            this.imageParaGroup.Controls.Add(this.trackBarRedGain);
            this.imageParaGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageParaGroup.Location = new System.Drawing.Point(0, 0);
            this.imageParaGroup.Name = "imageParaGroup";
            this.imageParaGroup.Size = new System.Drawing.Size(220, 720);
            this.imageParaGroup.TabIndex = 0;
            this.imageParaGroup.Text = "图像参数设置";
            // 
            // btnRestoreImage
            // 
            this.btnRestoreImage.Location = new System.Drawing.Point(130, 33);
            this.btnRestoreImage.Name = "btnRestoreImage";
            this.btnRestoreImage.Size = new System.Drawing.Size(75, 23);
            this.btnRestoreImage.TabIndex = 3;
            this.btnRestoreImage.Text = "还原图像";
            this.btnRestoreImage.UseVisualStyleBackColor = true;
            this.btnRestoreImage.Click += new System.EventHandler(this.btnRestoreImage_Click);
            // 
            // trackBarBlueGain
            // 
            this.trackBarBlueGain.Location = new System.Drawing.Point(5, 149);
            this.trackBarBlueGain.Name = "trackBarBlueGain";
            this.trackBarBlueGain.ParaName = "B偏移";
            this.trackBarBlueGain.ParaValue = 0;
            this.trackBarBlueGain.Size = new System.Drawing.Size(203, 36);
            this.trackBarBlueGain.TabIndex = 2;
            this.trackBarBlueGain.Unit = "";
            // 
            // trackBarGreenGain
            // 
            this.trackBarGreenGain.Location = new System.Drawing.Point(5, 107);
            this.trackBarGreenGain.Name = "trackBarGreenGain";
            this.trackBarGreenGain.ParaName = "G偏移";
            this.trackBarGreenGain.ParaValue = 0;
            this.trackBarGreenGain.Size = new System.Drawing.Size(203, 36);
            this.trackBarGreenGain.TabIndex = 1;
            this.trackBarGreenGain.Unit = "";
            // 
            // trackBarRedGain
            // 
            this.trackBarRedGain.Location = new System.Drawing.Point(5, 66);
            this.trackBarRedGain.Name = "trackBarRedGain";
            this.trackBarRedGain.ParaName = "R偏移";
            this.trackBarRedGain.ParaValue = 0;
            this.trackBarRedGain.Size = new System.Drawing.Size(203, 36);
            this.trackBarRedGain.TabIndex = 0;
            this.trackBarRedGain.Unit = "";
            // 
            // trackBarGamma
            // 
            this.trackBarGamma.Location = new System.Drawing.Point(5, 191);
            this.trackBarGamma.Name = "trackBarGamma";
            this.trackBarGamma.ParaName = "伽马";
            this.trackBarGamma.ParaValue = 0;
            this.trackBarGamma.Size = new System.Drawing.Size(203, 36);
            this.trackBarGamma.TabIndex = 4;
            this.trackBarGamma.Unit = "";
            this.trackBarGamma.Load += new System.EventHandler(this.trackBarGamma_Load);
            // 
            // trackBarContrast
            // 
            this.trackBarContrast.Location = new System.Drawing.Point(5, 233);
            this.trackBarContrast.Name = "trackBarContrast";
            this.trackBarContrast.ParaName = "对比度";
            this.trackBarContrast.ParaValue = 0;
            this.trackBarContrast.Size = new System.Drawing.Size(203, 36);
            this.trackBarContrast.TabIndex = 5;
            this.trackBarContrast.Unit = "";
            this.trackBarContrast.Load += new System.EventHandler(this.trackBarContrast_Load);
            // 
            // trackBarSaturation
            // 
            this.trackBarSaturation.Location = new System.Drawing.Point(5, 275);
            this.trackBarSaturation.Name = "trackBarSaturation";
            this.trackBarSaturation.ParaName = "饱和度";
            this.trackBarSaturation.ParaValue = 0;
            this.trackBarSaturation.Size = new System.Drawing.Size(203, 36);
            this.trackBarSaturation.TabIndex = 6;
            this.trackBarSaturation.Unit = "";
            this.trackBarSaturation.Load += new System.EventHandler(this.trackBarSaturation_Load);
            // 
            // ImageParaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageParaGroup);
            this.Name = "ImageParaPanel";
            this.Size = new System.Drawing.Size(220, 720);
            this.Load += new System.EventHandler(this.ImageParaSetPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageParaGroup)).EndInit();
            this.imageParaGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.GroupControl imageParaGroup;
        private TrackBarPanel trackBarRedGain;
        private TrackBarPanel trackBarGreenGain;
        private TrackBarPanel trackBarBlueGain;
        private System.Windows.Forms.Button btnRestoreImage;
        private TrackBarPanel trackBarGamma;
        private TrackBarPanel trackBarContrast;
        private TrackBarPanel trackBarSaturation;
    }
}
