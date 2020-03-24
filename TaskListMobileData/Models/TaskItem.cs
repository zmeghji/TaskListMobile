using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobileData.Enums;

namespace TaskListMobileData.Models
{
    public class TaskItem
    {
        public string Name { get; set; }
        public TaskItemStatus Status { get; set; }
    }
}
