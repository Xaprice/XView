namespace xview
{
    partial class SelectCameraForm
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this._btnCancelSelect = new DevExpress.XtraEditors.SimpleButton();
            this._btnSelectCamera = new DevExpress.XtraEditors.SimpleButton();
            this._gridControlCameraList = new DevExpress.XtraGrid.GridControl();
            this._gridViewCameraList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this._gridColumnCameraName = new DevExpress.XtraGrid.Columns.GridColumn();
            this._gridColumnCameraInterface = new DevExpress.XtraGrid.Columns.GridColumn();
            this._gridColumnCameraStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            this._btnSelectCameraRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem6 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridControlCameraList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridViewCameraList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this._btnSelectCameraRefresh);
            this.layoutControl1.Controls.Add(this._btnCancelSelect);
            this.layoutControl1.Controls.Add(this._btnSelectCamera);
            this.layoutControl1.Controls.Add(this._gridControlCameraList);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(776, 175, 419, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(358, 330);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // _btnCancelSelect
            // 
            this._btnCancelSelect.Location = new System.Drawing.Point(271, 263);
            this._btnCancelSelect.Margin = new System.Windows.Forms.Padding(2);
            this._btnCancelSelect.Name = "_btnCancelSelect";
            this._btnCancelSelect.Size = new System.Drawing.Size(54, 22);
            this._btnCancelSelect.StyleController = this.layoutControl1;
            this._btnCancelSelect.TabIndex = 6;
            this._btnCancelSelect.Text = "取消";
            this._btnCancelSelect.Click += new System.EventHandler(this._btnCancelSelect_Click);
            // 
            // _btnSelectCamera
            // 
            this._btnSelectCamera.Location = new System.Drawing.Point(175, 263);
            this._btnSelectCamera.Margin = new System.Windows.Forms.Padding(2);
            this._btnSelectCamera.Name = "_btnSelectCamera";
            this._btnSelectCamera.Size = new System.Drawing.Size(56, 22);
            this._btnSelectCamera.StyleController = this.layoutControl1;
            this._btnSelectCamera.TabIndex = 5;
            this._btnSelectCamera.Text = "确定";
            this._btnSelectCamera.Click += new System.EventHandler(this._btnSelectCamera_Click);
            // 
            // _gridControlCameraList
            // 
            this._gridControlCameraList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this._gridControlCameraList.Location = new System.Drawing.Point(12, 12);
            this._gridControlCameraList.MainView = this._gridViewCameraList;
            this._gridControlCameraList.Margin = new System.Windows.Forms.Padding(2);
            this._gridControlCameraList.Name = "_gridControlCameraList";
            this._gridControlCameraList.Size = new System.Drawing.Size(334, 232);
            this._gridControlCameraList.TabIndex = 4;
            this._gridControlCameraList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._gridViewCameraList});
            // 
            // _gridViewCameraList
            // 
            this._gridViewCameraList.Appearance.FocusedCell.BackColor = System.Drawing.Color.RoyalBlue;
            this._gridViewCameraList.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this._gridViewCameraList.Appearance.FocusedCell.Options.UseBackColor = true;
            this._gridViewCameraList.Appearance.FocusedCell.Options.UseForeColor = true;
            this._gridViewCameraList.Appearance.FocusedRow.BackColor = System.Drawing.Color.RoyalBlue;
            this._gridViewCameraList.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._gridViewCameraList.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this._gridViewCameraList.Appearance.FocusedRow.Options.UseBackColor = true;
            this._gridViewCameraList.Appearance.FocusedRow.Options.UseFont = true;
            this._gridViewCameraList.Appearance.FocusedRow.Options.UseForeColor = true;
            this._gridViewCameraList.Appearance.SelectedRow.BackColor = System.Drawing.Color.RoyalBlue;
            this._gridViewCameraList.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this._gridViewCameraList.Appearance.SelectedRow.Options.UseBackColor = true;
            this._gridViewCameraList.Appearance.SelectedRow.Options.UseForeColor = true;
            this._gridViewCameraList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this._gridColumnCameraName,
            this._gridColumnCameraInterface,
            this._gridColumnCameraStatus});
            this._gridViewCameraList.GridControl = this._gridControlCameraList;
            this._gridViewCameraList.Name = "_gridViewCameraList";
            this._gridViewCameraList.OptionsView.ShowGroupPanel = false;
            // 
            // _gridColumnCameraName
            // 
            this._gridColumnCameraName.Caption = "相机名称";
            this._gridColumnCameraName.FieldName = "CameraName";
            this._gridColumnCameraName.Name = "_gridColumnCameraName";
            this._gridColumnCameraName.OptionsColumn.AllowEdit = false;
            this._gridColumnCameraName.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this._gridColumnCameraName.OptionsColumn.AllowMove = false;
            this._gridColumnCameraName.OptionsColumn.AllowShowHide = false;
            this._gridColumnCameraName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this._gridColumnCameraName.OptionsColumn.ReadOnly = true;
            this._gridColumnCameraName.OptionsColumn.ShowInCustomizationForm = false;
            this._gridColumnCameraName.Visible = true;
            this._gridColumnCameraName.VisibleIndex = 0;
            // 
            // _gridColumnCameraInterface
            // 
            this._gridColumnCameraInterface.Caption = "相机接口";
            this._gridColumnCameraInterface.FieldName = "InterfaceType";
            this._gridColumnCameraInterface.Name = "_gridColumnCameraInterface";
            this._gridColumnCameraInterface.OptionsColumn.AllowEdit = false;
            this._gridColumnCameraInterface.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this._gridColumnCameraInterface.OptionsColumn.AllowMove = false;
            this._gridColumnCameraInterface.OptionsColumn.AllowShowHide = false;
            this._gridColumnCameraInterface.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this._gridColumnCameraInterface.OptionsColumn.ReadOnly = true;
            this._gridColumnCameraInterface.OptionsColumn.ShowInCustomizationForm = false;
            this._gridColumnCameraInterface.Visible = true;
            this._gridColumnCameraInterface.VisibleIndex = 1;
            // 
            // _gridColumnCameraStatus
            // 
            this._gridColumnCameraStatus.Caption = "状态";
            this._gridColumnCameraStatus.FieldName = "CameraStatus";
            this._gridColumnCameraStatus.Name = "_gridColumnCameraStatus";
            this._gridColumnCameraStatus.OptionsColumn.AllowEdit = false;
            this._gridColumnCameraStatus.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this._gridColumnCameraStatus.OptionsColumn.AllowMove = false;
            this._gridColumnCameraStatus.OptionsColumn.AllowShowHide = false;
            this._gridColumnCameraStatus.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this._gridColumnCameraStatus.OptionsColumn.ReadOnly = true;
            this._gridColumnCameraStatus.OptionsColumn.ShowInCustomizationForm = false;
            this._gridColumnCameraStatus.Visible = true;
            this._gridColumnCameraStatus.VisibleIndex = 2;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.emptySpaceItem2,
            this.layoutControlItem3,
            this.emptySpaceItem3,
            this.emptySpaceItem5,
            this.layoutControlItem4,
            this.emptySpaceItem6,
            this.emptySpaceItem4});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(358, 330);
            this.layoutControlGroup1.Text = "Root";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._gridControlCameraList;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(338, 236);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(317, 251);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(21, 26);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._btnSelectCamera;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(163, 251);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(60, 26);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 251);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(50, 26);
            this.emptySpaceItem2.Text = "emptySpaceItem2";
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._btnCancelSelect;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(259, 251);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(58, 26);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.CustomizationFormText = "emptySpaceItem3";
            this.emptySpaceItem3.Location = new System.Drawing.Point(223, 251);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(36, 26);
            this.emptySpaceItem3.Text = "emptySpaceItem3";
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.CustomizationFormText = "emptySpaceItem4";
            this.emptySpaceItem4.Location = new System.Drawing.Point(107, 251);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(56, 26);
            this.emptySpaceItem4.Text = "emptySpaceItem4";
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.CustomizationFormText = "emptySpaceItem5";
            this.emptySpaceItem5.Location = new System.Drawing.Point(0, 236);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(338, 15);
            this.emptySpaceItem5.Text = "emptySpaceItem5";
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // _btnSelectCameraRefresh
            // 
            this._btnSelectCameraRefresh.Location = new System.Drawing.Point(62, 263);
            this._btnSelectCameraRefresh.Name = "_btnSelectCameraRefresh";
            this._btnSelectCameraRefresh.Size = new System.Drawing.Size(53, 22);
            this._btnSelectCameraRefresh.StyleController = this.layoutControl1;
            this._btnSelectCameraRefresh.TabIndex = 7;
            this._btnSelectCameraRefresh.Text = "刷新";
            this._btnSelectCameraRefresh.Click += new System.EventHandler(this._btnSelectCameraRefresh_Click);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._btnSelectCameraRefresh;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(50, 251);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(57, 26);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem6
            // 
            this.emptySpaceItem6.AllowHotTrack = false;
            this.emptySpaceItem6.CustomizationFormText = "emptySpaceItem6";
            this.emptySpaceItem6.Location = new System.Drawing.Point(0, 277);
            this.emptySpaceItem6.Name = "emptySpaceItem6";
            this.emptySpaceItem6.Size = new System.Drawing.Size(338, 33);
            this.emptySpaceItem6.Text = "emptySpaceItem6";
            this.emptySpaceItem6.TextSize = new System.Drawing.Size(0, 0);
            // 
            // SelectCameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 330);
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SelectCameraForm";
            this.Text = "相机选择";
            this.Load += new System.EventHandler(this.SelectCameraForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridControlCameraList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._gridViewCameraList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton _btnCancelSelect;
        private DevExpress.XtraEditors.SimpleButton _btnSelectCamera;
        private DevExpress.XtraGrid.GridControl _gridControlCameraList;
        private DevExpress.XtraGrid.Views.Grid.GridView _gridViewCameraList;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraGrid.Columns.GridColumn _gridColumnCameraName;
        private DevExpress.XtraGrid.Columns.GridColumn _gridColumnCameraInterface;
        private DevExpress.XtraGrid.Columns.GridColumn _gridColumnCameraStatus;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
        private DevExpress.XtraEditors.SimpleButton _btnSelectCameraRefresh;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem6;
    }
}