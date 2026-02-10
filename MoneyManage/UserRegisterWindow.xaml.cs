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
    /// UserRegisterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UserRegisterWindow : Window
    {
        private DB db = new DB();
        private User user = new User();
        public UserRegisterWindow(int userId = 0)
        {
            InitializeComponent();

            if (userId != 0)
            {
                user = db.Users.ToList().Find(a => a.UserID == userId);
                nameTextBox.Text = user.UserName;
                adminCheckBox.IsChecked = user.IsAdmin;
                loginUserTextBlock.Text = $"ログイン：{user.UserName}";
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nameTextBox.Text.IsNullOrEmpty() && passBox.Password.IsNullOrEmpty() && cPassBox.Password.IsNullOrEmpty())
                {
                    "入力されていない項目があります。".Err();
                }
                if (passBox.Password != cPassBox.Password)
                {
                    "入力されたパスワードが一致しません。".Err();
                }
                if (adminCheckBox.IsChecked == true)
                {
                    if (MessageBox.Show("管理者として登録すると家計簿登録に加え、\n様々な機能を使用することができます。\nよろしいですか？", "注意", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.Cancel)
                    {
                        "再度利用者情報を入力してください。".Err();
                    }
                }

                user.UserName = nameTextBox.Text;
                user.PasswordHash = passBox.Password.CreateHash();
                user.IsAdmin = adminCheckBox.IsChecked.Value;

                if (db.Users.ToList().Any(a => a.UserID != user.UserID && a.UserName == user.UserName))
                {
                    "そのユーザー名は既に登録されています。\n違うユーザー名を設定してください。".Err();
                }

                if (user.UserID == 0)
                {
                    db.Users.Add(user);
                }

                db.SaveChanges();

                "ユーザーを登録しました。".Show();
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
    }
}
