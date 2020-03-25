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
using System.Linq;
using System.ComponentModel;

namespace TaskListMobile.ViewModels
{
    public class TaskListDetailViewModel : ViewModelBase
    {
        private readonly ITaskListRepository _taskListRepository;
        public ObservableCollection<TaskItemViewModel> TaskItems { get; set; }
        public TaskListDetailViewModel(ITaskListRepository taskListRepository, DateTime taskListDate)
        {
            _taskListRepository = taskListRepository;

            var taskList = _taskListRepository.Get(taskListDate);

            if (taskList == null)
            {
                taskList = new TaskList
                {
                    Date = taskListDate,
                };
                _taskListRepository.Create(taskList);
            }
            Id = taskList.Id;
            Date = taskList.Date;
            TaskItems = new ObservableCollection<TaskItemViewModel>(
                taskList.TaskItems.Select(t => new TaskItemViewModel(t, (s, a) => TaskListChanged()))
                );

            TaskItems.CollectionChanged+= (s, a) =>TaskListChanged();
        }
        private void TaskListChanged()
        {
            _taskListRepository.Update(new TaskList
            {
                Id = Id,
                Date = Date,
                TaskItems = TaskItems.Select(t => new TaskItem
                {
                    Name = t.Name,
                    Status = (t.IsCompleted ? TaskItemStatus.Completed : TaskItemStatus.Pending)
                }).ToList()
            });
        }

        public ICommand DeleteTaskItemCommand => new Command<string>(OnClickDeleteButton);
        private async void OnClickDeleteButton(string taskItemName)
        {
            var taskItemToRemove = TaskItems.First(s => s.Name == taskItemName);
            TaskItems.Remove(taskItemToRemove);
        }
        public ICommand DisplayCreateDialogCommand => new Command(OnClickedCreateButton);
        private async void OnClickedCreateButton()
        {
            var newTaskName = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                .SetTitle("Create Task")
                .SetPlaceholder("Enter Name")
                .SetInputMode(InputType.Name));

            var newTaskItem = new TaskItem
            {
                Name = newTaskName.Text,
                Status = TaskItemStatus.Pending
            };
            TaskItems.Add(new TaskItemViewModel(newTaskItem, (s, a) => TaskListChanged()));
        }

        public int Id { get; }
        public DateTime Date { get; }

    }
    public class TaskItemViewModel : ViewModelBase
    {
        private TaskItem _model;
        public TaskItemViewModel(TaskItem model, System.ComponentModel.PropertyChangedEventHandler eventHandler)
        {
            _model = model;
            PropertyChanged += eventHandler;
        }
        public string Name { get { return _model.Name; } }
        public bool IsCompleted
        {
            get { return _model.Status == TaskItemStatus.Completed; }
            set
            {
                if (value)
                {
                    _model.Status = TaskItemStatus.Completed;
                }
                else
                {
                    _model.Status = TaskItemStatus.Pending;
                }
                RaisePropertyChangedEvent(nameof(_model.Status));
            }
        }
    }
}
