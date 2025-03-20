using System.Diagnostics;
using System.Windows.Controls;

namespace CryptoViewer.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Debug.WriteLine("MainView: Constructor called.");
        }
    }
}