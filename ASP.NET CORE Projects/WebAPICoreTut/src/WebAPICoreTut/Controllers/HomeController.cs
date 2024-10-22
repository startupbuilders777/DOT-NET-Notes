﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
/*

Remote validation is a great feature to use when you need to validate data on the client against data on the server. 
For example, your app may need to verify whether an email or user name is already in use, and it must query a 
large amount of data to do so. Downloading large sets of data for validating one or a few fields consumes too 
many resources. It may also expose sensitive information. An alternative is to make a round-trip request to validate 
a field.

You can implement remote validation in a two step process.
First, you must annotate your model with the [Remote] attribute. 
The [Remote] attribute accepts multiple overloads you can use to direct client side 
JavaScript to the appropriate code to call. The example points to the VerifyEmail action method of 
the Users controller.

public class User
{
    [Remote(action: "VerifyEmail", controller: "Users")]
    public string Email { get; set; }
}
The second step is putting the validation code in the corresponding action method as defined in the [Remote] 
attribute. It returns a JsonResult that the client side can use to proceed or pause and display an error if needed.

[AcceptVerbs("Get", "Post")]
public IActionResult VerifyEmail(string email)
{
    if (!_userRepository.VerifyEmail(email))
    {
        return Json(data: $"Email {email} is already in use.");
    }

    return Json(data: true);
}
Now when users enter an email, JavaScript in the view makes a remote call to see if that email has been taken, 
and if so, then displays the error message. Otherwise, the user can submit the form as usual.
     */
namespace WebAPICoreTut.Controllers
{

    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        /*
        When MVC receives an HTTP request, it routes it to a specific action method of a controller. 
        It determines which action method to run based on what is in the route data, then it binds 
        values from the HTTP request to that action method’s parameters. For example, consider the following URL:

        http://contoso.com/movies/edit/2

        Since the route template looks like this, {controller=Home}/{action=Index}/{id?}, 
        movies/edit/2 routes to the Movies controller, and its Edit action method. 
        It also accepts an optional parameter called id. The code for the action method should look something like this:

        public IActionResult Edit(int? id) 
        The strings in the URL route are not case sensitive.

        MVC will try to bind request data to the action parameters by name. 
        MVC will look for values for each parameter using the parameter name and the names of 
        its public settable properties. In the above example, the only action parameter is named id, 
        which MVC binds to the value with the same name in the route values. In addition to route values
        MVC will bind data from various parts of the request and it does so in a set order. Below is a list 
        of the data sources in the order that model binding looks through them:

Form values: These are form values that go in the HTTP request using the POST method. (including jQuery POST requests).
Route values: The set of route values provided by routing.
Query strings: The query string part of the URI.

Form values, route data, and query strings are all stored as name-value pairs.
Since model binding asked for a key named id and there is nothing named id in the form values, 
it moved on to the route values looking for that key. In our example, it’s a match. Binding happens, and the value 
is converted to the integer 2. The same request using Edit(string id) would convert to the string “2”.
So far the example uses simple types. In MVC simple types are any .NET primitive type or type with a string 
type converter. If the action method’s parameter were a class such as the Movie type, which contains both simple and 
complex types as properties, MVC’s model binding will still handle it nicely. It uses reflection and recursion 
to traverse the properties of complex types looking for matches. Model binding looks for the pattern
parameter_name.property_name to bind values to properties. If it doesn’t find matching values of this form, 
it will attempt to bind using just the property name. For those types such as Collection types, 
model binding looks for matches to parameter_name[index] or just [index]. Model binding treats Dictionary types 
similarly, asking for parameter_name[key] or just [key], as long as the keys are simple types. 
Keys that are supported match the field names HTML and tag helpers generated for the same model type. 
This enables round-tripping values so that the form fields remain filled with the user’s input for their 
convenience, for example, when bound data from a create or edit did not pass validation.
In order for binding to happen the class must have a public default constructor and member to be bound must be 
public writable properties. When model binding happens the class will only be instantiated using the public 
default constructor, then the properties can be set.

When a parameter is bound, model binding stops looking for values with that name and it moves on to bind 
the next parameter. If binding fails, MVC does not throw an error. You can query for model state errors by checking 
the ModelState.IsValid property.
Each entry in the controller’s ModelState property is a ModelStateEntry containing an Errors property. 
It’s rarely necessary to query this collection yourself. Use ModelState.IsValid instead.
Additionally, there are some special data types that MVC must consider when performing model binding:

IFormFile, IEnumerable<IFormFile>: One or more uploaded files that are part of the HTTP request.
CancelationToken: Used to cancel activity in asynchronous controllers.         
These types can be bound to action parameters or to properties on a class type.

Once model binding is complete, validation occurs. Default model binding works great for the vast majority 
of development scenarios. It is also extensible so if you have unique needs you can customize the built-in behavior.
MVC contains several attributes that you can use to direct its default model binding behavior to a different source. 
For example, you can specify whether binding is required for a property, 
or if it should never happen at all by using the [BindRequired] or [BindNever] attributes. 
Alternatively, you can override the default data source, and specify the model binder’s data source. 
Below is a list of model binding attributes:

[BindRequired]: This attribute adds a model state error if binding cannot occur.
[BindNever]: Tells the model binder to never bind to this parameter.
[FromHeader], [FromQuery], [FromRoute], [FromForm]: Use these to specify the exact binding source you want to apply.
[FromServices]: This attribute uses dependency injection to bind parameters from services.
[FromBody]: Use the configured formatters to bind data from the request body. The formatter is 
            selected based on content type of the request.
[ModelBinder]: Used to override the default model binder, binding source and name.

Request data can come in a variety of formats including JSON, XML and many others. 
When you use the [FromBody] attribute to indicate that you want to bind a parameter 
to data in the request body, MVC uses a configured set of formatters to handle the 
request data based on its content type. By default MVC includes a JsonInputFormatter 
class for handling JSON data, but you can add additional formatters for handling XML 
and other custom formats.
ASP.NET selects input formatters based on the Content-Type header and the type of the parameter, 
unless there is an attribute applied to it specifying otherwise. If you’d like to use XML or 
another format you must configure it in the Startup.cs file, but you may first have to obtain 
a reference to Microsoft.AspNetCore.Mvc.Formatters.Xml using NuGet. Your startup code should look something 
like this:
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
       .AddXmlSerializerFormatters();
}

Before an app stores data in a database, the app must validate the data. 
Data must be checked for potential security threats, verified that it is appropriately formatted 
by type and size, and it must conform to your rules. Validation is necessary although it can be 
redundant and tedious to implement. In MVC, validation happens on both the client and server.

Fortunately, .NET has abstracted validation into validation attributes. 
These attributes contain validation code, thereby reducing the amount of code you must write.

         */

    }
}
