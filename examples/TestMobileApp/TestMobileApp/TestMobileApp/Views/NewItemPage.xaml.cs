
using TestMobileApp.Models;
using TestMobileApp.ViewModels;
using Xamarin.Forms;

namespace TestMobileApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}