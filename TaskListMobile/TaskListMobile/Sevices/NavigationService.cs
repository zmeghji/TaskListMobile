using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskListMobile.Configuration;
using TaskListMobile.Pages;
using TaskListMobile.ViewModels;
using TaskListMobileData.Repositories;
using Xamarin.Forms;

namespace TaskListMobile.Sevices
{
    public interface INavigationService
    {
        Task GoToTaskListDetails(DateTime? date);
    }
    public class NavigationService : INavigationService
    {
        public async Task GoToTaskListDetails(DateTime? date)
        {
            date = date ?? DateTime.Now.Date;
            var viewModel = new TaskListDetailViewModel(
                DIContainer.Resolve<ITaskListRepository>(),
                date.Value);
            Application.Current.MainPage = new NavigationPage(new TaskListDetail(viewModel));
        }

    }
}
