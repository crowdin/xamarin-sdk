
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestMobileApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(() => Browser.OpenAsync("https://crowdin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}