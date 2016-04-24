﻿using DrawTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xview.UserControls;

namespace xview.Draw
{
    public interface IDrawForm
    {
        void SetActiveDrawTool(ImageDrawBox.DrawToolType drawToolType);

        void DeleteDrawObjects(bool deleteAll);

        void SelectAllDrawObjects();

        List<MeasureListItem> GetMeasureListData();

        List<MeasureStatisticItem> GetMeasureStatisticData();

        void SetDrawingMode(xview.UserControls.ImageDrawBox.DrawingMode drawingMode);

        void SetUnit(double pixelsPerUm);

    }
}
