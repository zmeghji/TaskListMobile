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
        TaskList Get(DateTime date);
        List<TaskList> Get(DateTime? fromDate, DateTime? toDate);
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
       

        public TaskList Get(DateTime date)
        {
            return _connection.GetCollection<TaskList>().Query().Where(t => t.Date == date).SingleOrDefault();
        }

        public List<TaskList> Get(DateTime? fromDate, DateTime? toDate)
        {
            var queryable = _connection.GetCollection<TaskList>().Query();
            if (fromDate.HasValue && toDate.HasValue)
            {
                return queryable.Where(t => t.Date >= fromDate && t.Date <= toDate).ToList();
            }
            else if (fromDate.HasValue)
            {
                return queryable.Where(t => t.Date >= fromDate).ToList();
            }
            else if (toDate.HasValue)
            {
                return queryable.Where(t => t.Date <= toDate).ToList();
            }
            else
            {
                return queryable.ToList();
            }
        }
    }
}
