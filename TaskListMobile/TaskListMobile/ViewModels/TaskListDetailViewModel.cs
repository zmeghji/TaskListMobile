using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobileData.Enums;
using TaskListMobileData.Models;
using TaskListMobileData.Repositories;

namespace TaskListMobile.ViewModels
{
    public class TaskListDetailViewModel
    {
        private readonly ITaskListRepository _taskListRepository;
        public TaskList Model;

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
