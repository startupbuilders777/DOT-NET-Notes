using Ch11Ex01;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace CollectionsComparisonsConversionsGenerics
{

    //time to make our own strongly typed collection
    //implementing all methods timely process
    //instead derieve collection from a class, such as System.Collections.CollectionBase
    //which is an abstract base that supplies most implementation.
    //CollectionBase exposes interfaces IEnumerable, ICollection, IList but provides only
    //some req implementation(Clear(), RemoveAt() of IList, and Count property from ICollection)
    //Implement errything else urself -> CollectionBase provides 2 protected properties that 
    //enable access to the items, use List, which gives you access to the items through an IList,
    //interface and InnerList, which is ArrayList oject used to store items
    public class Animals : CollectionBase {//CollectionBase allows you to use foreach syntax with your derieved collections
                                           //You cant do animalCollection[0].Feed(); without indexer
                                           //To access items via index you need indexer
                                           //Indexer => array like access, special kind of property to access elements,
                                           //can define and use complex parameter types with the square brackets if you want
        public void Add(Animal newAnimal) {
            List.Add(newAnimal);
        }
        public void Remove(Animal oldAnimal) {
            List.Remove(oldAnimal);
        }
        public Animal this[int animalIndex] { //This is how you define indexer
            get { return (Animal)List[animalIndex]; }//Strongly typed here}
            set { List[animalIndex] = value;  }
        }

        //Add and Remove have been implmented as strongly typed methods that use the standard
        //Add method of the IList interface used to access the items.
        //Will only work with Animal classes or classes derieved from Animal unlike ArrayList
        //implementations which work with any object  
        public Animals() {        }
        
    }

    //Can also implement Collections using IDictionary inteface, which uses keys to index values
    //Base Class to simplify implmentation => DictionaryBase. Class implements IEnumerable
    // and ICollection, providing basic collection manipulation capabilities that are the same
    //for any collection
    //Clear and Count implemented, although RemoveAt() isnt b/c IList method. IDictionary does
    //however have Remove() method whcih you must implement. 

    public class Animalz : DictionaryBase {

        public void Add(string newID, Animal newAnimal) {
            Dictionary.Add(newID, newAnimal);//
        }
        public void Remove(string animalID) {
            Dictionary.Remove(animalID);
        }
        public Animalz(){}
        public Animal this[string animalKey] {
            get { return (Animal)Dictionary[animalKey]; }
            set { Dictionary[animalKey] = value; }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Animals animalCollection = new Animals();
            animalCollection.Add(new Cow("Donna"));
            animalCollection.Add(new Chicken("Kevin"));
            foreach (Animal myAnimal in animalCollection) {
                myAnimal.Feed();
            }
            ReadKey();
            //Several interfaces in System.Collections namespace provide basic collection functionality
            //IEnumberable - ability to loop through items in a collection
            //ICollection - Obtain number of items in a collection, and copy items into a simple array type
            //              inherits from IEnumerable
            //IList - Inherits from above two and provides a list of items for a collection along with capabilities
            //for accessing these items, and other basic abilities related to lists of items.
            //IDictionary - Inherits from IEnumberable and ICollection and similar to IList, but provides a list
            //              of items accesible via key value rather than index
            //System.Array implements IList, ICollection,IEnumberable, however not adv features of IList
            //ArrayList inherits same three above but can contain variable number of items 

            WriteLine("Create an Array type collection of Animal " +
                   "objects and use it:");

            Animal[] animalArray = new Animal[2];
            Cow myCow1 = new Cow("Lea");
            animalArray[0] = myCow1;
            animalArray[1] = new Chicken("Noa");

            foreach (Animal myAnimal in animalArray)
            {
                WriteLine($"New {myAnimal.ToString()} object added to Array collection, Name = {myAnimal.Name}");
            }

            WriteLine($"Array collection contains {animalArray.Length} objects.");
            animalArray[0].Feed();
            //because array is type Animal, you have to cast to get Chicken functions
            ((Chicken)animalArray[1]).LayEgg();
            WriteLine();

            WriteLine("Create an ArrayList type collection of Animal " +
                          "objects and use it:");
            ArrayList animalArrayList = new ArrayList();//You can also use 2 other constructors
                                                        //First copies the contents of an existing collection 
            //to new instance by specifying existing collection as parameter
            //Other one specify initial size of array list
            Cow myCow2 = new Cow("Rual");
            animalArrayList.Add(myCow2);//Add stuff in arraylist
            animalArrayList.Add(new Chicken("Andrea"));
            //Once you have added items you can overwrite using usual syntax
            //animalArrayList[0] = new Cow("Alma");
            foreach (Animal myAnimal in animalArrayList)//foreach possible because arraylist (array does to) 
                //implements IEnumerable which
                //has single function called GetEnumerator()
            {
                WriteLine($"New {myAnimal.ToString()} object added to ArrayList collection," +
                             $" Name = {myAnimal.Name}");
            }
            WriteLine($"ArrayList collection contains {animalArrayList.Count} "//Count property part of ICollection interface
                + " objects.");
            //Simple arrays strongly typed(allow direct access to type of item it contains), 
            //you could of called Feed with just animalArray[0].Feed();
            //ArrayList is collection of System.Object objects so you have to cast everything
            ((Animal)animalArrayList[0]).Feed();
            ((Chicken)animalArrayList[1]).LayEgg();
            WriteLine();

            WriteLine("Additional manipulation of ArrayList:");
            animalArrayList.RemoveAt(0);
            //Alternatively could use animalArrayList.Remove(myCow2)
            //Remove(remove based on reference) and RemoveAt(remove based on index) are IList methods
            ((Animal)animalArrayList[0]).Feed(); //Only item left is this which you can access like this
                                                 //removing shifts reminaing items left
 

            animalArrayList.AddRange(animalArray);
            //AddRange allows you to add multiple items at once and takes in any collection that 
            //implements ICollection
            //AddRange specifit function that just arraylist has
            //InsertRange() => inserts starting at some specified position
            ((Chicken)animalArrayList[2]).LayEgg();
            Console.WriteLine($"The animal called {myCow1.Name} is at " +
                $"index {animalArrayList.IndexOf(myCow1)}.");//IndexOf uses reference, part of IList, same like Remove()
            myCow1.Name = "Mary";
            WriteLine("The animal is now " +
                 $"called {((Animal)animalArrayList[1]).Name}.");
            ReadKey();




        }
    }
}
