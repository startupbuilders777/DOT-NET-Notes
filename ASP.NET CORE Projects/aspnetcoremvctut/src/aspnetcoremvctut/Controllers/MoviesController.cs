using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using aspnetcoremvctut.Models;

namespace aspnetcoremvctut.Controllers
{
    /*
     The Visual Studio scaffolding engine creates the following:

A movies controller (Controllers/MoviesController.cs)
Create, Delete, Details, Edit and Index Razor view files (Views/Movies)
Visual Studio automatically created the CRUD (create, read, update, and delete) action methods and 
views for you (the automatic creation of CRUD action methods and views is known as scaffolding). 
You’ll soon have a fully functional web application that lets you create, list, edit, and delete movie entries.

If you run the app and click on the Mvc Movie link, you’ll get the following errors:
We’ll follow those instructions to get the database ready for our Movie app.

Update the database        
Open a command prompt in the project directory (MvcMovie/src/MvcMovie). 
Follow these instructions for a quick way to open a folder in the project directory.
Open a file in the root of the project (for this example, use Startup.cs.)
Right click on Startup.cs > Open Containing Folder.
 Shift + right click a folder > Open command window here
 Run cd .. to move back up to the project directory
Run the following commands in the command prompt:
dotnet ef migrations add Initial
dotnet ef database update
dotnet (.NET Core) is a cross-platform implementation of .NET. You can read about it here
dotnet ef migrations add Initial Runs the Entity Framework .NET Core CLI migrations command and 
    creates the initial migration. The parameter “Initial” is arbitrary, but customary for the first (initial) 
    database migration. This operation creates the Data/Migrations/<date-time>_Initial.cs file containing the 
    migration commands to add (or drop) the Movie table to the database
dotnet ef database update Updates the database with the migration we just created
         */
    public class MoviesController : Controller
    {
        /*
        The constructor uses Dependency Injection to inject the database context into the controller. 
        The database context is used in each of the CRUD methods in the controller.

A request to the Movies controller returns all the entries in the Movies table and then passes 
the data to the Index view. 
             */
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;    
        }

        // GET: Movies
        //search capability to the Index action method that lets you search movies by genre or name.
        //   public async Task<IActionResult> Index(string searchString)
        //     {

        //        var movies = from m in _context.Movie
        //                      select m;
        /*
         The first line of the Index action method creates a LINQ query to select the movies:
         The query is only defined at this point, it has not been run against the database.
         Navigate to /Movies/Index. Append a query string such as ?searchString=ghost to the URL. The filtered movies are displayed.
         if you rename searchString to id, you can pass the search title as route data (a URL segment) 
         instead of as a query string value.

        However, you can’t expect users to modify the URL every time they want to search for a movie. 
        So now you’ll add UI to help them filter movies. If you changed the signature of the Index method 
        to test how to pass the route-bound ID parameter, change it back so that it takes a parameter 
        named searchString:

        Open the Views/Movies/Index.cshtml file, and add the <form> markup highlighted below:
        <form asp-controller="Movies" asp-action="Index">
            <p>
             Title: <input type="text" name="SearchString">
              <input type="submit" value="Filter" />
            </p>
            The HTML <form> tag uses the Form Tag Helper, so when you submit the form, 
            the filter string is posted to the Index action of the movies controller. 
            Save your changes and then test the filter.
        </form>

        There’s no [HttpPost] overload of the Index method as you might expect. 
        You don’t need it, because the method isn’t changing the state of the app, just filtering data.

         You could add the following [HttpPost] Index method.
          */

        //        if (!String.IsNullOrEmpty(searchString))
        //        {
        //             movies = movies.Where(s => s.Title.Contains(searchString));
        //        }

        //          return View(await movies.ToListAsync());
        //      }

        /*
         The notUsed parameter is used to create an overload for the Index method.
         We’ll talk about that later in the tutorial.

            If you add this method, the action invoker would match the [HttpPost] Index method, 
            and the [HttpPost] Index method would run as shown in the image below.

            However, even if you add this [HttpPost] version of the Index method, 
            there’s a limitation in how this has all been implemented. Imagine that you 
            want to bookmark a particular search or you want to send a link to friends that 
            they can click in order to see the same filtered list of movies. Notice that the URL for the
            HTTP POST request is the same as the URL for the GET request (localhost:xxxxx/Movies/Index) – 
            there’s no search information in the URL. The search string information is sent to the server as 
            a form field value. 

            Because the search parameter is in the request body and not the URL, 
            you can’t capture that search information to bookmark or share with others. 
            We’ll fix this by specifying the request should be HTTP GET. Notice how intelliSense 
            helps us update the markup.
        
             <form asp-controller="Movies" asp-action="Index" method = "get">
                <p>
                 Title: <input type="text" name="SearchString">
                  <input type="submit" value="Filter" />
                </p>
            </form>

            Now when you submit a search, the URL contains the search query string. 
            Searching will also go to the HttpGet Index action method, even if you have a HttpPost Index method.
             */
        //      [HttpPost]
        //        public string Index(string searchString, bool notUsed)
        //       {
        //          return "From [HttpPost]Index: filter on " + searchString;
        //      }

        //Query with both genre and title

        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genre's.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel();
            movieGenreVM.genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            movieGenreVM.movies = await movies.ToListAsync();

            return View(movieGenreVM);
            /*
             The SelectList of genres is created by projecting the distinct genres 
             (we don’t want our select list to have duplicate genres).
              movieGenreVM.genres = new SelectList(await genreQuery.Distinct().ToListAsync())
             */
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*
            Earlier in this tutorial, you saw how a controller can pass data or objects to a view 
            using the ViewData dictionary. The ViewData dictionary is a dynamic object that provides 
            a convenient late-bound way to pass information to a view.

            MVC also provides the ability to pass strongly typed objects to a view. This strongly typed 
            approach enables better compile-time checking of your code and richer IntelliSense in Visual 
            Studio (VS). The scaffolding mechanism in VS used this approach (that is, passing a strongly 
            typed model) with the MoviesController class and views when it created the methods and views.
            The id parameter is generally passed as route data, for example http://localhost:1234/movies/details/1 
            sets:

The controller to the movies controller (the first URL segment)
The action to details (the second URL segment)
The id to 1 (the last URL segment)
You could also pass in the id with a query string as follows:
http://localhost:1234/movies/details?id=1
If a Movie is found, an instance of the Movie model is passed to the Details view: 

Examine the contents of the Views/Movies/Details.cshtml file:
             */
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }


        /*
         The first (HTTP GET) Create action method displays the initial Create form. 
         The second ([HttpPost]) version handles the form post. The second Create method (The [HttpPost] version) 
         calls ModelState.IsValid to check whether the movie has any validation errors. Calling this method 
         evaluates any validation attributes that have been applied to the object. If the object has validation 
         errors, the Create method re-displays the form. If there are no errors, the method saves the new movie 
         in the database. In our movie example, the form is not posted to the server when there are validation 
         errors detected on the client side; the second Create method is never called when there are client side
         validation errors. If you disable JavaScript in your browser, client validation is disabled and you can 
         test the HTTP POST Create method ModelState.IsValid detecting any validation errors.

         Below is portion of the Create.cshtml view template that you scaffolded earlier in the tutorial. 
         It’s used by the action methods shown above both to display the initial form and to redisplay 
         it in the event of an error.

<form asp-action="Create">
    <div class="form-horizontal">
        <h4>Movie</h4>
        <hr />
        <div asp-validation-summary="ModelOnly" class="text-danger"></div>
        <div class="form-group">
            <label asp-for="Genre" class="col-md-2 control-label"></label>
            <div class="col-md-10">
                <input asp-for="Genre" class="form-control" />
                <span asp-validation-for="Genre" class="text-danger" />
            </div>
        </div>
        @*Markup removed for brevity.*@
        <div class="form-group">
            <label asp-for="Rating" class="col-md-2 control-label"></label>
            <div class="col-md-10">
                <input asp-for="Rating" class="form-control" />
                <span asp-validation-for="Rating" class="text-danger" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Create" class="btn btn-default" />
            </div>
        </div>
    </div>
</form>

The Input Tag Helper consumes the DataAnnotations attributes and produces HTML attributes needed for 
jQuery Validation on the client side. The Validation Tag Helper displays a validation errors. 
See Validation for more information. What’s really nice about this approach is that neither the controller 
nor the Create view template knows anything about the actual validation rules being enforced or 
about the specific error messages displayed. The validation rules and the error strings are specified 
only in the Movie class. These same validation rules are automatically applied to the Edit view and any other 
views templates you might create that edit your model. When you need to change validation logic, you can do so 
in exactly one place by adding validation attributes to the model (in this example, the Movie class). 
You won’t have to worry about different parts of the application being inconsistent with how the rules are enforced — 
all validation logic will be defined in one place and used everywhere. This keeps the code very clean, and makes 
it easy to maintain and evolve. And it means that that you’ll be fully honoring the DRY principle.
             */
        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
         The [Bind] attribute is one way to protect against over-posting. 
         You should only include properties in the [Bind] attribute that you want to change. 
         See Protect your controller from over-posting for more information. ViewModels provide an 
         alternative approach to prevent over-posting.
         The reason why I was able to accomplish this was because model binding in ASP.NET MVC tries to bind the data submitted to model properties based on their names. If it matches, it is accepted. And once it reaches the model, we simply save everything to the database. 

Bind
Using Bind attribute is a good way to prevent over posting attack. 
Using Bind attribute, we can include or exclude specific properties which are supposed to bind.

public ActionResult Edit
([Bind(Include = "AmountMoney")]Expense expense)
{
    ....... 
} 

In this example, only one property is added. 
So now, no matter how many values are posted from the client, 
we will get only one property out of those i.e. AmountMoney. 
All other properties will be ignored by the binding.

Similarly, we can also Exclude certain properties, if needed. 
ViewModel
Using ViewModel for each view also helps us in preventing this attack. 
The ViewModel should have only the properties which are supposed to edited 
by a particular view and no other properties. So these can be mapped to 
specific properties of the Model before its saved to the database. 
It is a good practice to have view models for views and to not use model entities for the view.
 
Server-Side Checking
On the HttpPost action method, we can explicitly check the value of the properties. 
We can write custom code to check if the current user is of employee role then use 
the database value of IsApproved flag else if the user is HR user role then use the value 
posted. This is feasible if the check needs to be done once in a while. This might 
get difficult to maintain pretty soon.
Notice the second Edit action method is preceded by the [HttpPost] attribute.

   The ValidateAntiForgeryTokenAttribute attribute is used to prevent forgery of a request 
   and is paired up with an anti-forgery token generated in the edit view file (Views/Movies/Edit.cshtml). 
   The edit view file generates the anti-forgery token with the Form Tag Helper.
   <form asp-action="Edit">
   The Form Tag Helper generates a hidden anti-forgery token that must match the 
   [ValidateAntiForgeryToken] generated anti-forgery token in the Edit method of the Movies controller.
             */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Genre,Price,ReleaseDate,Title, Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            /*
             Code First makes it easy to search for data using the SingleOrDefaultAsync method. 
             An important security feature built into the method is that the code verifies that 
             the search method has found a movie before the code tries to do anything with it. 
             */
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
         The [ValidateAntiForgeryToken] attribute validates the hidden XSRF token 
         generated by the anti-forgery token generator in the Form Tag Helper

The model binding system takes the posted form values and creates a Movie object that’s 
passed as the movie parameter. The ModelState.IsValid method verifies that the data submitted 
in the form can be used to modify (edit or update) a Movie object. If the data is valid it’s saved. 
The updated (edited) movie data is saved to the database by calling the SaveChangesAsync method of database 
context. After saving the data, the code redirects the user to the Index action method of the MoviesController 
class, which displays the movie collection, including the changes just made.
             */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Genre,Price,ReleaseDate,Title, Rating")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        /*
         Note that the HTTP GET Delete method doesn’t delete the specified movie, 
         it returns a view of the movie where you can submit (HttpPost) the deletion.
         Performing a delete operation in response to a GET request (or for that matter, 
         performing an edit operation, create operation, or any other operation that changes data) 
         opens up a security hole.
             */
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        /*
         The common language runtime (CLR) requires overloaded methods to have a unique parameter 
         signature (same method name but different list of parameters). However, here you need two Delete methods – 
         one for GET and one for POST – that both have the same parameter signature. 
         (They both need to accept a single integer as a parameter.)

         There are two approaches to this problem, one is to give the methods different names. 
         That’s what the scaffolding mechanism did in the preceding example. However, this introduces 
         a small problem: ASP.NET maps segments of a URL to action methods by name, and if you rename a method, 
         routing normally wouldn’t be able to find that method. The solution is what you see in the example, 
         which is to add the ActionName("Delete") attribute to the DeleteConfirmed method. That attribute 
         performs mapping for the routing system so that a URL that includes /Delete/ for a POST request 
         will find the DeleteConfirmed method.

         Another common work around for methods that have identical names and signatures is to artificially 
         change the signature of the POST method to include an extra (unused) parameter. That’s what we did 
         in a previous post when we added the notUsed parameter. You could do the same thing here for the [HttpPost] 
         Delete method:      
             */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
