using System;
using Microsoft.AspNet.Builder;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;


/*

By convention, the framework finds the Startup class and runs its Main method 
(which uses a new C#6 “lambda method”) to bootstrap the application using the Startup class. 
This is what the class looks like out of the box:

The Configure method is required by convention.what happens when you remove => error

This error message reveals that the framework will look for a method named either 
Configure or ConfigureDevelopment. The framework will actually look for a method named 
Configure{EnvironmentName} on the Startup class, where {EnvironmentName} is read from the 
ASPNET_ENV or Hosting:Environment environment variable which you can set in the environment itself 
(GOTCHA: I had to restart Visual Studio after changing the Windows 7 system environment variable 
before the value would be picked up) or using launchSettings.json:

{
...
"profiles": {
"IIS Express": {
...  
"environmentVariables": {
"Hosting:Environment": "Development"
}
},
"web": {
...
"environmentVariables": {
"Hosting:Environment": "Development"
}
}
}
}
*  
We also have the option of creating a set of Startup{EnvironmentName} classes,
one of which will be selected based on the current environment name. 
If no environment name is configured then Production will be used - a safe and sensible default!
So ASP.NET 5 gives us a neat new option for writing environment specific code just by applying a naming
convention to Startup classes and methods, but that is just one of your options. 
You can also inject the new IHostingEnvironment interface anywhere that you would like to have environment 
specific code (including the Startup constructor!). See Working with Multiple Environments for more information. 

If you have ever written a Web API 2 application in the past you will be familiar 
with the Global.asax file, which contains a WebApiApplication class by default:  
In the new world the same is achieved in the Startup class by calling services.AddMvc() 
from ConfigureServices and app.UseMvc() from Configure. 

So we don’t need any smelly GlobalConfiguration class any more.

In ASP.NET Core, the Startup class provides the entry point for an application, 
and is required for all applications. It’s possible to have environment-specific startup classes 
and methods (see Working with Multiple Environments), but regardless, one Startup class will 
serve as the entry point for the application. ASP.NET searches the primary assembly for a class named
Startup (in any namespace). You can specify a different assembly to search using the Hosting:Application configuration key. 
It doesn’t matter whether the class is defined as public; ASP.NET will still load it if it conforms to the naming 
convention. If there are multiple Startup classes, this will not trigger an exception. ASP.NET will select one 
based on its namespace (matching the project’s root namespace first, otherwise using the class in the alphabetically 
first namespace).

ASP.NET Core provides certain application services and objects during your application’s startup. 
You can request certain sets of these services by simply including the appropriate interface as a 
parameter on your Startup class’s constructor or one of its Configure or ConfigureServices methods. 
The services available to each method in the Startup class are described below. The framework services 
and objects include:

IApplicationBuilder
Used to build the application request pipeline. 
Available only to the Configure method in Startup. Learn more about Request Features.
IApplicationEnvironment
Provides access to the application properties, such as ApplicationName, ApplicationVersion, and ApplicationBasePath. 
Available to the Startup constructor and Configure method.
IHostingEnvironment
Provides the current EnvironmentName, WebRootPath, and web root file provider. 
Available to the Startup constructor and Configure method.
ILoggerFactory
Provides a mechanism for creating loggers. 
Available to the Startup constructor and Configure method. Learn more about Logging.
IServiceCollection
The current set of services configured in the container. 
Available only to the ConfigureServices method, and used 
by that method to configure the services available to an application.

Looking at each method in the Startup class in the order in which they are called, 
the following services may be requested as parameters:

Startup Constructor - IApplicationEnvironment - IHostingEnvironment - ILoggerFactory

ConfigureServices - IServiceCollection

Configure - IApplicationBuilder - IApplicationEnvironment - IHostingEnvironment - ILoggerFactory

Middleware are software components that are assembled into an application 
pipeline to handle requests and responses. Each component chooses whether to 
pass the request on to the next component in the pipeline, and can perform certain 
actions before and after the next component is invoked in the pipeline. Request delegates are used to 
build the request pipeline. The request delegates handle each HTTP request.
Request delegates are configured using Run, Map, and Use extension methods on the 
IApplicationBuilder type that is passed into the Configure method in the Startup class. 
An individual request delegate can be specified in-line as an anonymous method, or it can be defined in a 
reusable class. These reusable classes are middleware, or middleware components. Each middleware component in the 
request pipeline is responsible for invoking the next component in the pipeline, or short-circuiting the chain if 
appropriate.
The ASP.NET request pipeline consists of a sequence of request delegates, called one after the next,
Each delegate has the opportunity to perform operations before and after the next delegate. 
Any delegate can choose to stop passing the request on to the next delegate, and instead handle the request itself. 
This is referred to as short-circuiting the request pipeline, and is desirable because it allows unnecessary work to 
be avoided. For example, an authorization middleware might only call the next delegate if the request is authenticated; 
otherwise it could short-circuit the pipeline and return a “Not Authorized” response. Exception handling delegates 
need to be called early on in the pipeline, so they are able to catch exceptions that occur in deeper calls within 
the pipeline.
*/
namespace WebAPICoreTut
{
    /*
     Routing middleware is used to map requests to route handlers. 
     Routes are configured when the application starts up, and can extract values from the URL that 
     will be passed as arguments to route handlers.  

    The routing middleware uses routes to map requests to an IRouter instance. 
    The IRouter instance chooses whether or not to handle the request, and how. 
    The request is considered handled if its RouteContext.Handler property is set to a non-null value. 
    If no route handler is found for a request, then the middleware calls next (and the next middleware 
    in the request pipeline is invoked).
    To use routing, add it to the dependencies in project.json:
     "Microsoft.AspNetCore.Routing": "1.0.0-rc2-final",
     Add routing to ConfigureServices in Startup.cs:
     public void ConfigureServices(IServiceCollection services)
{
    services.AddRouting();
}

////////////////////////////////////////////////////////////////////////////////////////////////////      
     Static files are typically located in the web root(< content - root >/ wwwroot) folder.

    You generally set the content root to be the current directory so that your project’s web root will be found 
    while in development.

public static void Main(string[] args)
{
    var host = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseIISIntegration()
        .UseStartup<Startup>()
        .Build();

    host.Run();
}

        Static files can be stored in any folder under the web root and accessed with a relative path to that root. 
        For example, when you create a default Web application project using Visual Studio, 
        there are several folders created within the wwwroot folder - css, images, and js. 
        The URI to access an image in the images subfolder:

http://<app>/images/<imageFileName>
http://localhost:9189/images/banner3.svg
In order for static files to be served, you must configure the Middleware to add static files to the pipeline. 
The static file middleware can be configured by adding a dependency on the Microsoft.AspNetCore.StaticFiles 
package to your project and then calling the UseStaticFiles extension method from Startup.Configure:
          
The static file module provides no authorization checks. Any files served by it, including 
those under wwwroot are publicly available. To serve files based on authorization:
Store them outside of wwwroot and any directory accessible to the static file middleware and
Serve them through a controller action, returning a FileResult where authorization is applied         
         */


    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /*
         Your Startup class can optionally include a ConfigureServices method for configuring services 
         that are used by your application. The ConfigureServices method is a public method on your Startup 
         class that takes an IServiceCollection instance as a parameter and optionally returns an IServiceProvider. 
         The ConfigureServices method is called before Configure. This is important, because some features 
         like ASP.NET MVC require certain services to be added in ConfigureServices before they can be wired 
         up to the request pipeline.

        Just as with Configure, it is recommended that features that require substantial setup within 
        ConfigureServices be wrapped up in extension methods on IServiceCollection. You can see in 
        this example from the default web site template that several Add[Something] extension methods 
        are used to configure the app to use services from Entity Framework, Identity, and MVC:

        The ConfigureServices method is also where you should add configuration option classes, 
        like AppSettings in the example above, that you would like to have available in your application.
             */
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            // Add framework services.
            /*
            Any Web Api (REST based) should return JSON response in form of Camel Case so that we 
            can sure consume the API in any client. We need to enable CamelCasePropertyNamesContractResolver 
            in Configure Services. 
             */
            services.AddMvc(options => options.MaxModelValidationErrors = 50)
                    .AddJsonOptions(a => a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddRouting();
            services.AddCors();
          
         
            //Add our repository type
            services.AddSingleton<ITodoRepository, TodoRepository>();
        }

    
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            /*
          Routing is enabled in the Configure method in the Startup class. 
          Create an instance of RouteBuilder, passing a reference to IApplicationBuilder. 
          You can optionally provide a DefaultHandler as well. Add additional routes using MapRoute and when 
          finished call

            Pass UseRouter the result of the RouteBuilder.Build method.
                       
             The defaultHandler route handler is used as the default for the RouteBuilder. 
             Calls to MapRoute will use this handler by default. 
             A second handler is configured within the HelloRouter instance added by the AddHelloRoute extension method. 
             This extension methods adds a new Route to the RouteBuilder, 
             passing in an instance of IRouter, a template string, and an IInlineConstraintResolver 
             (which is responsible for enforcing any route constraints specified):
           

            */
            //////////Tricky Way to Route
            var defaultHandler = new RouteHandler((c) =>
         c.Response.WriteAsync($"Hello world! Route values: " +
         $"{string.Join(", ", c.GetRouteData().Values)}")
         );

            var routeBuilder = new RouteBuilder(app, defaultHandler);

            routeBuilder.AddHelloRoute(app);
          
            routeBuilder.MapRoute(
                "Track Package Route",
                "package/{operation:regex(track|create|detonate)}/{id:int}");

            app.UseRouter(routeBuilder.Build());


            /*
            Several extension methods on RouteBuilder are available for convenience. 
            The most common of these is MapRoute, which allows the specification of a 
            route given a name and template, and optionally default values, constraints, 
            and/or data tokens. When using these extensions, you must have specified the 
            DefaultHandler and ServiceProvider properties of the RouteBuilder instance to 
            which you’re adding the route. These MapRoute extensions add new TemplateRoute 
            instances to the RouteBuilder that each target the IRouter configured as the DefaultHandler. 

            MapRoute doesn’t take an IRouter parameter - it only adds routes that will be handled by the DefaultHandler. 
            Since the default handler is an IRouter, it may decide not to handle the request. 
            For example, MVC is typically configured as a default handler that only handles requests 
            that match an available controller action.

            Data Tokens
Data tokens represent data that is carried along if the route matches. 
They’re implemented as a property bag for developer-specified data. 
You can use data tokens to store data you want to associate with a route, 
when you don’t want the semantics of defaults. Data tokens have no impact on 
the behavior of the route, while defaults do. Data tokens can also be any arbitrary types, 
while defaults really need to be things that can be converted to/from strings.

Routing is also used to generate URLs based on route definitions. 
This is used by helpers to generate links to known actions on MVC controllers, 
but can also be used independent of MVC. Given a set of route values, and optionally 
a route name, you can produce a VirtualPathContext object. Using the VirtualPathContext 
object along with a RouteCollection, you can generate a VirtualPath. IRouter implementations 
participate in link generation through the GetVirtualPath method.
The example below shows how to generate a link to a route given a dictionary of route values and a RouteCollection

The VirtualPath generated at the end of the sample above is /package/create/123.

  The second parameter to the VirtualPathContext constructor is a collection of ambient values. 
  Ambient values provide convenience by limiting the number of values a developer must specify 
  within a certain request context. The current route values of the current request are considered 
  ambient values for link generation. For example, in an MVC application if you are in the About 
  action of the HomeController, you don’t need to specify the controller route value to link to the Index 
  action (the ambient value of Home will be used).
Ambient values that don’t match a parameter are ignored, and ambient values are 
also ignored when an explicitly-provided value overrides it, going from left to right in the URL.
Values that are explicitly provided but which don’t match anything are added 
to the query string.

// demonstrate link generation
var trackingRouteCollection = new RouteCollection();
trackingRouteCollection.Add(routeBuilder.Routes[1]); // "Track Package Route"

app.Run(async (context) =>
{
    var dictionary = new RouteValueDictionary
    {
        {"operation","create" },
        {"id",123}
    };

    var vpc = new VirtualPathContext(context,
        null, dictionary, "Track Package Route");

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("Menu<hr/>");
    await context.Response.WriteAsync(@"<a href='" +
        trackingRouteCollection.GetVirtualPath(vpc).VirtualPath +
        "'>Create Package 123</a><br/>");
 */




            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            /*
             Any production system needs logging and that usually means writing some basic infrastructure based around ILoggerFactory and ILogger services.
             ASP.NET 5 introduces a new logging framework which provides these services for us. When you create a new ASP.NET 5 application you get the 
             ILoggerFactory injected into the default Startup.Configure() method:

            The AddConsole() and AddDebug() methods that you see here are extension methods that call ILoggerFactory.AddProvider(). 
            So we automatically get console logging and debug logging (Visual Studio output window) in our application. 
            This is all very nice but these loggers aren’t going to be of much use in production so I’m going to add another provider 
            that will log to a text file (I don’t want to be distracted by setting up a database).

            I’m going to use serilog. This post describes how to configure serilog as a provider. 
            It’s simply a case of adding a reference to the ‘Serilog.Framework.Logging’ package in project.json and adding some configuration:
            
            The AddSerilog() method adds a provider using the given serilog configuration:
         
            You will only be able to see console logs if you launch the application from the console.
            You can do this by running dnx web from the project directory (the one containing project.json), 
            or you can use Visual Studio to do the same thing:

            Each provider can have a minimum required LogLevel. 
            The ConsoleLoggerProvider is configured from a configuration file; 
            in this case the file is appsettings.json because this path is given in the Startup constructor:

            To add Serilog;

            var log = new Serilog.LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .WriteTo.RollingFile(
                               pathFormat: env.MapPath("MvcLibrary-{Date}.log"),
                               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {SourceContext} [{Level}] {Message}{NewLine}{Exception}")
                           .CreateLogger();

            loggerFactory.AddSerilog(log);
             */

            /*
             makes the files in web root (wwwroot by default) servable.
             Later I’ll show how to make other directory contents servable with UseStaticFiles.
             */
            app.UseStaticFiles(); //For the wwwroot folder

            /* Suppose you have a project hierarchy where the static files you wish to serve are outside the web root. 
             For example:

             wwwroot
             css
             images
             ...
             MyStaticFiles
             test.png
             For a request to access test.png, configure the static files middleware as follows:

         public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
         {
             // Code removed for brevity.

             app.UseStaticFiles();

             app.UseStaticFiles(new StaticFileOptions()
             {
                 FileProvider = new PhysicalFileProvider(
                     Path.Combine(Directory.GetCurrentDirectory(), @"MyStaticFiles")),
                 RequestPath = new PathString("/StaticFiles")
             });
             A request to http://<app>/StaticFiles/test.png will serve the test.png file.

             Directory browsing allows the user of your web app to see a list of directories and 
             files within a specified directory. 
             To enable directory browsing, call the UseDirectoryBrowser extension method from Startup.Configure:
              */
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/MyImages")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/MyImages")
            });
            /*
    The code above allows directory browsing of the wwwroot/images folder 
    using the URL http://<app>/MyImages, with links to each file and folder:
    Note the two app.UseStaticFiles calls. The first one is required to serve the CSS, 
    images and JavaScript in the wwwroot folder, and the second call for directory browsing 
    of the wwwroot/images folder using the URL http://<app>/MyImages:

Serving a default document

Setting a default home page gives site visitors a place to start when visiting your site. 
In order for your Web app to serve a default page without the user having to fully qualify the URI, 
call the UseDefaultFiles extension method from Startup.Configure as follows.     
UseDefaultFiles must be called before UseStaticFiles to serve the default file. 
UseDefaultFiles is a URL re-writer that doesn’t actually serve the file. 
You must enable the static file middleware (UseStaticFiles) to serve the file.
With UseDefaultFiles, requests to a folder will search for:

default.htm
default.html
index.htm
index.html
The following code shows how to change the default file name to mydefault.html.

 DefaultFilesOptions options = new DefaultFilesOptions();
    options.DefaultFileNames.Clear();
    options.DefaultFileNames.Add("mydefault.html");
    app.UseDefaultFiles(options);
    app.UseStaticFiles();
             */
            app.UseDefaultFiles();
            /*
            UseFileServer combines the functionality of UseStaticFiles, UseDefaultFiles, and UseDirectoryBrowser.
            The following code enables static files and the default file to be served, 
            but does not allow directory browsing:

            app.UseFileServer();
            The following code enables static files, default files and directory browsing:
          app.UseFileServer(enableDirectoryBrowsing: true);
          if you wish to serve files that exist outside the web root, 
          you instantiate and configure an FileServerOptions object that you pass as a parameter to UseFileServer   
         
            app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"MyStaticFiles")),
    RequestPath = new PathString("/StaticFiles"),
    EnableDirectoryBrowsing = true
});                        

URI	Response
http://<app>/StaticFiles/test.png	StaticFiles/test.png
http://<app>/StaticFiles	MyStaticFiles/default.html
If no default named files are in the MyStaticFiles directory, 
http://<app>/StaticFiles returns the directory listing with clickable links:
                  
The FileExtensionContentTypeProvider class contains a collection that maps file extensions to 
MIME content types. In the following sample, several file extensions are registered to known MIME types, 
the ”.rtf” is replaced, and ”.mp4” is removed. 
                         */

            // Set up custom content types -associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            // Replace an existing mapping
            provider.Mappings[".rtf"] = "application/x-msdownload";
            // Remove MP4 videos.
            provider.Mappings.Remove(".mp4");

            /*
            The following code enables serving unknown types and will render the unknown file as an image.

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "image/png"
            }); 
            With the code above, a request for a file with an unknown content type will be returned as an image.
                         */
            /*
            You can use convention-based routing with ASP.NET MVC 5 by defining the routes 
            in your project’s Startup class. For example, here is how you would map the requests 
            /Super and /Awesome to the ProductsController.Index() action: 
             */
            app.UseMvc(routes =>
            {
                // route1
                routes.MapRoute(
                    name: "route1",
                    template: "super",
                    defaults: new { controller = "Products", action = "Index" }
                );
                // route2
                routes.MapRoute(
                    name: "route2",
                    template: "awesome",
                    defaults: new { controller = "Products", action = "Index" }
                );
            });
        }

        ////////////////////////////////////////////////////////////////////////////
        ////////Configure Middleware Details///////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /*
         Your Configure method must accept an IApplicationBuilder parameter. 
         Additional services, like IHostingEnvironment and ILoggerFactory may also be specified, 
         in which case these services will be injected by the server if they are available. 
         In the following example from the default web site template, you can see several 
         extension methods are used to configure the pipeline with support for BrowserLink, 
         error pages, static files, ASP.NET MVC, and Identity.
         Each Use extension method adds middleware to the request pipeline. 
         For instance, the UseMvc extension method adds the routing middleware to the 
         request pipeline and configures MVC as the default handler.

        MiddleWare used Here:
        In the code above (in non-development environments), 
        UseExceptionHandler is the first middleware added to the pipeline, 
        therefore will catch any exceptions that occur in later calls.
        The static file module provides no authorization checks. 
        Any files served by it, including those under wwwroot are publicly available. 
        If you want to serve files based on authorization:
        Store them outside of wwwroot and any directory accessible to the static file middleware.
        Deliver them through a controller action, returning a FileResult where authorization is applied.
        A request that is handled by the static file module will short circuit the pipeline. 
        (see Working with Static Files.) If the request is not handled by the static file module, 
        it’s passed on to the Identity module, which performs authentication. If the request is not 
        authenticated, the pipeline is short circuited. If the request does not fail authentication,
        the last stage of this pipeline is called, which is the MVC framework.
        The order in which you add middleware components is generally the order in which they take effect on the request, 
        and then in reverse for the response. This can be critical to your app’s security, performance and functionality. 
        In the code above, the static file middleware is called early in the pipeline so it can handle requests and 
        short circuit without going through unnecessary components. The authentication middleware is added to the 
        pipeline before anything that handles requests that need to be authenticated. Exception handling must be 
        registered before other middleware components in order to catch exceptions thrown by those components.

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    loggerFactory.AddDebug();

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        app.UseBrowserLink();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();

    app.UseIdentity();

    // Add external authentication middleware below. 
    To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });
}
         The simplest possible ASP.NET application sets up a single request delegate that handles all requests. 
         In this case, there isn’t really a request “pipeline”, so much as a single anonymous function that is 
         called in response to every HTTP request.
         The first App.Run delegate terminates the pipeline. In the following example, only the first delegate (“Hello, World!”) will run.

public void Configure(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Hello, World!");
    });

    app.Run(async context =>
    {
        await context.Response.WriteAsync("Hello, World, Again!");
    });
}

You chain multiple request delegates together; the next parameter represents 
the next delegate in the pipeline. You can terminate (short-circuit) the pipeline by not calling 
the next parameter. You can typically perform actions both before and after the next delegate, 
as this example demonstrates:
Avoid modifying HttpResponse after invoking next, one of the next components 
in the pipeline may have written to the response, causing it to be sent to the client.
This ConfigureLogInline method is called when the application is run with an environment 
set to LogInline. Learn more about Working with Multiple Environments. 
We will be using variations of Configure[Environment] to show different options 
in the rest of this article. The easiest way to run the samples in Visual Studio is with the web command, 
which is configured in project.json.
n the above example, the call to await next.Invoke() will call into the next delegate await context.
Response.WriteAsync("Hello from " + _environment);. The client will receive the expected response (“Hello from LogInline”), 
and the server’s console output includes both the before and after messages:
You configure the HTTP pipeline using Run, Map, and Use. The Run method short circuits the pipeline 
(that is, it will not call a next request delegate). Thus, Run should only be called at the end of your pipeline. 
Run is a convention, and some middleware components may expose their own Run[Middleware] methods that should 
only run at the end of the pipeline.

public void ConfigureLogInline(IApplicationBuilder app, ILoggerFactory loggerfactory)
{
    loggerfactory.AddConsole(minLevel: LogLevel.Information);
    var logger = loggerfactory.CreateLogger(_environment);
    app.Use(async (context, next) =>
    {
        logger.LogInformation("Handling request.");
        await next.Invoke();
        logger.LogInformation("Finished handling request.");
    });

    app.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from " + _environment);
    });
}

We’ve already seen several examples of how to build a request pipeline with Use. 
Map* extensions are used as a convention for branching the pipeline. 
The current implementation supports branching based on the request’s path, or using a predicate. 
The Map extension method is used to match request delegates based on a request’s path. 
Map simply accepts a path and a function that configures a separate middleware pipeline. 
In the following example, any request with the base path of /maptest will be handled by the pipeline 
configured in the HandleMapTest method.
When Map is used, the matched path segment(s) are removed from HttpRequest.Path 
and appended to HttpRequest.PathBase for each request.


private static void HandleMapTest(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test Successful");
    });
}

public void ConfigureMapping(IApplicationBuilder app)
{
    app.Map("/maptest", HandleMapTest);

}

In addition to path-based mapping, the MapWhen method supports predicate-based middleware branching, 
allowing separate pipelines to be constructed in a very flexible fashion. Any predicate of type Func<HttpContext, bool> 
can be used to map requests to a new branch of the pipeline. In the following example, a simple predicate is used 
to detect the presence of a query string variable branch:

private static void HandleBranch(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Branch used.");
    });
}

public void ConfigureMapWhen(IApplicationBuilder app)
{
    app.MapWhen(context => {
        return context.Request.Query.ContainsKey("branch");
    }, HandleBranch);

    app.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from " + _environment);
    });
}

Using the configuration shown above, any request that includes a query string value for branch 
will use the pipeline defined in the HandleBranch method (in this case, a response of “Branch used.”). 
All other requests (that do not define a query string value for branch) 
will be handled by the delegate defined on line 17.

You can also nest Maps:

app.Map("/level1", level1App => {
    level1App.Map("/level2a", level2AApp => {
        // "/level1/level2a"
        //...
    });
    level1App.Map("/level2b", level2BApp => {
        // "/level1/level2b"
        //...
    });
});


ASP.NET ships with the following middleware components:

Middleware¶
Middleware	Description
Authentication	Provides authentication support.
CORS	Configures Cross-Origin Resource Sharing.
Diagnostics	Includes support for error pages and runtime information.
Routing	Define and constrain request routes.
Session	Provides support for managing user sessions.
Static Files	Provides support for serving static files, and directory browsing.

For more complex request handling functionality, the ASP.NET team recommends implementing the middleware in its 
own class, and exposing an IApplicationBuilder extension method that can be called from the Configure method. 
The simple logging middleware shown in the previous example can be converted into a middleware class that 
takes in the next RequestDelegate in its constructor and supports an Invoke method as shown:

RequestLoggerMiddleware.cs¶
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;
using System.Threading.Tasks;

namespace MiddlewareSample
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Handling request: " + context.Request.Path);
            await _next.Invoke(context);
            _logger.LogInformation("Finished handling request.");
        }
    }
}

Middleware can take advantage of the UseMiddleware<T> extension to inject services directly into their constructors, 
as shown in the example below. Dependency injected services are automatically filled, and the extension takes a 
params array of arguments to be used for non-injected parameters.

public static class RequestLoggerExtensions
{
    public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggerMiddleware>();
    }
}

Using the extension method and associated middleware class, the Configure method becomes very simple and readable.

public void ConfigureLogMiddleware(IApplicationBuilder app,
    ILoggerFactory loggerfactory)
{
    loggerfactory.AddConsole(minLevel: LogLevel.Information);

    app.UseRequestLogger();

    app.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from " + _environment);
    });
}

Although RequestLoggerMiddleware requires an ILoggerFactory parameter in its constructor, 
neither the Startup class nor the UseRequestLogger extension method need to explicitly supply it. 
Instead, it is automatically provided through dependency injection performed within UseMiddleware<T>.
Testing the middleware (by setting the Hosting:Environment environment variable to LogMiddleware) 
should result in output like the following (when using WebListener):
         */

    }


    public static class HelloExtensions
    {

        public static IRouteBuilder AddHelloRoute(this IRouteBuilder routeBuilder,
    Microsoft.AspNetCore.Builder.IApplicationBuilder app)
        {
            /*
            HelloRouter is a custom IRouter implementation. 
            AddHelloRoute adds an instance of this router to the RouteBuilder using a template string, 
            “hello/{name:alpha}”. This template will only match requests of the form “hello/{name}” where name is 
            constrained to be alphabetical. Matching requests will be handled by HelloRouter (which implements 
            the IRouter interface), which responds to requests with a simple greeting. 
             */
            routeBuilder.Routes.Add(new Route(new HelloRouter(),
                "hello/{name:alpha}",
                app.ApplicationServices.GetService<IInlineConstraintResolver>()));

            return routeBuilder;
        }
    }

    public class HelloRouter : IRouter
    {
        /*
        HelloRouter checks to see if RouteData includes a value for the key name. If not, 
        it immediately returns without handling the request. Likewise, it checks to see if the request 
        begins with “/hello”. Otherwise, the Handler property is set to a delegate that responds with a greeting. 
        Setting the Handler property prevents additional routes from handling the request. 
        The GetVirtualPath method is used for link generation. 

        Remember, it’s possible for a particular route template to match a given request, but the associated 
        route handler can still reject it, allowing a different route to handle the request.)
        This route was configured to use an inline constraint, signified by the :alpha in the name route value. 
        This constraint limits which requests this route will handle, in this case to alphabetical values for name. 
        Thus, a request for “/hello/steve” will be handled, but a request to “/hello/123” will not (instead, in this 
        sample the request will not match any routes and will use the “app.Run” delegate).        

        Template Routes

The most common way to define routes is using TemplateRoute and route template strings. 
When a TemplateRoute matches, it calls its target IRouter handler. In a typical MVC app, 
you might use a default template route with a string like this one:

../_images/default-mvc-routetemplate.png
This route template would be handled by the MvcRouteHandler IRouter instance. 
Tokens within curly braces ({ }) define route value parameters which will be bound 
if the route is matched. You can define more than one route value parameter in a route segment,
but they must be separated by a literal value. For example {controller=Home}{action=Index} would 
not be a valid route, since there is no literal value between {controller} and {action}. These 
route value parameters must have a name, and may have additional attributes specified.

You can use the * character as a prefix to a route value name to bind to the rest of the URI. 
For example, blog/{*slug} would match any URI that started with /blog/ and had any value following 
it (which would be assigned to the slug route value).

Route value parameters may have default values, designated by specifying the default after the parameter name, 
separated by an =. For example, controller=Home would define Home as the default value for controller. 
The default value is used if no value is present in the URL for the parameter. In addition to default values,
route parameters may be optional (specified by appending a ? to the end of the parameter name, as in id?). 
The difference between optional and “has default” is that a route parameter with a default value always produces 
a value; an optional parameter may not. Route parameters may also have constraints, which further restrict 
which routes the template will match.

Route Template	Example Matching URL	Notes
hello	/hello	Will only match the single path ‘/hello’
{Page=Home}	/	Will match and set Page to Home.
{Page=Home}	/Contact	Will match and set Page to Contact
{controller}/{action}/{id?}	/Products/List	Will map to Products controller and List method; Since id was not supplied in the URL, it’s ignored.
{controller}/{action}/{id?}	/Products/Details/123	Will map to Products controller and Details method, with id set to 123.
{controller=Home}/{action=Index}/{id?}	/	Will map to Home controller and Index method; id is ignored.

Adding a colon : after the name allows additional inline constraints to be set on a route value parameter.
Constraints with types always use the invariant culture - they assume the URL is non-localizable. 
Route constraints limit which URLs will match a route - URLs that do not match the constraint are ignored by the route.

constraint	Example	Example Match	Notes
int	{id:int}	123	Matches any integer
bool	{active:bool}	true	Matches true or false
datetime	{dob:datetime}	2016-01-01	Matches a valid DateTime value (in the invariant culture - see options)
decimal	{price:decimal}	49.99	Matches a valid decimal value
double	{price:double}	4.234	Matches a valid double value
float	{price:float}	3.14	Matches a valid float value
guid	{id:guid}	7342570B-44E7-471C-A267-947DD2A35BF9	Matches a valid Guid value
long	{ticks:long}	123456789	Matches a valid long value
minlength(value)	{username:minlength(5)}	steve	String must be at least 5 characters long.
maxlength(value)	{filename:maxlength(8)}	somefile	String must be no more than 8 characters long.
length(min,max)	{filename:length(4,16)}	Somefile.txt	String must be at least 8 and no more than 16 characters long.
min(value)	{age:min(18)}	19	Value must be at least 18.
max(value)	{age:max(120)}	91	Value must be no more than 120.
range(min,max)	{age:range(18,120)}	91	Value must be at least 18 but no more than 120.
alpha	{name:alpha}	Steve	String must consist of alphabetical characters.
regex(expression)	{ssn:regex(d{3}-d{2}-d{4})}	123-45-6789	String must match the provided regular expression.
required	{name:required}	Steve	Used to enforce that a non-parameter value is present during during URL generation.

To constrain a parameter to a known set of possible values, you can use a regex: {action:regex(list|get|create)}. 
This would only match the action route value to list, get, or create. If passed into the constraints dictionary, 
the string “list|get|create” would be equivalent. Constraints that are passed in the constraints dictionary 
(not inline within a template) that don’t match one of the known constraints are also treated as regular expressions.

Constraints can be chained. You can specify that a route value is of a certain type and also 
must fall within a specified range, for example: {age:int:range(1,120)}. 
Numeric constraints like min, max, and range will automatically convert the value 
to long before being applied unless another numeric type is specified.

Route templates must be unambiguous, or they will be ignored. 
For example, {id?}/{foo} is ambiguous, because it’s not clear which 
route value would be bound to a request for “/bar”. Similarly, {*everything}/{plusone} 
would be ambiguous, because the first route parameter would match everything from that 
part of the request on, so it’s not clear what the plusone parameter would match.

There is a special case route for filenames, such that you can define a route value like files/{filename}.{ext?}. 
When both filename and ext exist, both values will be populated. However, if only filename exists in the URL, 
the trailing period . is also optional. Thus, these would both match: /files/foo.txt and /files/foo
             */
        public Task RouteAsync(RouteContext context)
        {
            var name = context.RouteData.Values["name"] as string;
            if (String.IsNullOrEmpty(name))
            {
                return Task.FromResult(0);
            }
            var requestPath = context.HttpContext.Request.Path;
            if (requestPath.StartsWithSegments("/hello", StringComparison.OrdinalIgnoreCase))
            {
                context.Handler = async c =>
                {
                    await c.Response.WriteAsync($"Hi, {name}!");
                };
            }
            return Task.FromResult(0);
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }
    }
    

}
