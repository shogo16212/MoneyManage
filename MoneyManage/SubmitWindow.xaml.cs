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
    /// SubmitWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SubmitWindow : Window
    {
        private DB db = new DB();
        private Transaction _transaction = new Transaction();
        public SubmitWindow(int transactionId = 0)
        {
            InitializeComponent();

            typeComboBoxr.ItemsSource = db.Entories.ToList();
            typeComboBoxr.SelectedItem = db.Entories.ToList().FirstOrDefault();

            datePicker.SelectedDate = DateTime.Now;

            if (transactionId != 0)
            {
                _transaction = db.Transactions.ToList().Single(a => a.TransactionID == transactionId);

                datePicker.SelectedDate = _transaction.Date;
                typeComboBoxr.SelectedItem = db.Entories.ToList().FirstOrDefault(a => a.EntoriesID == _transaction.CategoryID);
                categoryComboBoxr.SelectedItem = _transaction.Category;
                amountTextBox.Text = _transaction.Amount.ToString();
                memoTextBox.Text = _transaction.Memo;

            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var entory = typeComboBoxr.SelectedItem as Entory;
                var category = categoryComboBoxr.SelectedItem as Category;
                var amountResult = int.TryParse(amountTextBox.Text, out int amount);
                if (datePicker.SelectedDate == null || entory == null || category == null || !amountResult)
                {
                    "入力されていない項目があります。".Err();
                }

                _transaction = new Transaction
                {
                    CategoryID = category.CategoryID,
                    UserID = Common.UserID,
                    Amount = amount,
                    Date = datePicker.SelectedDate.Value,
                    Memo = memoTextBox.Text,
                };
                if (_transaction.TransactionID == 0)
                {
                    db.Transactions.Add(_transaction);
                }

                db.SaveChanges();
                "登録が完了しました。".Show();
                Close();

            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void typeComboBoxr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var entory = typeComboBoxr.SelectedItem as Entory;
            if (entory == null) return;
            categoryComboBoxr.ItemsSource = entory.Categories.ToList();
            categoryComboBoxr.SelectedItem = entory.Categories.ToList().FirstOrDefault();
        }
    }
}
