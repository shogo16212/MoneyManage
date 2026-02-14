using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// CategoryManageWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CategoryManageWindow : Window
    {
        private DB db = new DB();
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private List<Category> delteCategories = new List<Category>();
        public CategoryManageWindow()
        {
            InitializeComponent();

            entryComboBox.ItemsSource = db.Entories.ToList();
            entryComboBox.SelectedItem = db.Entories.ToList().FirstOrDefault();

            db.Categories.ToList().ForEach(a =>
            {
                categories.Add(a);
            });

            dataGrid.ItemsSource = categories;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
