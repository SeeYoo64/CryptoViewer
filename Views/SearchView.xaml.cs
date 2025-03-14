using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using CryptoViewer.Models;
using CryptoViewer.ViewModels;

namespace CryptoViewer.Views
{
    public partial class SearchView
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ListBoxItem_MouseDoubleClick triggered.");
            if (sender is ListBoxItem listBoxItem && listBoxItem.DataContext is SearchResult searchResult)
            {
                Debug.WriteLine($"Double-clicked on: {searchResult.Name} ({searchResult.Id})"); 
                if (DataContext is SearchViewModel viewModel)
                {
                    Debug.WriteLine("ViewModel found, executing NavigateToDetailsCommand.");
                    viewModel.NavigateToDetailsCommand.Execute(searchResult);
                }
                else
                {
                    Debug.WriteLine("ViewModel is null."); 
                }
            }
            else
            {
                Debug.WriteLine("No valid ListBoxItem or SearchResult found.");
            }
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ListBox_MouseDoubleClick triggered.");
            if (sender is ListBox listBox && listBox.SelectedItem is SearchResult searchResult)
            {
                Debug.WriteLine($"Double-clicked on: {searchResult.Name} ({searchResult.Id})");
                if (DataContext is SearchViewModel viewModel)
                {
                    Debug.WriteLine("ViewModel found, executing NavigateToDetailsCommand.");
                    viewModel.NavigateToDetailsCommand.Execute(searchResult);
                }
                else
                {
                    Debug.WriteLine("ViewModel is null.");
                }
            }
            else
            {
                Debug.WriteLine("No valid ListBox or SearchResult found.");
            }
        }


    }
}