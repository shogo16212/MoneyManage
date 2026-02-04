using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyManage
{
    /// <summary>
    /// StatisticsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private DB db = new DB();
        public StatisticsWindow()
        {
            InitializeComponent();

            ColumnChartRefresh();
        }

        private void ColumnChartRefresh()
        {
            columnChart.Titles.Clear();
            columnChart.ChartAreas.Clear();
            columnChart.Series.Clear();
            columnChart.Legends.Clear();

            columnChart.Titles.Add("日ごとの支出");
            columnChart.ChartAreas.Add("a");
            columnChart.Series.Add("支出合計");
            columnChart.Series.Add("収入合計");
            columnChart.Legends.Add("a");

            columnChart.ChartAreas[0].AxisX.Interval = 1;

            columnChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            columnChart.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            columnChart.Series[0].IsValueShownAsLabel = true;
            columnChart.Series[1].IsValueShownAsLabel = true;

            var today= DateTime.Today;
            var firstDate = new DateTime(today.Year, today.Month, 01);
            var lastDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year,today.Month));
            for (var dt = firstDate; dt <= lastDate; dt = dt.AddDays(1))
            {
                var total=db.Transactions.ToList().Where(a => a.UserID == Common.UserID && a.Date.Date == dt.Date && a.Type == "支出").Sum(a => a.Amount);
                var getTotal=db.Transactions.ToList().Where(a => a.UserID == Common.UserID && a.Date.Date == dt.Date && a.Type == "収入").Sum(a => a.Amount);
                columnChart.Series[0].Points.AddXY(dt.ToShortDateString(),total);
                columnChart.Series[1].Points.AddXY(dt.ToShortDateString(),getTotal);
            }
        }


    }
}
