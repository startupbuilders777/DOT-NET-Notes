using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using System.Data.Entity;//Entity Framework namespace
using System.ComponentModel.DataAnnotations; //Allows you to give hints to the EF on hw to set up the database
using static System.Console;

//Database is a persistent, structured storehouse for data. Relational databases include Microsoft SQL Server and Oracle. You can use Code First
//approach to create objects in C#, store them in a database, and use LINQ to query the objects without having to use another language liek SQL
//Code First => Entity Framework
//Comes from a database concept called the entity relationship model, where an entity is the abstract concept of a data object such as a customer,
//which is related to other entities such as orders and products in a relational database. 
//Entity framework maps the C# objects in your program to the entities in a relational database. This is called object relational mapping. 
//Object relatioal mapping is code that maps your classes, objects, and properties inc C# to tables , rows, and columns that make up a relational 
//database. Mapping code by hand tediuos and time consuming buy EF makes easy!

//The EF is built on top of ADO.NET, the low level data access library built into .NET. ADO.NET requires some knowledge of SQL, but EF handles this
//for you, and lets you concentrate on C# code. 
namespace DATABASE
{
    public class Store
    {
        [Key]
        public int StockId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual List<Stock> Inventory { get; set; }
        //Behaves like a normal List<Stock> collection excpect, b/c it is declared as virtual, the EF can override its
        //behaviour when storing and retrieving from the database.

    }
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public int OnHand { get; set; }
        public int OnOrder { get; set; }
        public virtual Book Item { get; set; }
    }
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        [Key]
        public int Code { get; set; }
        //The Key attribute is a data annotation telling C# to use this field as
        //the unique identifier for each object in the database, and for each row in the database
    }



    public class BookContext : DbContext
    {
        //Database context class to manage,create, update, and delete the table of books in the database
        public DbSet<Book> Books { get; set; }//collection of all the Book entities in my database
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Store> Stores { get; set; }

    }
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BookContext())
            {
                //using clause ensures that the database connection and other underlying plumbing
                //objects associated with the connection are closed properly when program is finished
                //even if there is an exception or other unexpected event
                //The reason for the "using" statement is to ensure that the object is disposed as soon 
                //as it goes out of scope, and it doesn't require explicit code to ensure that this happens.
                //Note that it's not necessarily a matter of the object being disposed correctly, but more of 
                //whether it is disposed in a timely manner. Objects implementing IDisposable which hold on to 
                //unmanaged resources like streams and file handles will also implement a finalizer that will ensure 
                //that Dispose is called during garbage collection. The problem is that GC might not happen for a 
                //relatively long time. using makes sure that Dispose is called once you're through with the object

                Book book1 = new Book { Title = "Wowwww", Author = "Perkins" };
                db.Books.Add(book1);
                Book book2 = new Book { Title = "WAZZGOOD", Author = "Quin" };
                db.Books.Add(book2);
                //Note that we did not assign any values to the Code property, at this point in time, that property
                //contains a default value

                var store1 = new Store
                {
                    Name = "Main St Books",
                    Address = "123 Main St",
                    Inventory = new List<Stock>()
                };
                db.Stores.Add(store1);

                Stock store1book1 = new Stock
                { Item = book1, OnHand = 4, OnOrder = 6 };
                store1.Inventory.Add(store1book1);

                Stock store1book2 = new Stock
                { Item = book2, OnHand = 1, OnOrder = 9 };
                store1.Inventory.Add(store1book2);

                var store2 = new Store
                {
                    Name = "Campus Books",
                    Address = "321 College Ave",
                    Inventory = new List<Stock>()
                };

                db.Stores.Add(store2);

                Stock store2book1 = new Stock
                { Item = book1, OnHand = 7, OnOrder = 23 };
                store2.Inventory.Add(store2book1);

                Stock store2book2 = new Stock
                { Item = book2, OnHand = 2, OnOrder = 8 };
                store2.Inventory.Add(store2book2);

                db.SaveChanges(); //Save changes to the database
                //Saved the changes to BookContext db to the database
                //Used Key attribute to identify Code as a key, a unique value was assigned to the Code field when
                //each object was saved to the database. You dont have to care about this value because it is taken
                //care of by EF

                //EF takes cares of db details such as adding a foreign key column to the Stocks table in the databse to implement
                //the Inventory relationship between a Store and its Stock records. Similary, EF adds another foreign key column
                //column to the Stock table in the database to implement the Item relationship bet Stock and Book
                var query = from b in db.Books
                            orderby b.Title
                            select b;

                WriteLine("All books in the database: ");
                foreach (var b in query)
                {
                    WriteLine($"{b.Title} byte {b.Author}, code ={b.Code}");
                }
                WriteLine();
                WriteLine();

                var query2 = from store in db.Stores
                             orderby store.Name
                             select store;


                WriteLine("Bookstore Inventory Report:");
                WriteLine();
                foreach (var store in query2)
                {

                    WriteLine($"{store.Name} located at {store.Address}");

                    foreach (Stock stock in store.Inventory)
                    {
                        WriteLine($"- Title: {stock.Item.Title}");
                        WriteLine($"-- Copies in Store: {stock.OnHand}");
                        WriteLine($"-- Copies on Order: {stock.OnOrder}");
                    }
                    WriteLine();
                }

                ReadKey();
                //But wait where is db?
                //Tools => Connect to databasee. EF  creates in the first local SQL server instance it find on your computer. 
                //If you never had any database on your comp prev, VC#2015 create a local SQL Server instance for you called
                //(localdb)\MSSQLLocalDB

                //Handling Migrations -> As you change code, properties, relationships, need to keep database updated with your changed 
                //classes. You need to add Code First Migrations package to do so. 
                //Type in package manager console => Enable-Migrations -EnableAutomaticMigrations
                //This adds a migration class to project
                //The Entity Framework will compare the timestamp of the database to your program and advise you when the database is out
                //of sync with your classes. To update the database, simply enter this command in the Package Manager Console:
                //Update-Database

                //Creating and Querying XML from an existing database: 
                //Use linq to entities to query the data, and then linq to xml classes to conver tthe data to XML. This is 
                //an example of database first opposed to code first programming when you take an existing database and generate
                //c# objects from it. 

                var query3 = from store in db.Stores
                             orderby store.Name
                             select store;
                foreach (var s in query3)
                {
                    XElement storeElement = new XElement("store",
                        new XAttribute("name", s.Name),
                        new XAttribute("address", s.Address),
                        from stock in s.Inventory
                        select new XElement("stock",
                              new XAttribute("StockID", stock.StockId),
                              new XAttribute("onHand",
                               stock.OnHand),
                              new XAttribute("onOrder",
                               stock.OnOrder),
                       new XElement("book",
                       new XAttribute("title",
                           stock.Item.Title),
                       new XAttribute("author",
                           stock.Item.Author)
                       )// end book
                     ) // end stock
                   ); // end store
                    WriteLine(storeElement);
                }
                Write("Program finished, press Enter/Return to continue:");
                ReadLine();
            


            }
        }
    }
}
