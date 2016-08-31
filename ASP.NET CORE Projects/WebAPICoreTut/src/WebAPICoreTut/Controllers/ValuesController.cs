using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

/*
 CONTROLLERBASE
On 10th December 2015 Microsoft introduced a new ControllerBase class which helps us 
implement a RESTful API controller without bringing in unnecessary properties/methods 
related to MVC websites such as ViewBag, ViewData and ViewResult. At the time of writing 
this is still on the dev branch so it will be interesting to see where Microsoft end up 
going with this new class…
     */
namespace WebAPICoreTut.Controllers
{
    /*
     
         You can specify a base route at the controller level using the new ‘[controller]’ 
         placeholder instead of hard-coding the controller name (‘[action]’ is also 
         supported but this would produce unRESTful URIs like ‘api/values/get’). 
         Routing has also been nicely combined with the various [Http{Verb}] 
         attributes so you can succinctly state that a PUT request with and ‘id’ will be handled by the Put method
         
    If you have used Web API 2 then you might expect the route to ‘api/values’ to work with no attribute routing at all. 

   Web API 2 controller would handle a GET request to ‘api/values’ for two reasons.

Firstly, the correct controller was selected because convention based routing is configured by default:
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        ...

        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );
    }
}
Secondly, the correct action was selected based on naming convention so a GET request will resolve to the Get method.

MVC 6 still allows actions to be selected based on the name matching the HTTP verb of the request, 
but since attribute based routing is now the recommended approach, you will no longer get a default routing 
configuration for selecting controllers by name.
         

         */
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

/*
Attribute-Based Routing
You also can use attribute-based routing with MVC 6. 
Here’s how you can modify the ProductsController Index() action so you can invoke it with the “/Things/All” request:

using Microsoft.AspNet.Mvc;
 
namespace RoutePlay.Controllers
{
    [Route("Things")]
    public class ProductsController : Controller
    {
        [Route("All")]
        public IActionResult Index()
        {
            return Content("It Works!");
        }
    }
}
Notice that both the ProductsController class and the Index() action are decorated with a [Route] attribute. 
The combination of the two attributes enables you to invoke the Index() method by requesting “/Things/All”. 
 
You also should notice that the Web API controller in the code above is decorated with a [Route] attribute 
that enables you to invoke the Movies controller with the request “/api/movies”. The special [controller] 
and [action] tokens are new to MVC 6 and they allow you to easily refer to the controller and action names 
in your route templates.
If you mix convention-based and attribute-based routing then attribute-based routing wins. Furthermore, both 
convention-based and attribute-based routing win over the default routes.        

[Route("[controller]")]
    public class MyController : Controller
    {
        // GET: /my/show
        [HttpGet("Show")]
        public IActionResult Show()
        {
            return View();
        }
 
        // GET: /my
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Get Invoked");
        }
 
        // POST: /my
        [HttpPost]
        public IActionResult Post()
        {
            return Content("Post Invoked");
        }
 
        // POST: /my/stuff
        [HttpPost("Stuff")]
        public IActionResult Post([FromBody]string firstName)
        {
            return Content("Post Stuff Invoked");
        }
    }

    The MyController has the following four actions:
Show() – Invoked with an HTTP GET request for “/my/show”
Get() – Invoked with an HTTP GET request for “/my”
Post() – Invoked with an HTTP POST request for “/my”
Post([fromBody]string firstName) – Invoked with an HTTP POST request for “/my/stuff”

If you just use the [HttpPost] attribute on an action then you can invoke the action 
without using the action name (for example, “/my”). If you use an attribute such as [HttpPost(“stuff”)] 
that includes a template parameter then you must include the action name when invoking the action (for example, “/my/stuff”).

You can use constraints to constrain the types of values that can be passed to a controller action. 
For example, if you have a controller action that displays product details for a particular product 
given a particular product id, then you might want to constrain the product id to be an integer:

[Route("[controller]")]
    public class ProductsController : Controller
    {
 
        [HttpGet("details/{id:int}")]
        public IActionResult Details(int id)
        {
            return View();
        }
 
    }

There are plenty of other inline constraints that you use such as alpha, minlength, and regex.
You can use a question mark ? in a template to mark a parameter as optional like this:

[HttpGet("details/{id:int?}")]
public IActionResult Details(int id)
{
    return View();
}

Finally, ASP.NET 5 introduces support for default parameter values. 
If you don’t supply a value for the id parameter then the parameter gets the value 99 automatically:
[HttpGet("details/{id:int=99}")]
public IActionResult Details(int id)
{
    return View();
}

    Mvc 6 Routing: RESTful Style Routes With MVC 6 Attribute Routing

Now with MVC 6, we no longer have the problem that I mentioned above, not only that, 
but now we can even declare RESTful like routes like [HttpGet("Our Route")] and [HttpPost("Our Route")].

[HttpGet("NewsLetter/SelectEmail/{page?}")]
public ActionResult SelectEmail(int? page, string priCat, string secCat)
{
 //Method body
}

Mvc 6 Routing: Attribute Routing Dynamic Controller and Action Name

In the past, we had to specify the controller and action name specifically, 
and if down the road we decide to change our controller or action name, 
we had to change the routes too. But now with new features added in MVC 6, 
we can declare that we want to use the same name for some section of our route as our controller or action, like this:

[Route("[controller]")]
public class MyController : Controller
{

  [Route("[controller]/[action]/{id}")]
  [HttpPost]
  [ValidateAntiForgeryToken]
   public ActionResult SelectEmail(int id)
  {

  }
}
MVC 6 Routing: Define Data Type and Default Value For Route Parameter
With MVC 6 Routing, now we can constrain the data type of our route parameters, or provide default value for it:

[HttpGet("details/{id:int=99}")]
 public IActionResult Details(int id)
 {
     return View();
 }


     */
