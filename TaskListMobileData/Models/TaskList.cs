using System;
using System.Collections.Generic;
using System.Text;

namespace TaskListMobileData.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<TaskItem> TaskItems { get; set; }
    }
}
