namespace xview.UserControls
{
    partial class MeasurePanel
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
            this._xtraTabMeasure = new DevExpress.XtraTab.XtraTabControl();
            this._xtraTabPageMeasureList = new DevExpress.XtraTab.XtraTabPage();
            this._gridControlMeasureList = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcToolType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcArea = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPerimeter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRadius = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcAngle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this._xtraTabPageMeasureStatistic = new DevExpress.XtraTab.XtraTabPage();
            this._gridControlMeasureStatistic = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcToolType2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStatisticType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcStatisticCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcAverageValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMinValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcMaxValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUnit2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this._xtraTabMeasure)).BeginInit();
            this._xtraTabMeasure.SuspendLayout();
            this._xtraTabPageMeasureList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridControlMeasureList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this._xtraTabPageMeasureStatistic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridControlMeasureStatistic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // _xtraTabMeasure
            // 
            this._xtraTabMeasure.Dock = System.Windows.Forms.DockStyle.Fill;
            this._xtraTabMeasure.Location = new System.Drawing.Point(8, 8);
            this._xtraTabMeasure.Margin = new System.Windows.Forms.Padding(2);
            this._xtraTabMeasure.Name = "_xtraTabMeasure";
            this._xtraTabMeasure.Padding = new System.Windows.Forms.Padding(2);
            this._xtraTabMeasure.SelectedTabPage = this._xtraTabPageMeasureList;
            this._xtraTabMeasure.Size = new System.Drawing.Size(584, 384);
            this._xtraTabMeasure.TabIndex = 0;
            this._xtraTabMeasure.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this._xtraTabPageMeasureList,
            this._xtraTabPageMeasureStatistic});
            // 
            // _xtraTabPageMeasureList
            // 
            this._xtraTabPageMeasureList.Controls.Add(this._gridControlMeasureList);
            this._xtraTabPageMeasureList.Name = "_xtraTabPageMeasureList";
            this._xtraTabPageMeasureList.Size = new System.Drawing.Size(578, 355);
            this._xtraTabPageMeasureList.Text = "列表";
            // 
            // _gridControlMeasureList
            // 
            this._gridControlMeasureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridControlMeasureList.Location = new System.Drawing.Point(0, 0);
            this._gridControlMeasureList.MainView = this.gridView1;
            this._gridControlMeasureList.Name = "_gridControlMeasureList";
            this._gridControlMeasureList.Size = new System.Drawing.Size(578, 355);
            this._gridControlMeasureList.TabIndex = 0;
            this._gridControlMeasureList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcToolType,
            this.gcLength,
            this.gcArea,
            this.gcPerimeter,
            this.gcRadius,
            this.gcAngle,
            this.gcUnit});
            this.gridView1.GridControl = this._gridControlMeasureList;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gcToolType
            // 
            this.gcToolType.Caption = "元素类型";
            this.gcToolType.FieldName = "ToolType";
            this.gcToolType.Name = "gcToolType";
            this.gcToolType.Visible = true;
            this.gcToolType.VisibleIndex = 0;
            // 
            // gcLength
            // 
            this.gcLength.Caption = "长度";
            this.gcLength.FieldName = "Length";
            this.gcLength.Name = "gcLength";
            this.gcLength.Visible = true;
            this.gcLength.VisibleIndex = 1;
            // 
            // gcArea
            // 
            this.gcArea.Caption = "面积";
            this.gcArea.FieldName = "Area";
            this.gcArea.Name = "gcArea";
            this.gcArea.Visible = true;
            this.gcArea.VisibleIndex = 2;
            // 
            // gcPerimeter
            // 
            this.gcPerimeter.Caption = "周长";
            this.gcPerimeter.FieldName = "Perimeter";
            this.gcPerimeter.Name = "gcPerimeter";
            this.gcPerimeter.Visible = true;
            this.gcPerimeter.VisibleIndex = 3;
            // 
            // gcRadius
            // 
            this.gcRadius.Caption = "半径";
            this.gcRadius.FieldName = "Radius";
            this.gcRadius.Name = "gcRadius";
            this.gcRadius.Visible = true;
            this.gcRadius.VisibleIndex = 4;
            // 
            // gcAngle
            // 
            this.gcAngle.Caption = "角度";
            this.gcAngle.FieldName = "Angle";
            this.gcAngle.Name = "gcAngle";
            this.gcAngle.Visible = true;
            this.gcAngle.VisibleIndex = 5;
            // 
            // gcUnit
            // 
            this.gcUnit.Caption = "单位";
            this.gcUnit.FieldName = "Unit";
            this.gcUnit.Name = "gcUnit";
            this.gcUnit.Visible = true;
            this.gcUnit.VisibleIndex = 6;
            // 
            // _xtraTabPageMeasureStatistic
            // 
            this._xtraTabPageMeasureStatistic.Controls.Add(this._gridControlMeasureStatistic);
            this._xtraTabPageMeasureStatistic.Name = "_xtraTabPageMeasureStatistic";
            this._xtraTabPageMeasureStatistic.Size = new System.Drawing.Size(578, 355);
            this._xtraTabPageMeasureStatistic.Text = "统计";
            // 
            // _gridControlMeasureStatistic
            // 
            this._gridControlMeasureStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridControlMeasureStatistic.Location = new System.Drawing.Point(0, 0);
            this._gridControlMeasureStatistic.MainView = this.gridView2;
            this._gridControlMeasureStatistic.Name = "_gridControlMeasureStatistic";
            this._gridControlMeasureStatistic.Size = new System.Drawing.Size(578, 355);
            this._gridControlMeasureStatistic.TabIndex = 0;
            this._gridControlMeasureStatistic.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcToolType2,
            this.gcStatisticType,
            this.gcStatisticCount,
            this.gcAverageValue,
            this.gcMinValue,
            this.gcMaxValue,
            this.gcUnit2});
            this.gridView2.GridControl = this._gridControlMeasureStatistic;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.ReadOnly = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // gcToolType2
            // 
            this.gcToolType2.Caption = "测量工具";
            this.gcToolType2.FieldName = "ToolType";
            this.gcToolType2.Name = "gcToolType2";
            this.gcToolType2.Visible = true;
            this.gcToolType2.VisibleIndex = 0;
            // 
            // gcStatisticType
            // 
            this.gcStatisticType.Caption = "统计类别";
            this.gcStatisticType.FieldName = "StatisticType";
            this.gcStatisticType.Name = "gcStatisticType";
            this.gcStatisticType.Visible = true;
            this.gcStatisticType.VisibleIndex = 1;
            // 
            // gcStatisticCount
            // 
            this.gcStatisticCount.Caption = "数量";
            this.gcStatisticCount.FieldName = "Count";
            this.gcStatisticCount.Name = "gcStatisticCount";
            this.gcStatisticCount.Visible = true;
            this.gcStatisticCount.VisibleIndex = 2;
            // 
            // gcAverageValue
            // 
            this.gcAverageValue.Caption = "平均值";
            this.gcAverageValue.FieldName = "AverageValue";
            this.gcAverageValue.Name = "gcAverageValue";
            this.gcAverageValue.Visible = true;
            this.gcAverageValue.VisibleIndex = 3;
            // 
            // gcMinValue
            // 
            this.gcMinValue.Caption = "最小值";
            this.gcMinValue.FieldName = "MinValue";
            this.gcMinValue.Name = "gcMinValue";
            this.gcMinValue.Visible = true;
            this.gcMinValue.VisibleIndex = 4;
            // 
            // gcMaxValue
            // 
            this.gcMaxValue.Caption = "最大值";
            this.gcMaxValue.FieldName = "MaxValue";
            this.gcMaxValue.Name = "gcMaxValue";
            this.gcMaxValue.Visible = true;
            this.gcMaxValue.VisibleIndex = 5;
            // 
            // gcUnit2
            // 
            this.gcUnit2.Caption = "单位";
            this.gcUnit2.FieldName = "Unit";
            this.gcUnit2.Name = "gcUnit2";
            this.gcUnit2.Visible = true;
            this.gcUnit2.VisibleIndex = 6;
            // 
            // MeasurePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._xtraTabMeasure);
            this.Name = "MeasurePanel";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(600, 400);
            ((System.ComponentModel.ISupportInitialize)(this._xtraTabMeasure)).EndInit();
            this._xtraTabMeasure.ResumeLayout(false);
            this._xtraTabPageMeasureList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridControlMeasureList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this._xtraTabPageMeasureStatistic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridControlMeasureStatistic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl _xtraTabMeasure;
        private DevExpress.XtraTab.XtraTabPage _xtraTabPageMeasureList;
        private DevExpress.XtraTab.XtraTabPage _xtraTabPageMeasureStatistic;
        private DevExpress.XtraGrid.GridControl _gridControlMeasureList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl _gridControlMeasureStatistic;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gcToolType;
        private DevExpress.XtraGrid.Columns.GridColumn gcLength;
        private DevExpress.XtraGrid.Columns.GridColumn gcArea;
        private DevExpress.XtraGrid.Columns.GridColumn gcPerimeter;
        private DevExpress.XtraGrid.Columns.GridColumn gcRadius;
        private DevExpress.XtraGrid.Columns.GridColumn gcAngle;
        private DevExpress.XtraGrid.Columns.GridColumn gcUnit;
        private DevExpress.XtraGrid.Columns.GridColumn gcToolType2;
        private DevExpress.XtraGrid.Columns.GridColumn gcStatisticType;
        private DevExpress.XtraGrid.Columns.GridColumn gcStatisticCount;
        private DevExpress.XtraGrid.Columns.GridColumn gcAverageValue;
        private DevExpress.XtraGrid.Columns.GridColumn gcMinValue;
        private DevExpress.XtraGrid.Columns.GridColumn gcMaxValue;
        private DevExpress.XtraGrid.Columns.GridColumn gcUnit2;
    }
}
