using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobileData.Enums;
using TaskListMobileData.Models;

namespace TaskListMobile.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        public TaskItem _model;
        public TaskItemViewModel(TaskItem model, System.ComponentModel.PropertyChangedEventHandler eventHandler)
        {
            _model = model;
            PropertyChanged += eventHandler;
        }
        public int Index
        {
            get { return _model.Index; }
             set { _model.Index = value; RaisePropertyChangedEvent(nameof(Index)); }
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
        public bool _markedForMove;
        public bool MarkedForMove
        {
            get { return _markedForMove; }
            set { _markedForMove = value; RaisePropertyChangedEvent(nameof(MarkedForMove)); }
        }
        public string RowBackgroundColor
        {
            get { if (MarkedForMove) { return "Lavender"; } else { return "White"; } }
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
