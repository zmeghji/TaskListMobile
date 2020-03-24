using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;
using TaskListMobileData.Models;

namespace TaskListMobileData.Repositories
{
    public interface ITaskListRepository
    {
        TaskList Create(TaskList taskList);
        TaskList GetByDate(DateTime date);
    }
    public class TaskListRepository : ITaskListRepository
    {
        private readonly LiteDatabase _connection;
        public TaskListRepository(string dbPath)
        {
            _connection = new LiteDatabase(dbPath);
        }

        public TaskList Create(TaskList taskList)
        {
            _connection.GetCollection<TaskList>().Insert(taskList);
            return taskList;
        }

        public TaskList GetByDate(DateTime date)
        {
            return _connection.GetCollection<TaskList>().Query().Where(t => t.Date == date).SingleOrDefault();
        }
    }
}
