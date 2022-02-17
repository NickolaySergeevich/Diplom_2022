using System.ComponentModel;

using DiplomApp.ViewModels;

using Xamarin.Forms;

namespace DiplomApp.Views
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