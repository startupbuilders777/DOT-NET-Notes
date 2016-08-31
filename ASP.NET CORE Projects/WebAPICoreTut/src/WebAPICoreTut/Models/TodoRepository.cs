using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 Register the repository

By defining a repository interface, we can decouple the repository class from the MVC controller 
that uses it. Instead of instantiating a TodoRepository inside the controller we will inject an 
ITodoRepository the built-in support in ASP.NET Core for dependency injection.

This approach makes it easier to unit test your controllers. Unit tests should inject a 
mock or stub version of ITodoRepository. That way, the test narrowly targets the controller logic 
and not the data access layer.

In order to inject the repository into the controller, we need to register it with the DI container.
Open the Startup.cs file. Add the following using directive:

using TodoApi.Models;
In the ConfigureServices method, add the highlighted code:

public void ConfigureServices(IServiceCollection services)
{
    // Add framework services.
    services.AddMvc();
    // Add our repository type
    services.AddSingleton<ITodoRepository, TodoRepository>();
}
     
     */
namespace TodoApi.Models
{
    public class TodoRepository : ITodoRepository
    {
        static ConcurrentDictionary<string, TodoItem> _todos =
              new ConcurrentDictionary<string, TodoItem>();

        public TodoRepository()
        {
            Add(new TodoItem { Name = "Item1" });
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return _todos.Values;
        }

        public void Add(TodoItem item)
        {
            item.Key = Guid.NewGuid().ToString();
            _todos[item.Key] = item;
        }

        public TodoItem Find(string key)
        {
            TodoItem item;
            _todos.TryGetValue(key, out item);
            return item;
        }

        public TodoItem Remove(string key)
        {
            TodoItem item;
            _todos.TryGetValue(key, out item);
            _todos.TryRemove(key, out item);
            return item;
        }

        public void Update(TodoItem item)
        {
            _todos[item.Key] = item;
        }
    }
}