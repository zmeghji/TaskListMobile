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

        private ObservableCollection<TaskItemViewModel> _taskItems;
        public ObservableCollection<TaskItemViewModel> FilteredTaskItems
        {
            get
            {
                if (ShowCompleted)
                {
                    return _taskItems;
                }
                else
                {
                    return new ObservableCollection<TaskItemViewModel>(_taskItems.Where(t => !t.IsCompleted));
                }
            }
            private set { _taskItems = value;}
        }

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
            _taskItems = new ObservableCollection<TaskItemViewModel>(
                taskList.TaskItems.Select(t => new TaskItemViewModel(t, (s, a) => TaskListChanged()))
                );

            _taskItems.CollectionChanged += (s, a) => TaskListChanged();
        }
        private void TaskListChanged()
        {
            RaisePropertyChangedEvent(nameof(FilteredTaskItems));
            _taskListRepository.Update(new TaskList
            {
                Id = Id,
                Date = Date,
                TaskItems = _taskItems.Select(t => new TaskItem
                {
                    Name = t.Name,
                    Status = (t.IsCompleted ? TaskItemStatus.Completed : TaskItemStatus.Pending)
                }).ToList()
            });
        }
        #region show-completed-toggle
        private bool _showCompleted;
        public bool ShowCompleted
        {
            get { return _showCompleted; }
            set
            {
                _showCompleted = value;
                RaisePropertyChangedEvent(nameof(ShowCompleted));
                RaisePropertyChangedEvent(nameof(FilteredTaskItems));
            }
        }
        #endregion
        #region delete-button
        public ICommand DeleteTaskItemCommand => new Command<string>(OnClickDeleteButton);
        private async void OnClickDeleteButton(string taskItemName)
        {
            var taskItemToRemove = _taskItems.First(s => s.Name == taskItemName);
            _taskItems.Remove(taskItemToRemove);
        }
        #endregion
        #region edit-button
        public ICommand DisplayEditDialogCommand => new Command<string>(OnClickedTaskItem);
        private async void OnClickedTaskItem(string taskItemName)
        {
            var taskItemToEdit = _taskItems.First(s => s.Name == taskItemName);
            var promptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig()
                .SetTitle("Edit Task")
                .SetText(taskItemToEdit.Name)
                .SetPlaceholder("Edit Name")
                .SetInputMode(InputType.Name));

            if (promptResult.Ok)
            {
                taskItemToEdit.Name = promptResult.Text;
            }

        }
        #endregion
        #region create-button
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
            _taskItems.Add(new TaskItemViewModel(newTaskItem, (s, a) => TaskListChanged()));
        }
        #endregion
        public int Id { get; }
        public DateTime Date { get; }

    }
    #region task-item-view-model
    public class TaskItemViewModel : ViewModelBase
    {
        private TaskItem _model;
        public TaskItemViewModel(TaskItem model, System.ComponentModel.PropertyChangedEventHandler eventHandler)
        {
            _model = model;
            PropertyChanged += eventHandler;
        }
        public string Name { 
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                RaisePropertyChangedEvent(nameof(Name));
            }
        }
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
    #endregion
}
