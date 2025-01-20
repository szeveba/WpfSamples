using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Basics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string FilePath = "todoItems.txt";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Save()
        {
            var op = new List<string>();
            foreach (string item in LB.Items)
            {
                op.Add(item);
            }
            File.WriteAllLines(FilePath, op);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (LB.SelectedItem == null)
            {
                LB.Items.Add(TB.Text);
            }
            else
            {
                int idx = LB.SelectedIndex;
                LB.Items.RemoveAt(idx);
                LB.Items.Insert(idx, TB.Text);
            }
            TB.Clear(); // TB.Text = "";
            Save();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LB.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) LB.Items.RemoveAt(LB.SelectedIndex);
            }
            Save();
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
                TB.Text = (string)LB.SelectedItem;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FilePath))
            {
                var lines = File.ReadAllLines(FilePath);
                foreach (var item in lines)
                {
                    LB.Items.Add(item);
                }
            }
        }
    }
}