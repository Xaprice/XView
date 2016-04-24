namespace xview.Forms
{
    partial class SetUnitForm
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
            this.tbUnit = new System.Windows.Forms.TextBox();
            this.cbUnits = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.msgLabel = new System.Windows.Forms.Label();
            this.pixelLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbUnit
            // 
            this.tbUnit.Location = new System.Drawing.Point(24, 44);
            this.tbUnit.Name = "tbUnit";
            this.tbUnit.Size = new System.Drawing.Size(69, 21);
            this.tbUnit.TabIndex = 0;
            this.tbUnit.Text = "1";
            this.tbUnit.TextChanged += new System.EventHandler(this.tbUnit_TextChanged);
            // 
            // cbUnits
            // 
            this.cbUnits.FormattingEnabled = true;
            this.cbUnits.Items.AddRange(new object[] {
            "像素(px)",
            "厘米(cm)",
            "毫米(mm)",
            "微米(um)",
            "纳米(nm)"});
            this.cbUnits.Location = new System.Drawing.Point(99, 44);
            this.cbUnits.Name = "cbUnits";
            this.cbUnits.Size = new System.Drawing.Size(101, 20);
            this.cbUnits.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(28, 120);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(120, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // msgLabel
            // 
            this.msgLabel.AutoSize = true;
            this.msgLabel.Location = new System.Drawing.Point(29, 77);
            this.msgLabel.Name = "msgLabel";
            this.msgLabel.Size = new System.Drawing.Size(47, 12);
            this.msgLabel.TabIndex = 4;
            this.msgLabel.Text = "消息...";
            // 
            // pixelLabel
            // 
            this.pixelLabel.AutoSize = true;
            this.pixelLabel.Location = new System.Drawing.Point(31, 18);
            this.pixelLabel.Name = "pixelLabel";
            this.pixelLabel.Size = new System.Drawing.Size(41, 12);
            this.pixelLabel.TabIndex = 5;
            this.pixelLabel.Text = "label1";
            // 
            // SetUnitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 155);
            this.Controls.Add(this.pixelLabel);
            this.Controls.Add(this.msgLabel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbUnits);
            this.Controls.Add(this.tbUnit);
            this.Name = "SetUnitForm";
            this.Text = "定标";
            this.Load += new System.EventHandler(this.SetUnitForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUnit;
        private System.Windows.Forms.ComboBox cbUnits;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label msgLabel;
        private System.Windows.Forms.Label pixelLabel;
    }
}