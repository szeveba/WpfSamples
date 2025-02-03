using System;
using System.Collections.Generic;
using System.IO;
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

namespace Basics
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        private const string FilePath = "todo.csv";
        public MainWindow2()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (LB.SelectedItem != null)
            {
                var selectedItem = (TodoItem)LB.SelectedItem;
                selectedItem.Value = TB.Text;
                selectedItem.DeadLine = DP.SelectedDate;
                LB.Items.Refresh();
            }
            else
            {
                var item = new TodoItem
                {
                    Value = TB.Text,
                    CompletionDate = CB.IsChecked == true ? DateTime.Now : null,
                    DeadLine = DP.SelectedDate
                };
                LB.Items.Add(item);
            }
            TB.Clear();
            CB.IsChecked = false;
            DP.SelectedDate = null;
            Save();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Delete confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                LB.Items.Remove(LB.SelectedItem);
                Save();
            }
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            BTN_Save.IsEnabled = TB.Text != "";
        }

        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BTN_Delete.IsEnabled = LB.SelectedItem != null;
            if (LB.SelectedItem != null)
            {
                var selectedItem = (TodoItem)LB.SelectedItem;
                TB.Text = selectedItem.Value;
                DP.SelectedDate = selectedItem.DeadLine;
                CB.IsChecked = selectedItem.CompletionDate.HasValue;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FilePath))
            {
                var lines = File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    LB.Items.Add(TodoItem.FromCsv(line));
                };
            }
        }

        private void Save()
        {
            var op = new List<string>();
            foreach (TodoItem item in LB.Items)
            {
                op.Add(item.ToCsv());
            }
            File.WriteAllLines(FilePath, op);
        }

        private void CB_Click(object sender, RoutedEventArgs e)
        {
            if (LB.SelectedItem != null)
            {
                ((TodoItem)LB.SelectedItem).CompletionDate = CB.IsChecked == true
                    ? DateTime.Now
                    : null;
            }
        }
    }
}
