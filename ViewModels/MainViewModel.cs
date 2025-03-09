using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoViewer.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "CryptoViewer Main Page";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainViewModel()
        {
        }


    }

}
