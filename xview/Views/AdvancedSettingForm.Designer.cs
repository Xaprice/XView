namespace xview.Views
{
    partial class AdvancedSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonEdit_ConfigPath = new DevExpress.XtraEditors.ButtonEdit();
            this.ButtonEdit_VideoPath = new DevExpress.XtraEditors.ButtonEdit();
            this.ButtonEdit_ImagePath = new DevExpress.XtraEditors.ButtonEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEdit_ConfigPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEdit_VideoPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEdit_ImagePath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(409, 406);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ButtonEdit_ConfigPath);
            this.tabPage1.Controls.Add(this.ButtonEdit_VideoPath);
            this.tabPage1.Controls.Add(this.ButtonEdit_ImagePath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(401, 380);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "工作目录";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 146);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "配置文件目录:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "视频目录:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "图像目录：";
            // 
            // ButtonEdit_ConfigPath
            // 
            this.ButtonEdit_ConfigPath.Location = new System.Drawing.Point(92, 142);
            this.ButtonEdit_ConfigPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonEdit_ConfigPath.Name = "ButtonEdit_ConfigPath";
            this.ButtonEdit_ConfigPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ButtonEdit_ConfigPath.Properties.ReadOnly = true;
            this.ButtonEdit_ConfigPath.Size = new System.Drawing.Size(254, 20);
            this.ButtonEdit_ConfigPath.TabIndex = 2;
            // 
            // ButtonEdit_VideoPath
            // 
            this.ButtonEdit_VideoPath.Location = new System.Drawing.Point(92, 88);
            this.ButtonEdit_VideoPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonEdit_VideoPath.Name = "ButtonEdit_VideoPath";
            this.ButtonEdit_VideoPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ButtonEdit_VideoPath.Properties.ReadOnly = true;
            this.ButtonEdit_VideoPath.Size = new System.Drawing.Size(254, 20);
            this.ButtonEdit_VideoPath.TabIndex = 1;
            // 
            // ButtonEdit_ImagePath
            // 
            this.ButtonEdit_ImagePath.Location = new System.Drawing.Point(92, 38);
            this.ButtonEdit_ImagePath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ButtonEdit_ImagePath.Name = "ButtonEdit_ImagePath";
            this.ButtonEdit_ImagePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.ButtonEdit_ImagePath.Properties.ReadOnly = true;
            this.ButtonEdit_ImagePath.Size = new System.Drawing.Size(254, 20);
            this.ButtonEdit_ImagePath.TabIndex = 0;
            this.ButtonEdit_ImagePath.Tag = "";
            // 
            // AdvancedSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 410);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AdvancedSettingForm";
            this.Text = "高级设置";
            this.TopMost = true;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEdit_ConfigPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEdit_VideoPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEdit_ImagePath.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private DevExpress.XtraEditors.ButtonEdit ButtonEdit_ConfigPath;
        private DevExpress.XtraEditors.ButtonEdit ButtonEdit_VideoPath;
        private DevExpress.XtraEditors.ButtonEdit ButtonEdit_ImagePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}