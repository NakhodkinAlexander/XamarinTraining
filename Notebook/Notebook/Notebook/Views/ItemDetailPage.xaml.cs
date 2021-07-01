using Notebook.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Notebook.Views
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