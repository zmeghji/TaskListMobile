using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using TaskListMobileData.Models;
using TaskListMobileData.Repositories;

namespace TaskListMobile.ViewModels
{
    public class TaskListIndexViewModel
    {
        private ObservableCollection<TaskList> _model;
        private readonly ITaskListRepository _taskListRepository;
        public TaskListIndexViewModel(ITaskListRepository taskListRepository, DateTime taskListDate)
        {
            _taskListRepository = taskListRepository;
            _model = new ObservableCollection<TaskList>(
                _taskListRepository.Get(taskListDate, null));
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
