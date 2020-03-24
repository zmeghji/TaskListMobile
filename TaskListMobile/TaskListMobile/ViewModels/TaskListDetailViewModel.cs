using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaskListMobileData.Enums;
using TaskListMobileData.Models;
using TaskListMobileData.Repositories;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace TaskListMobile.ViewModels
{
    public class TaskListDetailViewModel : ViewModelBase
    {
        private readonly ITaskListRepository _taskListRepository;
        private TaskList _model;
        public TaskList Model
        {
            get { return _model; }
            private set
            {
                _model = value;
                RaisePropertyChangedEvent(nameof(Model));
            }
        }
        public ICommand DisplayCreateDialogCommand => new Command(OnClickedCreateButton);
        private async void OnClickedCreateButton()
        {
            var newTaskName = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                .SetTitle("Create Task")
                .SetPlaceholder("Enter Name")
                .SetInputMode(InputType.Name));
            Model.TaskItems.Add(new TaskItem
            {
                Name = newTaskName.Text,
                Status = TaskItemStatus.Pending
            });
            _taskListRepository.Update(Model);
        }

        public TaskListDetailViewModel(ITaskListRepository taskListRepository, DateTime taskListDate)
        {
            _taskListRepository = taskListRepository;

            Model = _taskListRepository.Get(taskListDate);

            if (Model == null)
            {
                Model = new TaskList
                {
                    Date = taskListDate,
                    TaskItems = new List<TaskItem>
                    {
                        new TaskItem
                        {
                            Name = "Install Visual Studio",
                            Status = TaskItemStatus.Completed
                        },
                        new TaskItem
                        {
                            Name = "Install Windows SDK",
                            Status = TaskItemStatus.Pending
                        }
                    }
                };
                _taskListRepository.Create(Model);
            }
        }
    }
}
