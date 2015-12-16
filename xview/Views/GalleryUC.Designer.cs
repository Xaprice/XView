namespace xview.Views
{
    partial class GalleryUC
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
            this._galleryControl = new DevExpress.XtraBars.Ribbon.GalleryControl();
            this.galleryControlClient1 = new DevExpress.XtraBars.Ribbon.GalleryControlClient();
            this.buttonEditGalleryImagePath = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this._galleryControl)).BeginInit();
            this._galleryControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditGalleryImagePath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // _galleryControl
            // 
            this._galleryControl.Controls.Add(this.galleryControlClient1);
            this._galleryControl.DesignGalleryGroupIndex = 0;
            this._galleryControl.DesignGalleryItemIndex = 0;
            this._galleryControl.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // galleryControlGallery1
            // 
            this._galleryControl.Gallery.ItemClick += new DevExpress.XtraBars.Ribbon.GalleryItemClickEventHandler(this.galleryControlGallery1_ItemClick);
            this._galleryControl.Location = new System.Drawing.Point(0, 0);
            this._galleryControl.Margin = new System.Windows.Forms.Padding(2);
            this._galleryControl.Name = "_galleryControl";
            this._galleryControl.Size = new System.Drawing.Size(220, 510);
            this._galleryControl.TabIndex = 0;
            this._galleryControl.Text = "图像栏";
            // 
            // galleryControlClient1
            // 
            this.galleryControlClient1.GalleryControl = this._galleryControl;
            this.galleryControlClient1.Location = new System.Drawing.Point(2, 2);
            this.galleryControlClient1.Margin = new System.Windows.Forms.Padding(2);
            this.galleryControlClient1.Size = new System.Drawing.Size(199, 506);
            // 
            // buttonEditGalleryImagePath
            // 
            this.buttonEditGalleryImagePath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonEditGalleryImagePath.EditValue = "";
            this.buttonEditGalleryImagePath.Location = new System.Drawing.Point(0, 490);
            this.buttonEditGalleryImagePath.Name = "buttonEditGalleryImagePath";
            this.buttonEditGalleryImagePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEditGalleryImagePath.Size = new System.Drawing.Size(220, 20);
            this.buttonEditGalleryImagePath.TabIndex = 1;
            this.buttonEditGalleryImagePath.ToolTip = "切换图库目录";
            this.buttonEditGalleryImagePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEditGalleryImagePath_ButtonClick);
            // 
            // GalleryUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonEditGalleryImagePath);
            this.Controls.Add(this._galleryControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GalleryUC";
            this.Size = new System.Drawing.Size(220, 510);
            ((System.ComponentModel.ISupportInitialize)(this._galleryControl)).EndInit();
            this._galleryControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEditGalleryImagePath.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.GalleryControl _galleryControl;
        private DevExpress.XtraBars.Ribbon.GalleryControlClient galleryControlClient1;
        private DevExpress.XtraEditors.ButtonEdit buttonEditGalleryImagePath;
    }
}
