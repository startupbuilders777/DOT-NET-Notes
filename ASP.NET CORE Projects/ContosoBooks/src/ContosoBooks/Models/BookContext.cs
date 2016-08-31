using Microsoft.Data.Entity;
/*
 Add Entity Framework

Open the project.json file. In the dependencies section, add the following line:

"dependencies": {
  ...
  "EntityFramework.SqlServer": "7.0.0-beta8"
},
 


 Open config.json. Add the following highlighted lines:

{
  "AppSettings": {
    "SiteTitle": "Contoso Books"
  },
  "Data": {
    "ConnectionString": "Server=(localdb)\\MSSQLLocalDB;Database=ContosoBooks;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  
    This defines a connection string to LocalDB, which is a lightweight version of SQL Server Express for development.

}
     
     */
namespace ContosoBooks.Models
{
    public class BookContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}