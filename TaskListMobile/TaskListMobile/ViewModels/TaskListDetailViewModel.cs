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
using TaskListMobile.Sevices;

namespace TaskListMobile.ViewModels
{
    public class TaskListDetailViewModel : ViewModelBase
    {
        private readonly ITaskListRepository _taskListRepository;
        private ObservableCollection<TaskItemViewModel> _taskItems;
        private bool _loadingAnotherList;
        public ObservableCollection<TaskItemViewModel> FilteredTaskItems
        {
            get
            {
                if (ShowCompleted)
                {
                    return new ObservableCollection<TaskItemViewModel>(_taskItems.OrderBy(t => t.Index));
                }
                else
                {
                    return new ObservableCollection<TaskItemViewModel>(_taskItems.Where(t => !t.IsCompleted).OrderBy(t => t.Index));
                }
            }
            private set { _taskItems = value; }
        }
        private void LoadNewTaskList(DateTime taskListDate)
        {
            _loadingAnotherList = true;
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
            RaisePropertyChangedEvent(nameof(FilteredTaskItems));
            _loadingAnotherList = false;
            _taskItems.CollectionChanged += (s, a) => TaskListChanged();
        }

        public TaskListDetailViewModel(
            ITaskListRepository taskListRepository,
            DateTime taskListDate)
        {
            _taskListRepository = taskListRepository;
            LoadNewTaskList(taskListDate);
        }
        private void TaskListChanged()
        {
            if (!_loadingAnotherList)
            {
                RaisePropertyChangedEvent(nameof(FilteredTaskItems));
                _taskListRepository.Update(new TaskList
                {
                    Id = Id,
                    Date = Date,
                    TaskItems = _taskItems.Select(t => t._model).ToList()
                });
            }
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
        #region change-to-another-day
        public ICommand GoListForAnotherDayCommand => new Command<DateTime>(OnDateSelected);
        private async void OnDateSelected(DateTime date)
        {
            LoadNewTaskList(date);
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
        #region mark-for-move-button
        public ICommand MarkForMoveCommand => new Command<string>(OnClickedMoveButton);
        private async void OnClickedMoveButton(string taskItemName)
        {
            var taskItemToMove = _taskItems.First(s => s.Name == taskItemName);
            if (taskItemToMove.MarkedForMove)
            {
                taskItemToMove.MarkedForMove = false;
            }
            else if (_taskItems.Any(t => t.MarkedForMove))
            {
                var itemAlreadyMarkedToMove = _taskItems.First(t => t.MarkedForMove);
                var tmpIndex = itemAlreadyMarkedToMove.Index;
                itemAlreadyMarkedToMove.Index = taskItemToMove.Index;
                taskItemToMove.Index = tmpIndex;
                itemAlreadyMarkedToMove.MarkedForMove = false;
            }
            else
            {
                taskItemToMove.MarkedForMove = true;
            }
        }
        #endregion
        #region reschedule-button
        public ICommand RescheduleDialogCommand => new Command<string>(OnClickedRescheduleButton);
        private async void OnClickedRescheduleButton(string taskItemName)
        {
            var taskItemToEdit = _taskItems.First(s => s.Name == taskItemName);

            var promptResult = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig());

            if (promptResult.Ok)
            {
                var taskItemToReschedule = _taskItems.First(s => s.Name == taskItemName);
                _taskItems.Remove(taskItemToReschedule);
                _taskListRepository.MoveTask(
                    taskItem: taskItemToReschedule._model,
                    dateToMoveTo: promptResult.SelectedDate);
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
                Status = TaskItemStatus.Pending,
                Index = _taskItems.Count
            };
            _taskItems.Add(new TaskItemViewModel(newTaskItem, (s, a) => TaskListChanged()));
        }
        #endregion
        public int Id { get; private set; }
        public DateTime Date { get; set; }

    }

}
