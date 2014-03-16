using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using TestGUI.ViewModels;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.ChartView;
using System.Data.Entity.Core.Objects;
using System.Drawing;

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private FieldContext _db;
        public MainWindow()
        {
            _db = new FieldContext();
            InitializeComponent();
        }

        private void FieldChooserLoaded(object sender, RoutedEventArgs e)
        {
            var tree = sender as TreeView;
            foreach (var field in _db.Fields.Include(f => f.Wells))
            {
                var itemField = new TreeViewItem();
                itemField.Header = field.Name;
                //itemField.Name = field.Id.ToString();
                foreach (var well in field.Wells)
                {
                    var itemWell = new TreeViewItem();
                    itemWell.Header = well.Name;
                    itemField.Items.Add(itemWell);

                }
                tree.Items.Add(itemField);
            }
        }

        // TODO What's the best way to store Guid in TreeViewItem?
        private void FieldSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            var item = tree.SelectedValue as TreeViewItem;
            if (item.Parent.GetType() == typeof(TreeView))
            {
                DrawFieldOverview(item.Header.ToString());
            }
            else
            {
                DrawWellOverview(item.Header.ToString());
            }
        }

        private void DrawFieldOverview(string name)
        {
            FieldOverview.Visibility = System.Windows.Visibility.Visible;
            WellOverview.Visibility = System.Windows.Visibility.Hidden;
            LoadField(name);
        }

        private void LoadField(string name)
        {
            //FieldMap.Source = new ImageSource();
            //throw new NotImplementedException();
            DrawFieldMap(name);
            LoadDailyFieldPerformance(name);
            LoadMonthlyFieldPerformance(name);
            LoadPerformanceByWells(name);
        }

        private void DrawFieldMap(string name)
        {
            var imagePath = _db.Fields.Where(f => f.Name.Equals(name)).First().MapImage;
            imagePath = ConfigurationManager.AppSettings["ImageRootPath"].ToString() + imagePath;
            FieldMap.Background = new ImageBrush(new BitmapImage(new Uri(imagePath)));

            FieldMap.Children.Clear();
            var wells = _db.Wells.Where(f => f.Field.Name.Equals(name));
            foreach (var well in wells)
            {
                var ell = new Ellipse()
                {
                    Height = 10,
                    Width = 10,
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0,0,0))
                };
                Canvas.SetTop(ell, well.CoordinateY);
                Canvas.SetLeft(ell, well.CoordinateX);
                FieldMap.Children.Add(ell);
            }
        }

        private void LoadPerformanceByWells(string name)
        {
            var series = new PieSeries
            {
                ShowLabels = true
            };
            series.LabelDefinitions.Add(new ChartSeriesLabelDefinition
            {
                Binding = new PropertyNameDataPointBinding { PropertyName = "WellName" }
            });
            series.ValueBinding = new GenericDataPointBinding<FieldPerformanceByWellsViewModel, double> { ValueSelector = fp => fp.Production };
            series.ItemsSource = GetPerformanceByWells(name);
            PerformanceByWells.Series.Clear();
            PerformanceByWells.Series.Add(series);
        }

        private IEnumerable<FieldPerformanceByWellsViewModel> GetPerformanceByWells(string name)
        {
            return from w in _db.Facts
                   where w.Well.Field.Name.Equals(name)
                   group w by w.Well into f
                   select new FieldPerformanceByWellsViewModel
                   {
                       FieldName = name,
                       WellName = f.Key.Name,
                       Production = f.Sum(ff => ff.Production)
                   };
        }

        private void LoadMonthlyFieldPerformance(string name)
        {
            var series = new BarSeries { ShowLabels = true };
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            series.ValueBinding = new GenericDataPointBinding<FieldPerformance, double>() { ValueSelector = fp => fp.Performance };
            series.ItemsSource = GetFieldPerformanceMonthly(name);
            MonthlyFieldPerformance.Series.Clear();
            MonthlyFieldPerformance.Series.Add(series);
        }

        private IEnumerable<FieldPerformance> GetFieldPerformanceMonthly(string name)
        {
            var field = from f in _db.Facts
                        where f.Well.Field.Name.Equals(name)
                        group f by new { month = f.Date.Month, year = f.Date.Year } into d
                        select new
                        {
                            FieldName = name,
                            Performance = d.Sum(ff => ff.Production),
                            Date = d.Key.month
                            //Date = string.Format("{0}-{1}", d.Key.month, d.Key.year)
                        };
            var ret = new List<FieldPerformance>();
            foreach (var w in field.ToList().OrderBy(ff => ff.Date))
            {
                ret.Add(new FieldPerformance
                {
                    FieldName = w.FieldName,
                    Performance = w.Performance,
                    Date = w.Date.ToString()
                });
            }
            return ret;
            //var grouped = from p in posts
            //              group p by new { month = p.Create.Month, year = p.Create.Year } into d
            //              select new { dt = string.Format("{0}/{1}", d.Key.month, d.Key.year), count = d.Count() };
        }

        private void LoadDailyFieldPerformance(string name)
        {
            var series = new LineSeries { ShowLabels = true };
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            series.ValueBinding = new GenericDataPointBinding<FieldPerformance, double>() { ValueSelector = fp => fp.Performance };
            series.ItemsSource = GetFieldPerformanceDaily(name);
            DailyFieldPerformance.Series.Clear();
            DailyFieldPerformance.Series.Add(series);
        }

        private void DrawWellOverview(string name)
        {
            FieldOverview.Visibility = System.Windows.Visibility.Hidden;
            WellOverview.Visibility = System.Windows.Visibility.Visible;

            DrawWellProduction(name);
            DrawWellPressure(name);
            //DrawWellPieChart(name);
        }

        private void DrawWellPieChart(string name)
        {
            throw new NotImplementedException();
        }

        private void DrawWellPressure(string name)
        {
            var series = new LineSeries { ShowLabels = true };
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            series.ValueBinding = new GenericDataPointBinding<WellViewModel, double>() { ValueSelector = f => f.Pressure };
            series.ItemsSource = GetWellProduction(name);
            WellPressure.Series.Clear();
            WellPressure.Series.Add(series);
        }

        private void DrawWellProduction(string name)
        {
            var series = new LineSeries { ShowLabels = true };
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            series.ValueBinding = new GenericDataPointBinding<WellViewModel, double>() { ValueSelector = f => f.Production };
            series.ItemsSource = GetWellProduction(name);
            WellProduction.Series.Clear();
            WellProduction.Series.Add(series);
        }

        private IEnumerable<WellViewModel> GetWellProduction(string name)
        {
            var well = (from f in _db.Facts
                        where f.Well.Name.Equals(name)
                        select f).ToList();
            var ret = new List<WellViewModel>();
            foreach (var w in well)
            {
                ret.Add(new WellViewModel
                {
                    Name = name,
                    Production = w.Production,
                    Pressure = w.Pressure,
                    Date = w.Date.ToString("dd.MM.yyyy")
                });
            }
            return ret;
        }

        private IEnumerable<FieldPerformance> GetFieldPerformanceDaily(string name)
        {
            var field = from w in _db.Facts
                        where w.Well.Field.Name.Equals(name)
                        select new
                        {
                            FieldName = name,
                            Performance = w.Production,
                            Date = w.Date
                        };
            var ret = new List<FieldPerformance>();
            foreach (var w in field.ToList().OrderBy(ff => ff.Date))
            {
                ret.Add(new FieldPerformance
                {
                    FieldName = w.FieldName,
                    Performance = w.Performance,
                    Date = w.Date.ToString("dd.MM.yyyy")
                });
            }
            return ret;
        }

    }
}
