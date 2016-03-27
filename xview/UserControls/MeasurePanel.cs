using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawTools;

namespace xview.UserControls
{
    public partial class MeasurePanel : UserControl
    {
        public MeasurePanel()
        {
            InitializeComponent();
        }

        public void SetMeasureData(List<MeasureListItem> measureList, List<MeasureStatisticItem> statisticList)
        {
            if (measureList != null)
            {
                this._gridControlMeasureList.DataSource = measureList;
                this._gridControlMeasureStatistic.DataSource = statisticList;
            }
        }


    }
}
