using SinglePageApp.View;
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

namespace SinglePageApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame_Click(sender, e);
        }

        private void SecondFrame_Click(object sender, RoutedEventArgs e)
        {
            var secondPage = new SecondaryPage();
            MainFrame.Content = secondPage;
        }

        private void MainFrame_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = new MainPage();
            MainFrame.Content = mainPage;
        }

        private void ShowWindow_Click(object sender, RoutedEventArgs e)
        {
            var w = new SampleWindow();
            w.Show();
        }

        private void ShowDialog_Click(object sender, RoutedEventArgs e)
        {
            var w = new SampleWindow();
            w.ShowDialog();
        }
    }
}