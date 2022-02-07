using Riots.Sudoku.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Riots.Sudoku.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}