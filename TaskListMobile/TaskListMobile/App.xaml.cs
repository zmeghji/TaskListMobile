using System;
using TaskListMobile.Configuration;
using TaskListMobile.Pages;
using TaskListMobile.Sevices;
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
            DIContainer.Resolve<INavigationService>().GoToTaskListIndex();
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
