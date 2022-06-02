
using Crowdin.Xamarin.Forms;
using TestMobileApp.Resources;
using TestMobileApp.Services;
using Xamarin.Forms;

namespace TestMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
            
            DynamicResourcesLoader.GlobalOptions.DistributionHash = "{insert_here}";
            
            string langCode = DynamicResourcesLoader.CurrentCulture.TwoLetterISOLanguageName;
            DynamicResourcesLoader.LoadStaticStrings(Translations.ResourceManager, Current.Resources);
            DynamicResourcesLoader.LoadCrowdinStrings($"Translations.{langCode}.resx", Current.Resources);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
