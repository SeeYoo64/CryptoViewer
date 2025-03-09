using System.Windows;
using System.Windows.Controls;

namespace CryptoViewer.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button clicked!");
        }
    }
}