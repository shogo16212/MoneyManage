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

            var user = db.Users.Find(Common.UserID);

            loginUserTextBlock.Text = $"ログイン：{user.UserName}";

            var users = db.Users.ToList();
            users.Insert(0, new User { UserID = 0, UserName = "All" });
            searchComboBox.ItemsSource = users;
            searchComboBox.SelectedItem = users.FirstOrDefault();

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

            var today = DateTime.Today;
            var firstDate = new DateTime(today.Year, today.Month, 01);
            var lastDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            for (var dt = firstDate; dt <= lastDate; dt = dt.AddDays(1))
            {
                var total = db.Transactions.ToList().Where(a => a.UserID == Common.UserID && a.Date.Date == dt.Date && a.Category.EntoryID == 1).Sum(a => a.Amount);
                var getTotal = db.Transactions.ToList().Where(a => a.UserID == Common.UserID && a.Date.Date == dt.Date && a.Category.EntoryID == 2).Sum(a => a.Amount);
                columnChart.Series[0].Points.AddXY(dt.ToShortDateString(), total);
                columnChart.Series[1].Points.AddXY(dt.ToShortDateString(), getTotal);
            }
        }

        private void searchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = searchComboBox.SelectedItem as User;
            if (user == null) return;
            var firstDate = db.Transactions.ToList().Min(a => a.Date).Date;
            var yearMonths = new List<string>();

            if (user.UserID != 0)
            {
                firstDate = user.Transactions.Min(a => a.Date).Date;
            }

            for (var dt = firstDate; dt <= DateTime.Now; dt = dt.AddMonths(1))
            {
                yearMonths.Add(dt.ToString("yyyy-MM"));
            }

            yearMonths.Insert(0, "All");
            searchyearMonthComboBox.ItemsSource = yearMonths;
            searchyearMonthComboBox.SelectedItem = yearMonths.FirstOrDefault();
        }

        private void searchyearMonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                var user = searchComboBox.SelectedItem as User;
                var yearMonth = searchyearMonthComboBox.SelectedItem as string;
                if (user == null && yearMonth == null) return;
                var filterList = Filter(db.Transactions.ToList(), user.UserID, yearMonth);
                historyDataGrid.ItemsSource = filterList;

                titleTextBlock.Text = $"{user.UserName}さんの詳細";

                expenTextBlock.Text = filterList.Where(a => a.Category.EntoryID == 1).Sum(a => a.Amount).ToString();
                incomeTextBlock.Text = filterList.Where(a => a.Category.EntoryID == 2).Sum(a => a.Amount).ToString();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private List<Transaction> Filter(List<Transaction> transactions, int userId, string yearMonth)
        {
            if (userId != 0)
            {
                return Filter(transactions.Where(a => a.UserID == userId).ToList(), 0, yearMonth);
            }

            if (yearMonth != "All")
            {
                return Filter(transactions.Where(a => a.Date.ToString("yyyy-MM") == yearMonth).ToList(), userId, "All");
            }
            return transactions;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
