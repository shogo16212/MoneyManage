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
        private List<Category> deleteCategories = new List<Category>();
        private List<Category> addCategories = new List<Category>();
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
            try
            {
                var entry = entryComboBox.SelectedItem as Entory;
                if (entry == null) "タイプを入力してください".Err();

                if (nameTextBox.Text.IsNullOrEmpty()) "名前を入力してください".Err();

                if(db.Categories.Any(a => a.CategoryName == nameTextBox.Text)) "その名前はすでに使用されています".Err();

                var category = new Category { EntoryID = entry.EntoriesID, CategoryName = nameTextBox.Text, Entory = entry};

                addCategories.Add(category);
                categories.Add(category);
                
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var category = (sender as Button).Tag as Category;
            if(category == null) return;

            if (addCategories.Any(a => a.CategoryName == category.CategoryName))
            {
                var addedCategory = addCategories.First(a => a.CategoryName == category.CategoryName);
                addCategories.Remove(addedCategory);
            }

            deleteCategories.Add(category);

            categories.Remove(category);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                deleteCategories.ForEach(a => db.Categories.Remove(a));
                db.SaveChanges();
                addCategories.ForEach(a => db.Categories.Add(a));
                db.SaveChanges();
                "カテゴリーを登録しました".Show();
                Close();
            }
            catch (Exception ex)
            {
                ex.Message.Show();
            }
        }
    }
}
