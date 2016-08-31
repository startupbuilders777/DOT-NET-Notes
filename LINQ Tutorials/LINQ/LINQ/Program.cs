using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace LINQ
{
    class Program
    {

        public delegate bool StringFunc(string param);

        public static bool startWthS(string str) {
            return str.StartsWith("S");
        } 

        //Linq to XML is an alternate set of classes for XML that enables the use of LINQ for XML data and also makes certain operations with XML 
        //easier even if you are not using LINQ. LINQ to XML can have some advantages over XML DOM
        //For instance, LINQ to XML provides an easier way to create XML documents called functional construction, constructor calls nested 
        //in a way that naturally reflects the structure of an XML document. 
        static void Main(string[] args)
        {
            //Document contains elements, and each element contains attributes and other elements. 
            //The first and only parameter you pass in XDocument is is XElement
            //XElement - constructor takes the name of the element as a string, followed by a list of the XML objects contained withing that element. 
            //XAttribute takes only the name of the attribute and its value as parameters
            //Other Linq to XML constructors for all the XML node types:
            //XDeclaration() for the XML declaration at the start of an XML document, XComment() for an XML comment, and so on. 
            XDocument xdoc = new XDocument (new XElement("customers",
                                                    new XElement("customer", new XAttribute("ID", "A"),
                                                                             new XAttribute("City", "New York"),
                                                                             new XAttribute("Region", "North America"),
                                                                             new XElement("order",
                                                                                    new XAttribute("Item", "Widget"),
                                                                                    new XAttribute("Price", 100)
                                                                             ),
                                                                             new XElement("order",
                                                                                    new XAttribute("Item", "Tire"),
                                                                                    new XAttribute("Price", 200)
                                                                             )
                                                            ),
                                                    new XElement("customer", new XAttribute("ID", "B"),
                                                                             new XAttribute("City", "Mumbai"),
                                                                             new XAttribute("Region", "Asia"),
                                                                             new XElement("order",
                                                                                    new XAttribute("Item", "Oven"),
                                                                                    new XAttribute("Price", 501)
                                                                             )

                                                       )));
            WriteLine(xdoc); //Prints out XML using default ToString() method of XDocument()
            Write("Program finished, press Enter/Return to continue:");
            ReadLine();


            XElement xcust = new XElement("customers",
                                                    new XElement("customer", new XAttribute("ID", "A"),
                                                                             new XAttribute("City", "New York"),
                                                                             new XAttribute("Region", "North America"),
                                                                             new XElement("order",
                                                                                    new XAttribute("Item", "Widget"),
                                                                                    new XAttribute("Price", 100)
                                                                             ),
                                                                             new XElement("order",
                                                                                    new XAttribute("Item", "Tire"),
                                                                                    new XAttribute("Price", 200)
                                                                             )
                                                            ),
                                                    new XElement("customer", new XAttribute("ID", "B"),
                                                                             new XAttribute("City", "Mumbai"),
                                                                             new XAttribute("Region", "Asia"),
                                                                             new XElement("order",
                                                                                    new XAttribute("Item", "Oven"),
                                                                                    new XAttribute("Price", 501)
                                                                             )

                                                       )
                                                       );
            //Save, load, and display EXML element:
            /*
             Both XElement and XDocument inherit from the Linq to XML container class, XContainer class which implements an XML node that can contain other XML nodes. 
             Both classes also implement Load() and Save() so most operations that can be performed by XDocument can also be performed by XElement and its children
             XElement also supports the Load() and Parse() methods for loading XML from files and strings, respecitvely. 
             */
            string xmlFileName = @"c:\Users\HS1122\Desktop\C#\LINQ\fragment.xml";
            string xmlFileName2 = @"c:\Users\HS1122\Desktop\C#\LINQ\doc.xml";
            xcust.Save(xmlFileName);
            xdoc.Save(xmlFileName2);

            XElement xcust2 = XElement.Load(xmlFileName);
            XDocument xdoc2 = XDocument.Load(xmlFileName2);

            WriteLine("Contents of xcust: ");
            WriteLine(xcust);

            Write("Program finished, press Enter/Return to continue:");
            ReadLine();


            //Unlike XML DOM LINQ to XML works with XML fragmets(partial or incomplete XMML documents) in very much the same way as complete XML documents. 
            //Working with a fragment you simply work with XElement as the top-level  XML object instead of XDocument, when working with fragments, you cannot 
            //add esoteric XML node types that apply to docs such as XComment, XDeclaration, and XProcessingInstruction for XML processing instructions
            /*
             Linq to XML is just one example of a Linq provider, there are a number:
             LINQ to Objects - queries on any kind of C# in-memory object, such as arrays, lists, collection types,
             LINQ to XML - creating and manipulation of XML documents 
             LINQ to Entities => Enitity Framework 
             LINQ to DataSet => DataSet object was introduced in the first version of .Net framework, legacy, 
             LINQ to SQL => superseded by LINQ to Entities
             PLINQ => Parallel Linq extends Linq to objects with a parallel programming library that can split up a query to execute simultaneously
             LINQ to JSON => included in Newtonsoft package, creation and manipulation of json documents
             Syntax and Methods covered here will apply to all of these 
             
             */
            //LINQ TO OBJECTS PROVIDOR

            string[] names = { "Alonso", "Zheng", "Smith", "Jones", "Smythe", "Small", "Ruiz", "Hsieh", "Jorgenson", "Ilyich", "Singh", "Samba", "Fatimah" };

            //Linq query statement in this program uses the LINQ declarative query syntax
            //4 Parts: the result variable delaration beginning with var, which is assigned using a query expression consisting of the from clause, 
            //where clause, and the select clause. 
            //It should be var because then you dont have to worry about whats returned by the query. If the query can reutrn multiple items, then it acts like
            //a collection of the objects in the query data source(The query result will be a type that implements the IEnumerable<T> interface)
            //The form clause specifies the data you are querying: => from n in names (n is a stand in like in foreach), by specifying from, 
            //you are indicating you are going to query a subset of the collection. 
            //a Linq data source must be enumerable, so support IEnumberable<T> interface which is supported for any C#  arry or collection of items.
            //specify condition of query using where clause => where n.StartsWith("S"), any boolean expression that can be applied to the items in 
            //the data source can be specified in the where clause. where clause optional, it is a restriction operator in LINQ because it restricts the
            //results of the query. 
            //select clause specifies which items appear in the  result set. Is required clause b/c you must specify which items from query appear in the
            //result set
            
             
            var queryResults = from n in names
                               where n.StartsWith("S")
                               select n;


            WriteLine("Names that start with S: ");
            //foreach loop is part of your code that actually executes the linq query!!!
            //The assignment of the query result variable only saves a plan for executing the query; with Linq, the data itself is not retrieved until
            //the results are accessed. =>Called deferred query execution or lazy evaluation of queries. Execution will be deferred for any query that
            //produces a sequence, or, list of results. 
            foreach (var name in queryResults) {
                WriteLine(name);
            }
            ReadKey();

            /*Previous example done in LINQ query syntax, you can also rewrite example using Linq's method syntax(aka explicit syntax)
              LINQ is implemented as a series of extension methods to collcetions, arrays, query results, and any other object that 
              implements the IEnumerable<T> interface. 
              See names variable?
              there is a names.where(some bullshit parameter) function
             For instance, the Where<T> method and most other methods are extension methods . They are LINQ extensions, such as 
             Where<T>, Union<T>,Take<T> and others. The from...where...select querey expression translated into a series of calls by
             these methods. 

            Most of the LINQ methods that use method syntax require that you pass a method of function to evaluate the query expression. 
            The method/fuction parameter is passed in the form of a delegate which typically refereces a method. But you can also create 
            using lambda expressions that encapsulate the delegate in an elegant manner. 

             n => n < 0
            This declares a method with single parameter n, and returns true if n is less than 0, false otherwise. 
            
            (a,b) => a+b
            2 paramter method which returns sum of 2 params. You dont have to declare what type a and b are. They can be int, double
            or string. C# compiler infers the types.  
             */
            WriteLine();

            StringFunc StartWithS = new StringFunc(startWthS);

            string[] names2 = { "Alonso", "Zheng", "Smith", "Jones", "Smythe", "Small", "Ruiz", "Hsieh", "Jorgenson", "Ilyich", "Singh", "Samba", "Fatimah" };
            var queryResults2 = names2.Where(n => n.StartsWith("S")); //If the lambda expression is true for an item it is included. C# compiler infers that 
                                                                      //the where method should accept string from defn of input source
            var queryResults3 = names2.Where(startWthS);
            foreach (var name in queryResults2)
            {
                WriteLine(name);
            }
            WriteLine();
            foreach (var name in queryResults3)
            {
                WriteLine(name);
            }
            ReadKey();

            //You can also order query results
            //The orderby clause is optional like the where clause. 
            //by default orderby orders in ascending order (A to Z) but you can specify descending order(from Z to A) simply by adding the descending keyword
            //orderby n descending
            //plus you can order by the last letter in the name isntead of normal alphabetical order, you just change the orderby clause to the following =>
            //orderby n.Substring(n.Length - 1)
            //Last letters in alphabetical order. Note only looks at last letter, so Smith can come before Singh and Singh can come before Smith
            var queryResults4 = from n in names2
                                where n.StartsWith("S")
                                orderby n
                                select n;
            WriteLine("Names beg with S ordered alphabetically");
            foreach (var name in queryResults4)
            {
                WriteLine(name);
            }
            ReadKey();

            //If you did the query down below but did n> 1000, too many numbers!!
            //Here are some aggregrate operaters to simplify operations:
            //Count() => Count of Results
            //Min() => Min val in Results
            //Max() => Max val in Results
            //Average() => Average val of numeric results
            //Sum() => Total of all  of numeric results
            //There are more aggregate operations such as Aggregate() for executing arbitrary code in a manner that enables you to code your own aggregate 
            //function
            int[] numbers = GenerateLotsOfNumbers(48);
            var queryResults5 = from n in numbers
                                 where n > 1000
                              //   orderby n ascending
                                 select n;
            WriteLine("Count of numbers > 1000");
            WriteLine(queryResults5.Count());

            WriteLine("Max of Numbers > 1000");
            WriteLine(queryResults5.Max());

            WriteLine("Min of Numbers > 1000");
            WriteLine(queryResults5.Min());

            WriteLine("Average of Numbers > 1000");
            WriteLine(queryResults5.Average());

            WriteLine("Sum of Numbers > 1000");
            WriteLine(queryResults5.Sum(n =>(long)n)); 
            //32 bit int is what the default no parameter version of sum would return too small
            //lambda expression allows you convert result to long
            //There is a longcount. Other than that, If a 64 bit number expected from all the other operators, 
            //require a lambda to 64 bit coversion as parameter

            var nums = new[] { 1, 2, 3, 4 };
            var sums = nums.Aggregate((a, b) => a + b);
            Console.WriteLine(sums); //Output is 10, a is the aggregate and b is the next element, so adds it like this:
            //1+2 = 3, 3 +3 = 6, 6+4 = 10
            var divide = nums.Aggregate((a, b) => a * b);
            Console.WriteLine(divide); // 1*2*3*4 = 24

            //Another type of query is the SELECT DISTINCT query in which you search for the unique values in your data, 
            //query removes any repeated values from the result set. 
            List<Customer> customers = new List<Customer> {
              new Customer { ID="A", City="New York", Country="USA", Region="North America", Sales=9999},
              new Customer { ID="B", City="Mumbai", Country="India", Region="Asia", Sales=8888},
              new Customer { ID="C", City="Karachi", Country="Pakistan", Region="Asia", Sales=7777},
              new Customer { ID="D", City="Delhi", Country="India", Region="Asia", Sales=6666},
              new Customer { ID="E", City="São Paulo", Country="Brazil", Region="South America", Sales=5555 },
              new Customer { ID="F", City="Moscow", Country="Russia", Region="Europe", Sales=4444 },
              new Customer { ID="G", City="Seoul", Country="Korea", Region="Asia", Sales=3333 },
              new Customer { ID="H", City="Istanbul", Country="Turkey", Region="Asia", Sales=2222 },
              new Customer { ID="I", City="Shanghai", Country="China", Region="Asia", Sales=1111 },
              new Customer { ID="J", City="Lagos", Country="Nigeria", Region="Africa", Sales=1000 },
              new Customer { ID="K", City="Mexico City", Country="Mexico", Region="North America", Sales=2000 },
              new Customer { ID="L", City="Jakarta", Country="Indonesia", Region="Asia", Sales=3000 },
              new Customer { ID="M", City="Tokyo", Country="Japan", Region="Asia", Sales=4000 },
              new Customer { ID="N", City="Los Angeles", Country="USA", Region="North America", Sales=5000 },
              new Customer { ID="O", City="Cairo", Country="Egypt", Region="Africa", Sales=6000 },
              new Customer { ID="P", City="Tehran", Country="Iran", Region="Asia", Sales=7000 },
              new Customer { ID="Q", City="London", Country="UK", Region="Europe", Sales=8000 },
              new Customer { ID="R", City="Beijing", Country="China", Region="Asia", Sales=9000 },
              new Customer { ID="S", City="Bogotá", Country="Colombia", Region="South America", Sales=1001 },
              new Customer { ID="T", City="Lima", Country="Peru", Region="South America", Sales=2002 }
           };
            //Because Distinct() is available only in method syntax, you make the call using method syntax
            //however you can call it in query syntax too!

            //method syntax
            var queryResults6 = customers.Select(c => c.Region).Distinct();
            //query syntax: var queryResults = (from c in customers select c.Region).Distinct();

            foreach (var item in queryResults6)
            {
                WriteLine(item);
            }
            Write("Program finished, press Enter/Return to continue:");
            ReadLine();
            ReadLine();
            //Ordering By Multiple Levels,  What if you wanted to query your results then order alphabetically by region
            //then order alphabetically opposite by country if they the same, then order alphabetically by city name. You can do 
            //that like this: 

            var queryResults7 = from c in customers
                                orderby c.Region, c.Country descending, c.City 
                                //orderby operates on a field by field basis
                                //you can add descending to any fields listed here
                                select new { c.ID, c.Region, c.Country, c.City };
            //The select statement uses an anonymous type to encapsulate a set of read only properties into a single object
            //without having to define a type. The type of each property is inferred by the compiler. 
            //var v = new { Amount = 108, Message = "Hello" }; This shows an anonymous type initiliazed with 2 properties 
            //named Amount and Message
            //If you do not specify member names in the anonymous type, the compiler gives the anonymous type members the 
            //same name as the property being used to initialize them.
            foreach (var item in queryResults7)
            {
                WriteLine(item);
            }
            Write("Program finished, press Enter/Return to continue:");
            ReadLine();
            WriteLine();
            WriteLine();

            //Group Queries
            //Divides data into groups and enables you to sort, calculate aggregates, and compare by group
            //The data in a group is grouped by a key field, the field for which all the memebers of each group 
            //share a value. In this, the key field is the Region
            //You watn to calculate a total for each group, so you group into a new result set named cg
            var groupQuery = from c in customers
                             group c by c.Region into cg 
                             select new { TotalSales = cg.Sum(c => c.Sales), Region = cg.Key };
                             //The group result set implements the LINQ IGrouping interface which supports the Key property
                             //Always want to reference key somehow b/c it represents criteria by which each group in your
                             //data was created
            var orderedResults = from cg in groupQuery
                                 orderby cg.TotalSales descending
                                 select cg;
            WriteLine("Total\t: By\nSales\t: Region\n-----\t ------");
            foreach (var item in orderedResults) {
                WriteLine($"{item.TotalSales}\t: {item.Region}");
            }

            //A data set such as the customers and orders list you just created, with a shared key field(ID), enables a join query
            //whereby you can query related data in both lists with a single query, joining the results togetehtor with the key field
            WriteLine();
            WriteLine();

            List<Order> orders = new List<Order> {
              new Order { ID="P", Amount=100 },
              new Order { ID="Q", Amount=200 },
              new Order { ID="R", Amount=300 },
              new Order { ID="S", Amount=400 },
              new Order { ID="T", Amount=500 },
              new Order { ID="U", Amount=600 },
              new Order { ID="V", Amount=700 },
              new Order { ID="W", Amount=800 },
              new Order { ID="X", Amount=900 },
              new Order { ID="Y", Amount=1000 },
              new Order { ID="Z", Amount=1100 }
            };

            //The query uses the join keyword to unite customers with their corresponding orders using the ID fields from
            //the Customer and Order classes, respectively. The query result only includes the data for objects that have the same
            //id field value as the corresponding id field in the other collection
            var queryResults8 = from c in customers
                                join o in orders on c.ID equals o.ID
                                select new
                                {
                                    c.ID,
                                    c.City,
                                    SalesBefore = c.Sales,
                                    NewOrder = o.Amount,
                                    SalesAfter = c.Sales + o.Amount
                                };

            foreach (var item in queryResults)
            {
                WriteLine(item);
            }



            ReadKey();
        }
        class Order
        {
            public string ID { get; set; }
            public decimal Amount { get; set; }
        }

        class Customer
        {
            public string ID { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Region { get; set; }
            public decimal Sales { get; set; }

            public override string ToString()
            {
                return "ID: " + ID + " City: " + City + " Country: " + Country + " Region: " + Region + " Sales: " + Sales;
            }
        }

        private static int[] GenerateLotsOfNumbers(int count) {
            Random generator = new Random(DateTime.Now.Millisecond);
            int[] result = new int[count];
            for (int i = 0; i < count; ++i) {
                result[i] = generator.Next();
            }
            return result;
        }
    }
}
