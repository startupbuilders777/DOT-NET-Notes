using System.ComponentModel.DataAnnotations;

namespace ContosoBooks.Models
{
    public class Book
    {
        public int BookID { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public string Genre { get; set; }
        /*
         To keep the app simple, each book has a single author. 
         The Author property provides a way to navigate the relationship from a book to an author. 
         In EF, this type of property is called a navigation property. 
         When EF creates the DB schema, EF automatically infers that AuthorID should be a 
         foreign key to the Authors table.

            Primary Key	Foreign Key
Primary key uniquely identify a record in the table.	
Foreign key is a field in the table that is primary key in another table.
Primary Key can't accept null values.	
Foreign key can accept multiple null value.
By default, Primary key is clustered index and data in the database table 
is physically organized in the sequence of clustered index.	
Foreign key do not automatically create an index, clustered or non-clustered. 
You can manually create an index on foreign key.
We can have only one Primary key in a table.	
We can have more than one foreign key in a table.
             */


        public int AuthorID { get; set; }

        // Navigation property
        public Author Author { get; set; }
    }
}