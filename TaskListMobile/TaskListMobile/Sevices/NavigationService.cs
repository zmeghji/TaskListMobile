using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobile.Configuration;
using TaskListMobile.Pages;
using TaskListMobile.ViewModels;
using TaskListMobileData.Repositories;
using Xamarin.Forms;

namespace TaskListMobile.Sevices
{
    public interface INavigationService
    {
        void GoToTaskListDetails(DateTime? date);
        void GoToTaskListIndex();

    }
    public class NavigationService : INavigationService
    {
        public void GoToTaskListDetails(DateTime? date)
        {
            date = date ?? DateTime.Now.Date;
            var viewModel = new TaskListDetailViewModel(
                DIContainer.Resolve<ITaskListRepository>(),
                date.Value);
            Application.Current.MainPage = new TaskListDetail(viewModel);
        }

        public void GoToTaskListIndex()
        {
            var viewModel = new TaskListIndexViewModel(
                DIContainer.Resolve<ITaskListRepository>(),
                DateTime.Now.Date,
                DIContainer.Resolve<INavigationService>());
            Application.Current.MainPage = new TaskListIndex(viewModel);
        }
    }
}
