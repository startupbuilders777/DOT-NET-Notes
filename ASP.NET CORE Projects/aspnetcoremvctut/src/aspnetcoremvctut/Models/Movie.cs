using System;
using System.ComponentModel.DataAnnotations;
/*
You’ll use a .NET Framework data-access technology known as the Entity Framework Core to define and work with these 
data model classes. Entity Framework Core (often referred to as EF Core) features a development paradigm called Code 
First. You write the code first, and the database tables are created from this code. Code First allows you to create 
data model objects by writing simple classes. (These are also known as POCO classes, from “plain-old CLR objects.”) 
The database is created from your classes. If you are required to create the database first, you can still follow 
this tutorial to learn about MVC and EF app development. 

In addition to the properties you’d expect to model a movie, the ID field is required by the DB for the primary key. 

Open the Movie.cs file. DataAnnotations provides a built-in set of validation attributes that 
you apply declaratively to any class or property. (It also contains formatting attributes like 
DataType that help with formatting and don’t provide any validation.)
Update the Movie class to take advantage of the built-in Required, StringLength,
RegularExpression, and Range validation attributes.

The validation attributes specify behavior that you want to enforce on the model 
properties they are applied to. The Required and MinimumLength attributes indicates 
that a property must have a value; but nothing prevents a user from entering white 
space to satisfy this validation. The RegularExpression attribute is used to limit what 
characters can be input. In the code above, Genre and Rating must use only letters (white space, 
numbers and special characters are not allowed). The Range attribute constrains a value to within 
a specified range. The StringLength attribute lets you set the maximum length of a string property, 
and optionally its minimum length. Value types (such as decimal, int, float, DateTime) are inherently 
required and don’t need the [Required] attribute.
*/
namespace MvcMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//Specifies how datafields 
        //displayed by ASP.NET
        //We don’t want to see the time (12:00:00 AM in the image below) and ReleaseDate should be two words.
        /*
         The Display attribute specifies what to display for the name of a field (in this case “Release Date” 
         instead of “ReleaseDate”). The DataType attribute specifies the type of the data, in this case it’s a 
         date, so the time information stored in the field is not displayed.

        In this section you’ll use Entity Framework Code First Migrations to add a new 
        field to the model and migrate that change to the database.
        When you use EF Code First to automatically create a database, Code First adds a table to the database to 
        help track whether the schema of the database is in sync with the model classes it was generated from. If 
        they aren’t in sync, EF throws an exception. This makes it easier to track down issues at development time 
        that you might otherwise only find (by obscure errors) at run time.

Notice how the form has automatically rendered an appropriate validation error message in each 
field containing an invalid value. The errors are enforced both client-side (using JavaScript and jQuery) 
and server-side (in case a user has JavaScript disabled).

A significant benefit is that you didn’t need to change a single line of code in the MoviesController 
class or in the Create.cshtml view in order to enable this validation UI. The controller and views you 
created earlier in this tutorial automatically picked up the validation rules that you specified by using 
validation attributes on the properties of the Movie model class. Test validation using the Edit action method, 
and the same validation is applied.

The form data is not sent to the server until there are no client side validation errors. You can verify 
this by putting a break point in the HTTP Post method, by using the Fiddler tool , or the F12 Developer tools.

Open the Movie.cs file and examine the Movie class. The System.ComponentModel.DataAnnotations namespace provides
formatting attributes in addition to the built-in set of validation attributes. We’ve already applied a DataType 
enumeration value to the release date and to the price fields. The following code shows the ReleaseDate and Price 
properties with the appropriate DataType attribute.

The DataType attributes only provide hints for the view engine to format the data (and supply attributes such 
as <a> for URL’s and <a href="mailto:EmailAddress.com"> for email. You can use the RegularExpression attribute 
to validate the format of the data. The DataType attribute is used to specify a data type that is more specific than
the database intrinsic type, they are not validation attributes. In this case we only want to keep track of the date, 
not the time. The DataType Enumeration provides for many data types, such as Date, Time, PhoneNumber, Currency, 
EmailAddress and more. The DataType attribute can also enable the application to automatically provide type-specific 
features. For example, a mailto: link can be created for DataType.EmailAddress, and a date selector can be provided 
for DataType.Date in browsers that support HTML5. The DataType attributes emits HTML 5 data- (pronounced data dash) 
attributes that HTML 5 browsers can understand. The DataType attributes do not provide any validation.

DataType.Date does not specify the format of the date that is displayed. 
By default, the data field is displayed according to the default formats based on the server’s CultureInfo.
The DisplayFormat attribute is used to explicitly specify the date format:

[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateTime ReleaseDate { get; set; }

The ApplyFormatInEditMode setting specifies that the formatting should also be applied 
when the value is displayed in a text box for editing. (You might not want that for some fields — 
for example, for currency values, you probably do not want the currency symbol in the text box for editing.)
You can use the DisplayFormat attribute by itself, but it’s generally a good idea to use the DataType attribute. 
The DataType attribute conveys the semantics of the data as opposed to how to render it on a screen, and provides 
the following benefits that you don’t get with DisplayFormat:
The browser can enable HTML5 features (for example to show a calendar control, the locale-appropriate currency symbol, 
email links, etc.)
By default, the browser will render data using the correct format based on your locale
The DataType attribute can enable MVC to choose the right field template to render the data 
(the DisplayFormat if used by itself uses the string template).

Note:
Query validation does not work with the Range attribute and DateTime. For example, the following code 
will always display a client side validation error, even when the date is in the specified range:
[Range(typeof(DateTime), "1/1/1966", "1/1/2020")]

The following code shows combining attributes on one line:

public class Movie
{
    public int ID { get; set; }

    [StringLength(60, MinimumLength = 3)]
    public string Title { get; set; }

    [Display(Name = "Release Date"), DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$"), Required, StringLength(30)]
    public string Genre { get; set; }

    [Range(1, 100), DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$"), StringLength(5)]
    public string Rating { get; set; }
}

             */

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        //NEW FIELD MIGRATIONS NEED TO BE WORKDED

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
        /*
         To add it in migrations do this:
         dotnet ef migrations add Rating
         dotnet ef database update
         or
          PM> Add-Migration -Context MvcMovieContext
          PM> Update-Database -Context MvcMovieContext
         
         */
    }
}
/*
 The ApplicationDbContext class handles the task of connecting to the database and mapping Movie 
 objects to database records. The database context is registered with the Dependency Injection container 
 in the ConfigureServices method in the Startup.cs file:

public void ConfigureServices(IServiceCollection services)
{
    // Add framework services.
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
The ASP.NET Core Configuration system reads the ConnectionString. For local development, it gets the connection string from the appsettings.json file:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=aspnet-MvcMovie-4ae3798a;
    Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "IncludeScopes": false,
When you deploy the app to a test or production server, you can use an environment variable or 
another approach to set the connection string to a real SQL Server.



The Edit, Details, and Delete links are generated by the MVC Core Anchor Tag Helper in the Views/Movies/Index.cshtml file.

<td>
    <a asp-action="Edit" asp-route-id="@item.ID">Edit</a> |
    <a asp-action="Details" asp-route-id="@item.ID">Details</a> |
    <a asp-action="Delete" asp-route-id="@item.ID">Delete</a>
</td>
Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files. 
In the code above, the AnchorTagHelper dynamically generates the HTML href attribute value from the controller 
action method and route id. You use View Source from your favorite browser or use the F12 tools to examine the 
generated markup. The F12 tools are shown below.

Recall the format for routing set in the Startup.cs file.

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

ASP.NET Core translates http://localhost:1234/Movies/Edit/4 into a request to the Edit action method of the 
Movies controller with the parameter ID of 4. (Controller methods are also known as action methods.)
Tag Helpers are one of the most popular new features in ASP.NET Core. See Additional resources for more information.
     */
