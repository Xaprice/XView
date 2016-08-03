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
            this.components = new System.ComponentModel.Container();
            this.tbUnit = new System.Windows.Forms.TextBox();
            this.cbUnits = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.pixelLabel = new System.Windows.Forms.Label();
            this.tbNewScaleName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gcScales = new DevExpress.XtraGrid.GridControl();
            this.gvScales = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColScaleName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColPixels = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColUnitValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColUnitType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupBoxCreateNewScale = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemDeleteScale = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemClearScales = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemApplyScale = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gcScales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvScales)).BeginInit();
            this.groupBoxCreateNewScale.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbUnit
            // 
            this.tbUnit.Location = new System.Drawing.Point(26, 43);
            this.tbUnit.Name = "tbUnit";
            this.tbUnit.Size = new System.Drawing.Size(64, 21);
            this.tbUnit.TabIndex = 0;
            this.tbUnit.Text = "1";
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
            this.cbUnits.Location = new System.Drawing.Point(101, 43);
            this.cbUnits.Name = "cbUnits";
            this.cbUnits.Size = new System.Drawing.Size(96, 20);
            this.cbUnits.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(265, 81);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "添加";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pixelLabel
            // 
            this.pixelLabel.AutoSize = true;
            this.pixelLabel.Location = new System.Drawing.Point(28, 17);
            this.pixelLabel.Name = "pixelLabel";
            this.pixelLabel.Size = new System.Drawing.Size(41, 12);
            this.pixelLabel.TabIndex = 5;
            this.pixelLabel.Text = "label1";
            // 
            // tbNewScaleName
            // 
            this.tbNewScaleName.Location = new System.Drawing.Point(126, 81);
            this.tbNewScaleName.Name = "tbNewScaleName";
            this.tbNewScaleName.Size = new System.Drawing.Size(112, 21);
            this.tbNewScaleName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "新的标尺名：";
            // 
            // gcScales
            // 
            this.gcScales.Location = new System.Drawing.Point(13, 13);
            this.gcScales.MainView = this.gvScales;
            this.gcScales.Name = "gcScales";
            this.gcScales.Size = new System.Drawing.Size(411, 244);
            this.gcScales.TabIndex = 8;
            this.gcScales.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvScales});
            // 
            // gvScales
            // 
            this.gvScales.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColScaleName,
            this.gridColPixels,
            this.gridColUnitValue,
            this.gridColUnitType});
            this.gvScales.GridControl = this.gcScales;
            this.gvScales.Name = "gvScales";
            this.gvScales.OptionsBehavior.Editable = false;
            this.gvScales.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvScales.OptionsView.ShowGroupPanel = false;
            // 
            // gridColScaleName
            // 
            this.gridColScaleName.Caption = "标尺名";
            this.gridColScaleName.FieldName = "Name";
            this.gridColScaleName.Name = "gridColScaleName";
            this.gridColScaleName.Visible = true;
            this.gridColScaleName.VisibleIndex = 0;
            // 
            // gridColPixels
            // 
            this.gridColPixels.Caption = "像素";
            this.gridColPixels.FieldName = "Pixels";
            this.gridColPixels.Name = "gridColPixels";
            this.gridColPixels.Visible = true;
            this.gridColPixels.VisibleIndex = 1;
            // 
            // gridColUnitValue
            // 
            this.gridColUnitValue.Caption = "标度值";
            this.gridColUnitValue.FieldName = "UnitValue";
            this.gridColUnitValue.Name = "gridColUnitValue";
            this.gridColUnitValue.Visible = true;
            this.gridColUnitValue.VisibleIndex = 2;
            // 
            // gridColUnitType
            // 
            this.gridColUnitType.Caption = "标度单位";
            this.gridColUnitType.FieldName = "UnitTypeDisplayName";
            this.gridColUnitType.Name = "gridColUnitType";
            this.gridColUnitType.Visible = true;
            this.gridColUnitType.VisibleIndex = 3;
            // 
            // groupBoxCreateNewScale
            // 
            this.groupBoxCreateNewScale.Controls.Add(this.pixelLabel);
            this.groupBoxCreateNewScale.Controls.Add(this.tbUnit);
            this.groupBoxCreateNewScale.Controls.Add(this.label1);
            this.groupBoxCreateNewScale.Controls.Add(this.cbUnits);
            this.groupBoxCreateNewScale.Controls.Add(this.tbNewScaleName);
            this.groupBoxCreateNewScale.Controls.Add(this.btnOK);
            this.groupBoxCreateNewScale.Location = new System.Drawing.Point(13, 263);
            this.groupBoxCreateNewScale.Name = "groupBoxCreateNewScale";
            this.groupBoxCreateNewScale.Size = new System.Drawing.Size(411, 126);
            this.groupBoxCreateNewScale.TabIndex = 9;
            this.groupBoxCreateNewScale.TabStop = false;
            this.groupBoxCreateNewScale.Text = "添加新的标尺";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemApplyScale,
            this.toolStripSeparator1,
            this.menuItemDeleteScale,
            this.menuItemClearScales});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(161, 98);
            // 
            // menuItemDeleteScale
            // 
            this.menuItemDeleteScale.Name = "menuItemDeleteScale";
            this.menuItemDeleteScale.Size = new System.Drawing.Size(160, 22);
            this.menuItemDeleteScale.Text = "删除选中标尺";
            // 
            // menuItemClearScales
            // 
            this.menuItemClearScales.Name = "menuItemClearScales";
            this.menuItemClearScales.Size = new System.Drawing.Size(160, 22);
            this.menuItemClearScales.Text = "清空所有标尺";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // menuItemApplyScale
            // 
            this.menuItemApplyScale.Name = "menuItemApplyScale";
            this.menuItemApplyScale.Size = new System.Drawing.Size(160, 22);
            this.menuItemApplyScale.Text = "设置为当前标尺";
            // 
            // SetUnitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 396);
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.groupBoxCreateNewScale);
            this.Controls.Add(this.gcScales);
            this.Name = "SetUnitForm";
            this.Text = "标尺";
            this.Load += new System.EventHandler(this.SetUnitForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcScales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvScales)).EndInit();
            this.groupBoxCreateNewScale.ResumeLayout(false);
            this.groupBoxCreateNewScale.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbUnit;
        private System.Windows.Forms.ComboBox cbUnits;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label pixelLabel;
        private System.Windows.Forms.TextBox tbNewScaleName;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gcScales;
        private DevExpress.XtraGrid.Views.Grid.GridView gvScales;
        private DevExpress.XtraGrid.Columns.GridColumn gridColScaleName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColPixels;
        private DevExpress.XtraGrid.Columns.GridColumn gridColUnitValue;
        private DevExpress.XtraGrid.Columns.GridColumn gridColUnitType;
        private System.Windows.Forms.GroupBox groupBoxCreateNewScale;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteScale;
        private System.Windows.Forms.ToolStripMenuItem menuItemClearScales;
        private System.Windows.Forms.ToolStripMenuItem menuItemApplyScale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}