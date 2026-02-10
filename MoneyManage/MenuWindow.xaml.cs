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
    /// MenuWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MenuWindow : Window
    {
        DB db = new DB();
        User _user;
        DateTime dateTime = DateTime.Today;
        public MenuWindow()
        {
            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            db = new DB();

            _user = db.Users.Find(Common.UserID);

            loginUserTextBlock.Text = $"ログイン：{_user.UserName}";

            if (_user.IsAdmin)
            {
                userAddButton.Visibility = Visibility.Visible;
            }

            monthTextBlock.Text = dateTime.ToString("yyyy-MM");

            dataGrid.ItemsSource = _user.Transactions.ToList().Where(a => a.Date.ToString("yyyy-MM") == dateTime.ToString("yyyy-MM")).ToList();

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            new SubmitWindow().ShowDialog();
            Refresh();
        }

        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            new StatisticsWindow().ShowDialog();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Common.UserID = 0;
            Close();
        }

        private void userAddButton_Click(object sender, RoutedEventArgs e)
        {
            new UserRegisterWindow().ShowDialog();
        }

        private void userEditButton_Click(object sender, RoutedEventArgs e)
        {
            new UserRegisterWindow(Common.UserID).ShowDialog();
        }

        private void preventButton_Click(object sender, RoutedEventArgs e)
        {
            dateTime = dateTime.AddMonths(-1);
            Refresh();
        }

        private void afterButton_Click(object sender, RoutedEventArgs e)
        {
            dateTime = dateTime.AddMonths(1);
            Refresh();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var transaction = dataGrid.SelectedItem as Transaction;
            if (transaction == null) return;
            new SubmitWindow(transaction.TransactionID).ShowDialog();
            Refresh();
        }
    }
}
