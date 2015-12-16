namespace xview
{
    partial class ImageForm
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
            this.components = new System.ComponentModel.Container();
            this._imageBox = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this._imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _imageBox
            // 
            this._imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._imageBox.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.PanAndZoom;
            this._imageBox.Location = new System.Drawing.Point(0, 0);
            this._imageBox.Name = "_imageBox";
            this._imageBox.Size = new System.Drawing.Size(664, 472);
            this._imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._imageBox.TabIndex = 2;
            this._imageBox.TabStop = false;
            // 
            // ChildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(664, 472);
            this.Controls.Add(this._imageBox);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ChildForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChildForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._imageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox _imageBox;


    }
}
