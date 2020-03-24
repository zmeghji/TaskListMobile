using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using TaskListMobile.Pages;
using TaskListMobile.Sevices;
using TaskListMobileData.Models;
using TaskListMobileData.Repositories;
using Xamarin.Forms;

namespace TaskListMobile.ViewModels
{
    public class TaskListIndexViewModel : ViewModelBase
    {
        private ObservableCollection<TaskList> _model;
        private readonly ITaskListRepository _taskListRepository;
        private readonly INavigationService _navigationService;

        public TaskListIndexViewModel(
            ITaskListRepository taskListRepository,
            DateTime taskListDate,
            INavigationService navigationService)
        {
            _taskListRepository = taskListRepository;
            _navigationService = navigationService;
            _model = new ObservableCollection<TaskList>(
                _taskListRepository.Get(taskListDate, null));
        }

        public ICommand GoToTaskListCommand => new Command<TaskList>(OnClickTaskList);
        private async void OnClickTaskList(TaskList taskList)
        {
            _navigationService.GoToTaskListDetails(taskList.Date);
        }
        public ICommand DisplayCreateDialogCommand => new Command(OnClickedCreateButton);
        private async void OnClickedCreateButton()
        {
            var dateString = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                .SetTitle("Create Task List")
                .SetPlaceholder("Enter Date (MM-dd-yyyy)")
                .SetInputMode(InputType.Name));
            var date = DateTime.Parse(dateString.Text);
            _navigationService.GoToTaskListDetails(date);
        }
        public ObservableCollection<TaskList> TaskLists
        {
            get => _model;
            private set
            {
                _model = value;
                RaisePropertyChangedEvent(nameof(TaskLists));
            }
        }
    }
}
