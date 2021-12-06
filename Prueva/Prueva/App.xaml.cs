using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prueva
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new principal.PrinUserView());
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
