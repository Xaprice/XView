using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;

namespace xview.UserControls
{
    public partial class HistogramPanel : UserControl
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HistogramPanel));

        private static readonly int _bins = 255;
        private List<SeriesPoint> _leftBoundLineSeriesPts = new List<SeriesPoint>();
        private List<SeriesPoint> _rightBoundLineSeriesPts = new List<SeriesPoint>();
        private List<SeriesPoint> _tranformLineSeriesPts = new List<SeriesPoint>();
        private List<SeriesPoint> _blueChannelSeriesPts = new List<SeriesPoint>();
        private List<SeriesPoint> _greenChannelSeriesPts = new List<SeriesPoint>();
        private List<SeriesPoint> _redChannelSeriesPts = new List<SeriesPoint>();

        public ImageForm ImageForm { get; set; }

        public HistogramPanel()
        {
            InitializeComponent();
        }

        public void Init(Image<Bgr, Byte> img)
        {
            try
            {
	            //初始化三条辅助线
	            for (int i = 0; i < _bins+1; i++)
	            {
	                _leftBoundLineSeriesPts.Add(new SeriesPoint(_bins, (double)i / _bins));
	                _rightBoundLineSeriesPts.Add(new SeriesPoint(0, (double)i / _bins));
	                _tranformLineSeriesPts.Add(new SeriesPoint(i, (double)i / _bins));
	            }
	            _chartControl.Series["_leftBoundLine"].Points.AddRange(_leftBoundLineSeriesPts.ToArray());
	            _chartControl.Series["_rightBoundLine"].Points.AddRange(_rightBoundLineSeriesPts.ToArray());
	            _chartControl.Series["_tranformLine"].Points.AddRange(_tranformLineSeriesPts.ToArray());
	
	            //初始化RGB直方图的曲线
	            DenseHistogram blueHistogram = new DenseHistogram(255, new RangeF(0, 255));
	            DenseHistogram greenHistogram = new DenseHistogram(255, new RangeF(0, 255));
	            DenseHistogram redHistogram = new DenseHistogram(255, new RangeF(0, 255));
	            blueHistogram.Calculate(new IImage[] { img.Split()[0]}, false, null);
	            greenHistogram.Calculate(new IImage[] { img.Split()[1] }, false, null);
	            redHistogram.Calculate(new IImage[] { img.Split()[2] }, false, null);
                //blueHistogram.Normalize(1.0);
                //greenHistogram.Normalize(1.0);
                //redHistogram.Normalize(1.0);
	            //int bBiblueHistogramnNumber = histogram.BinDimension[0].Size;
                float minVal, maxVal1, maxVal2, maxVal3;
                int[] minLocs, maxLocs;
                blueHistogram.MinMax(out minVal, out maxVal1, out minLocs, out maxLocs);
                greenHistogram.MinMax(out minVal, out maxVal2, out minLocs, out maxLocs);
                redHistogram.MinMax(out minVal, out maxVal3, out minLocs, out maxLocs);
                double maxVal = Math.Max(Math.Max(maxVal1, maxVal2), maxVal3);

	            for (int i = 0; i < _bins; i++)
	            {
	                double blueVal = CvInvoke.cvQueryHistValue_1D(blueHistogram, i);
	                double greenVal = CvInvoke.cvQueryHistValue_1D(greenHistogram, i);
	                double redVal = CvInvoke.cvQueryHistValue_1D(redHistogram, i);
                    _blueChannelSeriesPts.Add(new SeriesPoint(i, blueVal / maxVal));
                    _greenChannelSeriesPts.Add(new SeriesPoint(i, greenVal / maxVal));
                    _redChannelSeriesPts.Add(new SeriesPoint(i, redVal / maxVal));
	            }
                _chartControl.Series["_blueChannelLine"].Points.AddRange(_blueChannelSeriesPts.ToArray());
                _chartControl.Series["_greenChannelLine"].Points.AddRange(_greenChannelSeriesPts.ToArray());
                _chartControl.Series["_redChannelLine"].Points.AddRange(_redChannelSeriesPts.ToArray());
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _trackBarLow_Scroll(object sender, EventArgs e)
        {
            try
            {
	            if (_trackBarLow.Value < _trackBarHigh.Value)
	            {
	                _leftBoundLineSeriesPts.ForEach(x => x.Argument = _trackBarLow.Value.ToString());
	                UpdateTransformLinePts(_trackBarLow.Value, _trackBarHigh.Value);
	                RefreshLine("_leftBoundLine");
	                RefreshLine("_tranformLine");
	            }
	            else
	            {
	                _trackBarLow.Value = _trackBarHigh.Value;
	            }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _trackBarHigh_Scroll(object sender, EventArgs e)
        {
            try
            {
	            if (_trackBarLow.Value < _trackBarHigh.Value)
	            {
	                _rightBoundLineSeriesPts.ForEach(x => x.Argument = _trackBarHigh.Value.ToString());
	                UpdateTransformLinePts(_trackBarLow.Value, _trackBarHigh.Value);
	                RefreshLine("_rightBoundLine");
	                RefreshLine("_tranformLine");
	            }
	            else
	            {
	                _trackBarHigh.Value = _trackBarLow.Value;
	            }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void _trackBarLow_MouseUp(object sender, MouseEventArgs e)
        {
            if (ImageForm != null)
            {
                ImageForm.SetThreshold(_trackBarLow.Value, true);
            }
        }

        private void _trackBarHigh_MouseUp(object sender, MouseEventArgs e)
        {
            if (ImageForm != null)
            {
                ImageForm.SetThreshold(_trackBarHigh.Value, false);
            }
        }

        private void RefreshLine(string lineName)
        {
            try
            {
	            _chartControl.Series[lineName].Points.Clear();
	            List<SeriesPoint> pointList = new List<SeriesPoint>();
	            switch (lineName)
	            {
	                case "_leftBoundLine":
	                    pointList = _leftBoundLineSeriesPts;
	                    break;
	                case "_rightBoundLine":
	                    pointList = _rightBoundLineSeriesPts;
	                    break;
	                case "_tranformLine":
	                    pointList = _tranformLineSeriesPts;
	                    break;
	                case "_blueChannelLine":
	                    pointList = _blueChannelSeriesPts;
	                    break;
	                case "_greenChannelLine":
	                    pointList = _greenChannelSeriesPts;
	                    break;
	                case "_redChannelLine":
	                    pointList = _redChannelSeriesPts;
	                    break;
	                default:
	                    break;
	            }
	            _chartControl.Series[lineName].Points.AddRange(pointList.ToArray());
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void UpdateTransformLinePts(int low, int high)
        {
            try
            {
	            if (low <= high)
	            {
	                _tranformLineSeriesPts.Clear();
	                for (int i = 0; i < _bins; i++)
	                {
	                    if (i < low)
	                        _tranformLineSeriesPts.Add(new SeriesPoint(i, 0));
	                    else if (i > high)
	                        _tranformLineSeriesPts.Add(new SeriesPoint(i, 1.0));
	                    else
	                    {
	                        double val = 1.0 - (double)(high - i)/(double)(high - low);
	                        _tranformLineSeriesPts.Add(new SeriesPoint(i, val));
	                    }
	                }
	            }
	            else
	            {
	                logger.Warn("low value is larger than high value!");
	            }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
