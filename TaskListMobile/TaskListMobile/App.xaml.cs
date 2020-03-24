using System;
using TaskListMobile.Configuration;
using TaskListMobile.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskListMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DIContainer.RegisterDependencies();

            MainPage = new NavigationPage(new TaskListIndex());

            //MainPage = new NavigationPage(new TaskListDetail());
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
