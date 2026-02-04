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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyManage
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DB db = new DB();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            db = new DB();
            try
            {
                var user = db.Users.ToList().FirstOrDefault(a => a.UserName == nameTextBox.Text && a.PasswordHash == passBox.Password.CreateHash());
                if (user == null) "ログインに失敗しました。".Err();
                Common.UserID = user.UserID;
                new MenuWindow().Show();
                Close();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }
    }
}
