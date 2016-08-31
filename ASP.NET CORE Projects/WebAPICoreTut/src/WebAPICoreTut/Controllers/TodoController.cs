using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Framework.Logging;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        /*
         PROPERTY INJECTION
For some reason Microsoft decided to use property injection to introduce a repository to their TodoController:

public class TodoController : Controller
{
    [FromServices]
    public ITodoRepository TodoItems { get; set; }
}
I can only assume that property injection was chosen by the author in order to show off the [FromServices] attribute because 
I think constructor injection should be preferred since it advertises the dependencies of a class.

         Having configured the ILoggerFactory you could now inject an ILoggerFactory into your 
         controller and call the CreateLogger<T> extension method to create a logger instance named 
         after a given type. However, I’m going to inject an ILogger<BooksController> directly:

            This is neat because the framework authors have realised that most consumers 
            would just use the ILoggerFactory to create an ILogger with a name relevant 
            to the context from which it is being called, so when the framework sees that 
            we want an ILogger<BooksController> it uses the configured ILoggerFactory to 
            provide one so we don’t have to pollute the constructor code with a call to CreateLogger.
       
      The default LoggerFactory maintains an in-memory dictionary of Logger instances keyed 
      by name so injecting an ILoggerFactory<ConcreteType> anywhere in your application 
      will resolve to the same logger instance as long as the type is the same.        
                           */


        public TodoController(ITodoRepository todoItems)// Microsoft.Framework.Logging.ILogger<TodoItem> logger)
        {
            TodoItems = todoItems;
          //  this.logger = logger;
        }
        public ITodoRepository TodoItems { get; set; }
       // private Microsoft.Framework.Logging.ILogger logger;

        //These methods implement the two GET methods:

        //GET /api/todo
        //GET /api/todo/{id}
        /*
         Here is an example HTTP response for the GetAll method:

    HTTP/1.1 200 OK
    Content-Type: application/json; charset=utf-8
    Server: Microsoft-IIS/10.0
    Date: Thu, 18 Jun 2015 20:51:10 GMT
    Content-Length: 82

    [{"Key":"4f67d7c5-a2a9-4aae-b030-16003dd829ae","Name":"Item1","IsComplete":false}]
               
   The [HttpGet] attribute specifies that these are HTTP GET methods. The URL path for each method is constructed as follows:

Take the template string in the controller’s route attribute, [Route("api/[controller]")]
Replace “[Controller]” with the name of the controller, which is the controller class name minus the “Controller” suffix. 
For this sample the name of the controller is “todo” (case insensitive). For this sample, the controller class name is 
TodoController and the root name is “todo”. ASP.NET MVC Core is not case sensitive.
If the [HttpGet] attribute also has a template string, append that to the path. This sample doesn’t use a template string.
For the GetById method, “{id}” is a placeholder variable. In the actual HTTP request, the client 
will use the ID of the todo item. At runtime, when MVC invokes GetById, it assigns the value of “{id}” 
in the URL the method’s id parameter. 
                      
Change the launch URL to “api/todo”
Right click on the project > Properties
Select the Debug tab and change the Launch URL to “api/todo”

The GetAll method returns a CLR object. MVC automatically serializes the object to JSON and 
writes the JSON into the body of the response message. The response code for this method is 200, 
assuming there are no unhandled exceptions. (Unhandled exceptions are translated into 5xx errors.)
In contrast, the GetById method returns the more general IActionResult type, which represents a 
generic result type. That’s because GetById has two different return types:
If no item matches the requested ID, the method returns a 404 error. This is done by returning NotFound.
Otherwise, the method returns 200 with a JSON response body. This is done by returning an ObjectResult.
                */

        public IEnumerable<TodoItem> GetAll()
        {
            return TodoItems.GetAll();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(string id)
        {
            var item = TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }


        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            /*
             This is an HTTP POST method, indicated by the [HttpPost] attribute. 
             The [FromBody] attribute tells MVC to get the value of the to-do item from the body of the HTTP request.

             The CreatedAtRoute method returns a 201 response, which is the standard response for an HTTP POST 
             method that creates a new resource on the server. CreateAtRoute also adds a Location header to the response. 
             The Location header specifies the URI of the newly created to-do item. 

            I want to add a book using the following HTTP request:

POST http://localhost:5000/api/books HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
    "title": "my title",
    "author": "my author"
}

 It’s worth noting that as long as there is no [FromBody] (or similar) on a complex action parameter 
 then the framework will also allow properties to be bound via the query string like this:

POST http://localhost:5000/api/books?title=query+title&author=query+author HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
    "title": "body title",
    "author": "body author"
}
In this case the body is ignored and the title and author are bound from the query string. 
Note the Author and Title properties shown in the Locals window below:

The fun doesn’t stop there. If you use content-type ‘application/x-www-form-urlencoded’ you can also mix and match between query string and body like this:

POST http://localhost:5000/api/books?title=query%20title HTTP/1.1
Host: localhost:5000
Content-Type: application/x-www-form-urlencoded

title=form+title&author=form+author

This time the body contains the author and title, and the query string also contains the title. This means the Title property is bound from the query string, not the body.

             */
            TodoItems.Add(item);
            /*
            I’ll log a message whenever a book is created or edited using the ILogger.LogVerbose() extension method: 
             */
          //  this.logger.LogVerbose("Added {0} which has been completed: {1}", item.Name, item.IsComplete);
            return CreatedAtRoute("GetTodo", new { controller = "Todo", id = item.Key }, item);
        }


        /*
         Update is similar to Create, but uses HTTP PUT. The response is 204 (No Content). 
         According to the HTTP spec, a PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH.
             
             */
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] TodoItem item)
        {
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }

            var todo = TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoItems.Update(item);
            return new NoContentResult();



            //Returning from actions
            /*
             So I’ve used:

HttpNotFound(): 404 response with no body
Ok(book): 200 response with a JSON formatted book in the body
HttpBadRequest(): 400 response with no body
CreatedAtRoute("GetBook", new { id = book.Id }, book): 201 response with a Location header containing the URI of the new resource 
                                                       (based on the named route: "GetBook") and a JSON formatted book in the body

Note that we always have the option of creating a concrete implementation of IActionResult without the use of a helper method. 
The happy path of the Update method above just returns a new NoContentResult() without needing to call a helper. This will produce a 204 response.

Other Return Types:
A couple of noteworthy points:

void gives you a 200 response instead of the (surely, more appropriate) 204 response that Web API 2 produces.
string gives you content-type ‘text/plain’ instead of ‘application/json’, which seems like a more sensible change.
             */

        }

        /*
         The void return type returns a 204 (No Content) response. That means the client receives a 204 even if the item has already been deleted, 
         or never existed. There are two ways to think about a request to delete a non-existent resource:

“Delete” means “delete an existing item”, and the item doesn’t exist, so return 404.
“Delete” means “ensure the item is not in the collection.” The item is already not in the collection, so return a 204.
             */
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            TodoItems.Remove(id);

        }

    }
}