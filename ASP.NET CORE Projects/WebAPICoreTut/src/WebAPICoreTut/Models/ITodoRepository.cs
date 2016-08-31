using System.Collections.Generic;
/*
 This interface defines basic CRUD operations.
 Next, add a TodoRepository class that implements ITodoRepository:
     */
namespace TodoApi.Models
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);
        IEnumerable<TodoItem> GetAll();
        TodoItem Find(string key);
        TodoItem Remove(string key);
        void Update(TodoItem item);
    }
}