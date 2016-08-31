using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Console;

namespace Ch07Ex02
{
    //Abstract Classes VS Interfaces
    //Interface memebers are by definiton 
    //public while abstact classes can have 
    //private members as long as they are not 
    //abstract, and abstract members can be protected 
    //and internal, and protected internal(protected internal 
    //these are only accessible from code-derieved classes within the project(more accurately the assembly))
    class MyClass
    { // by default classes declared as internal. 
        static int numOfClassInstances; //owned by class and not any object instances, MyClass.numOfClassInstances
        const int fart = 3;// static members by definition, do not use static keyword on them doe thats error
        static int yoo = 4; //optional initialization

        protected internal int poo = 5;//optional init

        public readonly int poopoo; //readonly means field only can be assigned a value during constructor execution
                                    //or  by intial assignment
        private int MyInt;
        public int MyIntProperty
        {
            //get and set are known as accesors, used to control the access level of the property
            //Omit one or the other to create readonly or writeonly data 
            // omiting - applie to external code, class code can still access
            //You can put accesibility modifiers on property and the accesors of the property making for instance get public
            //while set proteceted
            //Properties can use the virtual, override, and abstract keywords unlike fields
            //You must include at least one of these blocks to create a valid property
            //The accessibilities that are permitted for accesors depend on the accesibility of the property
            //Forbidden to make accesor more accesible than the property to which it belongs
            protected get
            {
                //Property get code
                //get blocks must have a return value of the type of the property
                //simple properties associated with private fields of class, controlling access to that field
                return MyInt;
            }
            set
            {
                //Propert set code
                if (value >= 0 && value <= 10)
                    MyInt = value;//use keyword value to refer to th value recieved from user of the property
                else//If invalid value inputted, throw, or log event and do nothing
                    throw new ArgumentOutOfRangeException("MyIntProp", value,
                        "MyIntProperty must be assigned val bet 0 and 10");

            }
        }
        private int myDoubledInit = 5;
        //C# 6 introduced feature called expression based properties
        //property
        public int MyDoubledIntProp => (myDoubledInit * 2);

        public override string ToString()
        {
            return "poopoo";
        }

        static MyClass() //Static construtor
        {

            numOfClassInstances = 0;
        }

        public MyClass()
        {
            //No parameters so default constructor for class

        }


        public MyClass(int myInt)
        {

            //Nondefault constructor code
        }

        ~MyClass()
        {

            //Destructor Body
            //Finalize() is System.Object's destructor
            //Executed when garbage collection occurs
            //After this occurs, implicit calls to the 
            //destructoors of base classes and finally call to Finalize by System.Object
        }
        public string GetString() => "Wazz good bruh";
        //Other method keywords 
        public virtual void DoSomethine()
        {
            //virtual means method can be overriden
        }
        //Method kewyords: virtual, override, abstact, extern
        //extern - The method definition is found elsewhere

    }
    //same as
    internal class MyClass2
    {
        //internal:  Only code in current project will have access to them

        private int fartsszz;

        private MyClass2()
        {
            //private constructor non-creatable
        }

        public int Fartsszz
        {
            get{return fartsszz;}
            set{fartsszz = value;}
        }
    }

    public class MyClass3
    {
        //opposite of internal, accessible to code in other projects

        public MyClass3() : this(3, 5)
        { // this keyword instructs to use nondefault 
            //constructor on the current class before the specified constructor is called

        }
        public MyClass3(int i, int j)
        {
        }

        public int MyIntProp { get; set; } //Automatic property
        //You define the accessibility, type, and name of the property in the usual way
        //but don't provide any implementation for the get or set block and compiler provides
        //the usual implementation of these blocks(and the underlying field).
        //Limitiations are you cant define read- or write- only properties this way
        //You can only create an externally read only property as follows:

        public int MyIntProp2 { get; private set; }

        //Prior to C#6 automatic properties requried setters which limited the utilization of immutable data types
        //immutable types such as System.String
        //getter-only auto properties
        public int MyIntPropC6 { get; }
        //initialization of auto properties:
        public int MyIntPropPoo { get; } = 9;
    }

    //NonCreateableClasses are still useful through the static members they possess
    //in this case static member used to return instance of noncreatable class 
    class CreateMe
    {
        private CreateMe() { }

        static public CreateMe GetCreateMe()
        {
            return new CreateMe();
        }


    }

    public abstract class MyBase
    {
        //can use either acces modifier
        private int x;

        protected MyBase() { }
        protected MyBase(int i)
        {
            x = i;
        }
        public abstract int fart();//abstract method- method must be overriden in derieved class

       public virtual void DoSomething()
        {
            //virtual - method can be overriden
        }
    }

    public sealed class MyClass4
    {
        //can use either access modifier
        //sealed means cannot be inherited
    }

    public class MyClass5 : MyBase //inheritence
    {
        public MyClass5()
        {

            //You can configure any constructor to call 
            //any other constructor before it executes its own code
            //For derieved class to be instantiated, base class 
            //must be instantiated, and that''s base class must be 
            //instantiated up to System.Objet()
            //Unless you specify otherwise, default constructor 
            //of base class used in derieved class

            //instantiating MyClass5 by typing MyClass5 obj = new MyClass5(); will
            //cause System.Object.Object() constructor to execute,
            //then MyBase constructor default, then this constructor. 

        }

        //Class Members
        //Only one base class
        public sealed override void DoSomething()
        {
            //Derieved class implementation that overrides base class
            //The sealed keyword also adds this effect: Specify no further modification can be made to this 
            //method in the derieved class
            base.DoSomething();
        }

        public override int fart()
        {
            return 3;

        }
    }
    public class MyClass6 : MyBase
    {

        public MyClass6() : this(3) { }
        public MyClass6(int i) : base(i) { }//costructor initializer, calls that base constructor

        public override int fart()
        {
            return 3;

        }
    }


    //If base class public, then derived class can be either public/internal
    //If base class is internal, the derived class can only be interna;
    //All objects have base class System.Object


    public interface BaseInterface
    {
 
        void InterfaceKill();
    }

    public interface IMyInterface1 : BaseInterface
    {
        //Interface Members

        //No access modifiers are all, all members are implicitely public, private, protected, internal
        //Interface members can't contain code bodies or define field members
        //Interface members cant use static, virtual, abstract or sealed keywords
        //Type definition members are forbidden
        //You can use new if you wna to hide memebers inherited from base interfaces
        //Like This:
        new void InterfaceKill();
        //properties defined in interfaces define either or both access blocks
        int MyInt { get; set; } //cannot specify fields to store property data
        //This sytax similar to automatic properties except automtic prperties are def ined for classes
        //not interfaces, and automatic properties must have both get and set accesors
            
        int MyInt2 { get; }

        //interfaces can be defined as members of classes, but interfaces cannot nest in other interfaces
        //because interfaces cannot contain type definitions

        void InterfaceFun();
        int timeToParty();
        int explicitelyImplementMehPls();

    }

    interface IMyInterface2
    { //keyword internal by default just like classes
        //Interface Members, 
        //cant use abstract or sealed keywords
    }

    interface IMyInterface4
    { 
    }

    public class MyBaseBase {
        public virtual int fart() {
            return 4;
        }
        public int timeToParty() {
            return 13;
        }

    }

    public class MyClass7 : MyBaseBase, IMyInterface1, IMyInterface2
    {
        private int yummy;
        public int whatever() { return 3; }
        public int MyInt
        {
            get{return yummy;}
            set{ yummy = value;}
        }
            
        //in interface we just gave this property a get block
        //we can add the set block, but only if we implement 
        //implicitely, 
        //Most cases you want this accesor to limit its accessibility
        //If you define additional accesor as public, then code with access to the 
        //class implementing the interface can access it, however, code that has access
        //only to the interface wount be able to access it. 
        public int MyInt2 { get; protected set; }

        //Put base class before interfaces.
        public override int fart()
        {return 3;}

        //Implement all interface functions,
        //match the signitures specified, including matching
        //the specified get and set blocks, and must be public
        public virtual void InterfaceKill() { }//Interface members cant use const or static
        public virtual void InterfaceFun() { } //Interface members can use virtual or abstract keywords
                                               //Dont have to implement interface member TimeToParty because base class does it

        //Interface members can also be implemented explicitely, then member can only be accessed 
        //by the interface not the class. Implicity doent matter.
        //Explicit implmentation
      int IMyInterface1.explicitelyImplementMehPls() {//modifiers like public or virtual not used in explicit
            return 5;
        }
    }

    public class MyClass8 : MyClass7 {
        //Inheriting from base class that implements an interface
        //means that the interface is implicitely supported by the derieved class
        //Useful to define base class interface implementations virtual so derived class can 
        //replace implementation rather than hide it 
        //If you hide base class member using new, method IMyInterface.InterfaceFun() would
        //always refer to the base class version even if the derieved class were being accesed
        //via the interface
        public override void InterfaceFun()
        {   }//These are implicit implementations
    }


    internal interface IMyInterface3 : IMyInterface1, IMyInterface2
    {
        //interface inheritence where multiple interfaces can be used. 
    }

    enum orientation : byte
    {
        South = 0,
        North = 1,
        West = 2,
        East = 3
    }
    //Structs are value types and Classes are references types

    class MyyClass
    {

        public int val;
    }

    struct MyyStruct
    {

        public int val;
    }

    //Hiding Base Class Members:

    public class MyBaseClass
    {

        public int num { set; get; } = 0;
        public virtual int add(int x, int y) => x + y;
    }

    public class derievedClass1 : MyBaseClass
    {
        //regardless of whether the base class method is virtual or not, you can,
        //if you want, hide the implementation
        //useful when public inherited member doesnt work quite as you want it to. 

        new public int add(int x, int y)
        { //you dont need new, but adding new will stop warning 
            base.add(x, y); //you can still access base class method doesnt matter whther you hide or override
            return x * y;
            //As base works using object instances , error to use from static member.Same rule applies for this.
            //So you cant use this and base in in static members because 
            //static members are not part of an object instance)
            //can also use "this" which refers to this object instance. 
            this.add(x, y); //this would cause infinite recursion good thing we return beforehand
            //using this allows polymorphic behaviour.


        }
        //Will compare hiding and overriding in main
    }

    public class derievedClass2 : MyBaseClass
    {
        public override int add(int x, int y)
        {
            return x * y;
        }
    }

    //Nested Classes
    public class outsideClass
    {
        public class nestedClass
        {
            public int nestedClassField;
        }
    }
    //You can instantiate if nested class is public like this:(cant if nested is private)
    //outsideClass.nestedClass myObj = new outsideClass.nestedClass();
    //nested classes have access to privated and protected members of their containing class

    //Partial Class definitions - split definition of a class across multiple files
    //put fields, properties and constructor in one field, , and methods in another
    //Use partiial keyword in each file and each class divison
    //Interface applied to one partial class  part applies to whole class
    public partial class myPartialClass : IMyInterface2
    {
        partial void MyPartialMethod();//Partial methods can be declared in one partial
                                       //and defined in other partial
        //Partial methods can also be static, but they are always private and cant return a value
        //Any parameters they use cant be out, but they can be ref
        //Also cant use the virtual, abstract, override, new, sealed, or extern modifiers
        //When you compile code that contains a partial method declaration without an implementation
        //And it is being used somewhere, the compiler actually removes the method entirely
        //It also removes any calls to the method => performance boosted.
        //So you kinda can make them an optional addition to a program with performance boosting 
        //if not included
    }
    public partial class myPartialClass : IMyInterface4
    {
        partial void MyPartialMethod()
        {//Method Implementation}
        }
        //Is Equivalent to => public class myPartialClass: IMyInterface2, IMyInterface4{}
        //Only one base class, and has to be same base class for each partial, 
        // doesnt have to be put on all of the partials

        // /////////////////////////////////////////////////////////Program Starts here

        class Program
    {
        enum orientation : byte //enums can be inside or outside the Program Class, doesnt matter
        {
            North,
            South,
            East,
            West
        }


        static string[] eTypes = { "none", "simple", "index",
                                 "nested index", "filter" };

        public class classA
        {
            private int state = -1;
            public int State { get { return state; } }
            public class classB
            {
                public void SetPrivateState(classA target, int newState)
                {
                    target.state = newState;
                }
            }
        }



        static void Main(string[] args)
        {   //difference between overridin and hiding?

            //implicit vs explicit implementation of interface memebers
            MyClass7 obj7 = new MyClass7();
            obj7.InterfaceKill(); //implicitely defined but cant access function implementMeh
            obj7.whatever();//method declared and define in class
            IMyInterface1 myInt = obj7;
            myInt.InterfaceKill();
            myInt.explicitelyImplementMehPls(); //Now we can call it!!!
            //myInt.whatever(); interface code cant access this

            MyBaseClass zuluHide = new derievedClass1(); //Hiding base class implementation
            MyBaseClass zuluOverride = new derievedClass2(); //Overriding

            Console.WriteLine($"Hiding: {zuluHide.add(2, 5)}"); //Ran base class method, not polymorphic
            Console.WriteLine($"Hiding: {zuluOverride.add(2, 5)}"); //called polymorphic overriden method

            classA A = new classA();
            Console.WriteLine($"{A.State}");
            classA.classB B = new classA.classB();
            B.SetPrivateState(A, 999);  //This brings up another point however, how were we able to change the
            //object if it was pass by value and not pass by reference?
            Console.WriteLine($"{A.State}");
            ReadKey();
            //Answer:
            /*
             Since classA is a class (which are reference objects), 
             you can change the contents inside A without passing it as a ref. 
             However, if you pass A as a ref, SetPrivateState can change what the original 
             A refers to. i.e. make it point to a different object.
            
            A reference type is type which has as its value a reference to the appropriate data
            rathar than the data itself
            StringBuilder sb = new StringBuilder();
            sb, create a new string builder object, and assign sb as a reference to the object
            class, interface,delegate, array types are all reference types
            structs and enums are value types
            While reference types have layer of indirection between variable and real data,
            value type dont. variables of value type directly contain data.
            4 types of parameters in functions, default which is value, 
            reference ref, output out, and parameter arrays(params)
            You can use any of them with both value and reference types!
            By default parameter value type, new storage location created
            for the variable in the function member declaration and it starts off
            with the value that you specify in the function member invocation.
  ////////////////////////////////////
          void Foo (StringBuilder x){
                x = null;
            }

            StringBuilder y = new StringBuilder();
            y.Append("hello");
            Foo(y);
            Console.WriteLine(y == null); //This is false because x is a copy of what y refers  but now x refers to null
            //if x was a ref parameter, then this would be true because they 
            dont pass the values of variables, they use the variables themselves
            //The same storage locaiton is used rather than creating a new storage location

/////////////////////////////////////////////////
            void Foo (StringBuilder x){
                x.Append(" world");
            }

            StringBuilder y = new StringBuilder();
            y.Append("hello");
            Foo(y);
            //When you pass y which is a class so reference type, do not think of this as
            object being passed by reference, but object reference passed by value
            Console.WriteLine(y); //returns "hello world", 
            //if y was value type, y would remain "hello" unless it was a ref parameter,
           //then even if y was value type it would be "hello world"

            Output parameters are very similar to reference parameters. The only differences are:
            The parameter must be assigned a value before the function member completes normally.

            /////////////////////////////////////////////////
 
             */

            //System.Object members
            //all objects have a ToString() method which is a method defined in System.Object and which
            //derieved classes can override to output something else. By default, ToString() method 
            //returns class name of the object as a string qualified by any relevant namespaces. 
            //Object Methods:
            //Object() => Constructor
            //~Object() => Destructor
            //static virtual bool Equals(object,object) => checks whther the object paramter refers to the same object
            //override for real check of states
            //static bool ReferenceEquals(object,object) ==> checks whther they are references to the same instnace
            //virtual string ToString() overide pls
            //object MemberwiseClone() ==> protected method can only be used used from within the class or derieved classes
            //member copies references which lead to references to samee objects
            //System.Type GetType() //returns type in form of System.Type object
            //virtual int GetHashCode() //Used as a hash function for objects where this is required. A hash function
            //returns a value identifying the object state in some compressed form.
            Stopwatch watch = new Stopwatch();
            watch.Start();

            //Using typoof and getType =>
            object fun = new MyClass3();
            if (fun.GetType() == typeof(MyClass3))
            {
                Console.WriteLine("They are the same");
            }

            MyClass myObj = new MyClass();
            for (int i = -1; i <= 0; i++)
            {
                try
                {
                    myObj.MyIntProperty = i;
                }
                catch (Exception e)
                {

                    WriteLine($"Exception {e.GetType().FullName} thrown.");
                    WriteLine($"Message: \n\"{e.Message}\"");
                }

            }


            MyClass3 obj3 = new MyClass3();
            obj3.MyIntProp = 5;
            Console.WriteLine($"Automatic Properties are kwel {obj3.MyIntProp}");

            //Classes are reference types and structs are value types
            MyyClass objectA = new MyyClass();
            MyyClass objectB = objectA;//When you assign an object to var, you are actually assigning var with a pointer to which the object refers
            objectA.val = 10;
            objectB.val = 20;
            MyyStruct structA = new MyyStruct();
            MyyStruct structB = structA;//Copying all the information from one struct to another
            structA.val = 10;
            structB.val = 20;

            WriteLine($"objectA val: {objectA.val}");
            WriteLine($"objectB val: {objectB.val}");
            WriteLine($"structA val: {structA.val}");
            WriteLine($"structB val: {structB.val}");

            //TO implement deep copying use interface ICloneable and define function Clone() which returns System.Object


            orientation direction = orientation.North;

            bool success = false;
            do
            {
                try
                {
                    WriteLine("What direction do you want to go (1,2,3,4)");
                    string input = ReadLine();
                    byte number = Convert.ToByte(input);
                    if (number < 0 || number > 4)
                    {
                        throw new System.Exception();
                    }
                    direction = checked((orientation)number);
                    success = true;
                }
                catch (System.InvalidCastException e)
                {
                    WriteLine("Please try again");
                }
                catch (System.FormatException e)
                {
                    WriteLine("Please try again");
                }
                catch
                {
                    WriteLine("Please Try AGGAIN");
                }
            } while (success == false);

            WriteLine("Direction is {0}", direction);




            foreach (string eType in eTypes)
            {
                try
                {
                    WriteLine("Main() try block reached.");        // Line 21
                    WriteLine($"ThrowException(\"{eType}\") called.");
                    ThrowException(eType);
                    WriteLine("Main() try block continues.");      // Line 24
                }
                catch (System.IndexOutOfRangeException e) when (eType == "filter")            // Line 26
                {
                    BackgroundColor = ConsoleColor.Red;
                    WriteLine($"Main() FILTERED System.IndexOutOfRangeException catch block reached. Message:\n\"{e.Message}\"");
                    ResetColor();
                }
                catch (System.IndexOutOfRangeException e)              // Line 32
                {
                    WriteLine($"Main() System.IndexOutOfRangeException catch block reached. Message:\n\"{e.Message}\"");
                }
                catch                                                    // Line 36
                {
                    WriteLine("Main() general catch block reached.");
                }
                finally
                {
                    WriteLine("Main() finally block reached.");
                }
                WriteLine();
            }

            ReadKey();
            watch.Stop();
            WriteLine($"You spent this time on program: {watch.Elapsed}");

        }

        static void ThrowException(string exceptionType)
        {
            WriteLine($"ThrowException(\"{exceptionType}\") reached.");

            switch (exceptionType)
            {
                case "none":
                    WriteLine("Not throwing an exception.");
                    break;                                               // Line 57
                case "simple":
                    WriteLine("Throwing System.Exception.");
                    throw new System.Exception();                        // Line 60
                case "index":
                    WriteLine("Throwing System.IndexOutOfRangeException.");
                    eTypes[5] = "error";                                 // Line 63
                    break;
                case "nested index":
                    try                                                  // Line 66
                    {
                        WriteLine("ThrowException(\"nested index\") " +
                                   "try block reached.");
                        WriteLine("ThrowException(\"index\") called.");
                        ThrowException("index");                          // Line 71
                    }
                    catch                                                // Line 66
                    {
                        WriteLine("ThrowException(\"nested index\") general"
                                          + " catch block reached.");
                        throw; //THIS WAS THE KEYWORD I WAS MISSING FOOOKOKOK
                    }
                    finally
                    {
                        WriteLine("ThrowException(\"nested index\") finally"
                                          + " block reached.");
                    }
                    break;
                case "filter":
                    try                                                  // Line 86
                    {
                        WriteLine("ThrowException(\"filter\") " +
                                  "try block reached.");
                        WriteLine("ThrowException(\"index\") called.");
                        ThrowException("index");                          // Line 91
                    }
                    catch                                                // Line 93
                    {
                        WriteLine("ThrowException(\"filter\") general"
                                         + " catch block reached.");
                        throw;
                    }
                    break;
            }
        }
    }
}