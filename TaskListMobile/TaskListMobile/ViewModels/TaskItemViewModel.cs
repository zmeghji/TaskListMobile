using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobileData.Enums;
using TaskListMobileData.Models;

namespace TaskListMobile.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private TaskItem _model;
        public TaskItemViewModel(TaskItem model, System.ComponentModel.PropertyChangedEventHandler eventHandler)
        {
            _model = model;
            PropertyChanged += eventHandler;
        }
        public string Name
        {
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
}
