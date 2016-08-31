
/*
The Model-View-Controller (MVC) architectural pattern separates an app into three main components: 
the Model, the View, and the Controller. The MVC pattern helps you create apps that are testable 
and easier to maintain and update than traditional monolithic apps. MVC-based apps contain:

Models: Classes that represent the data of the app and that use validation logic to enforce business rules for that data. 
Typically, model objects retrieve and store model state in a database. In this tutorial, a Movie model retrieves 
movie data from a database, provides it to the view or updates it. Updated data is written to a SQL Server database.
Views: Views are the components that display the app’s user interface (UI). Generally, this UI displays the model data.
Controllers: Classes that handle browser requests, retrieve model data, and then specify view templates that return a 
response to the browser. In an MVC app, the view only displays information; the controller handles and responds to user 
input and interaction. For example, the controller handles route data and query-string values, and passes these values 
to the model. The model might use these values to query the database.
The MVC pattern helps you create apps that separate the different aspects of the app (input logic, business logic, and
UI logic), while providing a loose coupling between these elements. The pattern specifies where each kind of logic 
should be located in the app. The UI logic belongs in the view. Input logic belongs in the controller. Business logic 
belongs in the model. This separation helps you manage complexity when you build an app, because it enables you to work 
on one aspect of the implementation at a time without impacting the code of another. For example, you can work on the 
view code without depending on the business logic code.

Every public method in a controller is callable as an HTTP endpoint. 
In the sample above, both methods return a string. Note the comments preceding each method:

MVC invokes controller classes (and the action methods within them) depending on the incoming URL. 
The default URL routing logic used by MVC uses a format like this to determine what code to invoke:

/[Controller]/[ActionName]/[Parameters]

You set the format for routing in the Startup.cs file.
When you run the app and don’t supply any URL segments, it defaults to the “Home” controller and the “Index” 
method specified in the template line highlighted above.
The first URL segment determines the controller class to run. So localhost:xxxx/HelloWorld maps to the
HelloWorldController class. The second part of the URL segment determines the action method on the class. 
So localhost:xxxx/HelloWorld/Index would cause the Index method of the HelloWorldController class to run. 
Notice that we only had to browse to localhost:xxxx/HelloWorld and the Index method was called by default. 
This is because Index is the default method that will be called on a controller if a method name is not explicitly 
specified. The third part of the URL segment ( id) is for route data. We’ll see route data later on in this tutorial.


     */
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        // 
        // GET: /HelloWorld/
        /*
         Currently the Index method returns a string with a message that is hard-coded in the controller class. 
         Change the Index method to return a View object, as shown in the following code:
             */
        public IActionResult Index()
        {
            /*
            The Index method above uses a view template to generate an HTML response to the browser. 
            Controller methods (also known as action methods, such as the Index method above, generally 
            return an IActionResult (or a class derived from ActionResult), not primitive types like string. 
             */
            return View();
        }

        // 
        // GET: /HelloWorld/Welcome/ 
        /*
         Let’s modify the example slightly so that you can pass some parameter information from the URL to the controller 
(for example, /HelloWorld/Welcome?name=Scott&numtimes=4). Change the Welcome method to include two parameters 
as shown below. Note that the code uses the C# optional-parameter feature to indicate that the numTimes parameter 
defaults to 1 if no value is passed for that parameter.

            Run your app and browse to:

http://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4

You can try different values for name and numtimes in the URL. 
The MVC model binding system automatically maps the named parameters 
from the query string in the address bar to parameters in your method.
In the sample above, the URL segment (Parameters) is not used, the name and numTimes parameters 
are passed as query strings. The ? (question mark) in the above URL is a separator, and the query 
strings follow. The & character separates query strings.

Run the app and enter the following URL: http://localhost:xxx/HelloWorld/Welcome/3?name=Rick
This time the third URL segment matched the route parameter id. The Welcome method contains a parameter id that 
matched the URL template in the MapRoute method. The trailing ? (in id?) indicates the id parameter is optional.

Before we go to a database and talk about models, though, let’s first talk about passing information from the 
controller to a view. Controller actions are invoked in response to an incoming URL request. A controller class 
is where you write the code that handles the incoming browser requests, retrieves data from a database, and 
ultimately decides what type of response to send back to the browser. View templates can then be used from a 
controller to generate and format an HTML response to the browser.

Controllers are responsible for providing whatever data or objects are required in order 
for a view template to render a response to the browser. A best practice: A view template should 
never perform business logic or interact with a database directly. Instead, a view template should 
work only with the data that’s provided to it by the controller. Maintaining this “separation of concerns” 
helps keep your code clean, testable and more maintainable.

Currently, the Welcome method in the HelloWorldController class takes a name and a ID parameter and 
then outputs the values directly to the browser. Rather than have the controller render this response as 
a string, let’s change the controller to use a view template instead. The view template will generate a 
dynamic response, which means that you need to pass appropriate bits of data from the controller to the view 
in order to generate the response. You can do this by having the controller put the dynamic data (parameters) 
that the view template needs in a ViewData dictionary that the view template can then access.

Return to the HelloWorldController.cs file and change the Welcome method to add a Message and NumTimes 
value to the ViewData dictionary. The ViewData dictionary is a dynamic object, which means you can put 
whatever you want in to it; the ViewData object has no defined properties until you put something inside it.
The MVC model binding system automatically maps the named parameters (name and numTimes) from the query string 
in the address bar to parameters in your method. The complete HelloWorldController.cs file looks like this:
             */
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            // return HtmlEncoder.Default.Encode($"Hello {name}, id: {numTimes}");

            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}