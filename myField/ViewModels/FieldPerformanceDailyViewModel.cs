using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Charting;
using Telerik.Reporting.Charting.Styles;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace TestGUI.ViewModels
{
    public class FieldPerformanceDailyViewModel : ViewModelBase
    {
        //private ChartSeriesCombineMode _barCombineMode = ChartSeriesCombineMode.Cluster;
        //private Orientation _chartOrientation = Orientation.Vertical;
        //private bool _isShowLabelsEnabled = true;
        //private bool _showLabels = false;

        //private double _gapLength = 0.2d;
        //private double _axisMaxValue = 20000d;
        //private double _axisStep = 5000d;
        //private string _axisTitle = "PROFIT (USD)";
        //private string _axisLabelFormat = "N0";
        //private GridLineVisibility _majorLinesVisibility = GridLineVisibility.Y;

        public FieldPerformanceDailyViewModel()
        {
        }
        public IEnumerable<FieldPerformance> Daily { get; set; }
    }
}
