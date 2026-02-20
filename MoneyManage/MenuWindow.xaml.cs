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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            else
            {
                onlyCheckBox.Visibility = Visibility.Collapsed;
                onlyCheckBox.IsChecked = true;
            }

            monthTextBlock.Text = dateTime.ToString("yyyy-MM");

            dataGrid.ItemsSource = _user.Transactions.ToList().Where(a => a.Date.ToString("yyyy-MM") == dateTime.ToString("yyyy-MM")).ToList();


            if (listRadioButton.IsChecked == true)
            {
                dataGrid.Visibility = Visibility.Visible;
                detailGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                dataGrid.Visibility = Visibility.Collapsed;
                detailGrid.Visibility = Visibility.Visible;

            }

            detailStackPanel.Children.Clear();

            if (onlyCheckBox.IsChecked == true)
            {
                var stackPanel = new StackPanel();
                _user.Transactions.Where(a => a.Date.ToString("yyyy-MM") == dateTime.ToString("yyyy-MM")).ToList().GroupBy(a => a.Category).ToList().ForEach(a =>
                {
                    stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
                    stackPanel.Children.Add(new TextBlock
                    {
                        Width = 150,
                        Text = a.Key.CategoryName,
                    });

                    stackPanel.Children.Add(new TextBlock
                    {
                        Width = 150,
                        Text = $"{a.Sum(b => b.Amount)}円",
                    });

                    detailStackPanel.Children.Add(stackPanel);
                });

                stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

                stackPanel.Children.Add(new TextBlock
                {
                    Width = 150,
                    Text = "合計"
                });

                stackPanel.Children.Add(new TextBlock
                {
                    Width = 150,
                    Text = $"{_user.Transactions.Where(a => a.Date.ToString("yyyy-MM") == dateTime.ToString("yyyy-MM")).ToList().Sum(b => b.Amount)}円",
                });
                detailStackPanel.Children.Add(stackPanel);
            }
            else
            {
                var stackPanel = new StackPanel();
                db.Categories.ToList().ForEach(a =>
                {
                    stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

                    stackPanel.Children.Add(new TextBlock
                    {
                        Width = 150,
                        Text = a.CategoryName,
                    });

                    stackPanel.Children.Add(new TextBlock
                    {
                        Width = 150,
                        Text = $"{a.Transactions.Where(b => b.Date.ToString("yyyy-MM") == dateTime.ToString("yyyy-MM")).ToList().Sum(b => b.Amount)}円",
                    });

                    detailStackPanel.Children.Add(stackPanel);
                });

                stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

                stackPanel.Children.Add(new TextBlock
                {
                    Width = 150,
                    Text = "合計"
                });

                stackPanel.Children.Add(new TextBlock
                {
                    Width = 150,
                    // tostringで怒られる
                    Text = $"{db.Transactions.Where(a => a.Date.ToString("yyyy-MM") == dateTime.ToString("yyyy-MM")).ToList().Sum(b => b.Amount)}円",
                });

                detailStackPanel.Children.Add(stackPanel);
            }
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

        private void categoryManageButton_Click(object sender, RoutedEventArgs e)
        {
            new CategoryManageWindow().ShowDialog();
            Refresh();
        }

        private void listRadioButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void detailRadioButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void onlyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
