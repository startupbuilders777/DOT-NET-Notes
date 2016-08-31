using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;
using static System.Convert;

namespace ArraysEnumsStructsConversions
{

    enum orientation : byte
    {
        North = 1,
        South = 2,
        East = 3,
        West = 4,
        south = South
    }

    struct route
    {
        public orientation direction;
        public double distance;
    }

    
    class Program
    {
        
        static void Main(string[] args)
        {
            

            double number = 3.14;
            int number2 = (int)number;
            float floatingNum = checked((float)number);
            Console.WriteLine(Convert.ToString("Hello World!"));
            orientation direction = orientation.North;
            Console.WriteLine(Convert.ToString(direction));
            Console.WriteLine("This is the number {0}", floatingNum);
            Console.WriteLine((orientation)2);

            string myString = "North";
            direction = (orientation)Enum.Parse(typeof(orientation), myString);
            Console.WriteLine(direction);

            route myRoute;
            myRoute.direction = orientation.North;
            myRoute.distance = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("myRoute specifies a direction of {0}" + " and a distance of {1}", myRoute.direction, myRoute.distance);

            int[] myIntArray1 = { 1, 2, 3, 4 };
            int[] myIntArray2 = new int[5];
            int[] myIntArray3 = new int[3] { 1, 2, 3 };

            const int ArraySize = 3; //must be const if specifying array size
            int[] myIntArray4 = new int[ArraySize] { 1, 2, 3 };

            Console.WriteLine(myIntArray4.Length);

            string[] names = { "John", "Mike", "Susan" };
            foreach (string name in names)
            {  //foreach gives you read-only access only to the array contents
                Console.WriteLine(name);
            }

            double[,] hillHeight = new double[3, 4]; // 3 rows and 4 cols and multidimensional array
            double[,] hillHeight2 = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 10, 11, 12, 13 } }; //3 rows and 4 cols

            foreach (double height in hillHeight2) {
                //The height of the hills goes from hillHeight[0,0] to hillHeight[0,1]...hillHeight[2,3]
                WriteLine($"height: {height}");
            }

            //Arrays of Arrays which can form jagged arrays

            int[][] jaggedIntArray;
            //cant initialize like this jaggedIntArray = new int[3][4]
            //nor can you do this jaggedIntArray = {{1,2,3},{1},{1,2}};
            //2 options

            jaggedIntArray = new int[2][];
            jaggedIntArray[0] = new int[3];
            jaggedIntArray[1] = new int[4];
            //or
            int[][] jaggedIntArray2 = { new int[] { 1, 2, 3 }, new int[] { 1 }, new int[] { 1, 2 } };

            //To use foreach with these you have to nest

            int[][] divisors = {
                new int[] { 1},
                new int[] { 1,2},
                new int[] { 1,3},
                new int[] { 1,2,4},
                new int[] {1, 5},
                new int[] { 1,2,3,6} };

            foreach (int[] div in divisors) {
                foreach (int divisor in div) {
                    WriteLine($"Divisor is {divisor}");
                }
             }

            string myString2 = "A String";
            char letterOfMyString = myString2[0];
            char[] myChars = myString2.ToCharArray();
            myString = myString2.ToLower();
            string myStringUpper = myString2.ToUpper();

            char[] trimThis = { ' ', 'u', 'n'};
            string trimmingsNeeds = "  unufunnyBunnyuu    ";
            WriteLine(trimmingsNeeds.Trim());
            WriteLine(trimmingsNeeds.Trim(trimThis));
            WriteLine(trimmingsNeeds.TrimStart()); //can also have char array specified
            WriteLine(trimmingsNeeds.TrimEnd());//can also have char arrays specified

            string fun = "cards";
            fun = fun.PadLeft(6, 'x'); // the first number is the total length so thisll make xcards
            fun = fun.PadRight(12, 'y'); // will cause fun to be length of 12
            Console.WriteLine( fun + fun);

            string splitMeh = "This is a test.unomsayin . b . bb ..  bbb..";
            char[] splitPoints = { ' ', '.' };
            string[] splitted = splitMeh.Split(splitPoints);

            foreach (string word in splitted) {
                WriteLine(word);
            }

            int[] arr = { 3, 4, 5 };
            //Resize and put another element
            int[] oldarr = arr;
            arr = new int[4];
            oldarr.CopyTo(arr, 0);
            arr[3] = 6;
            foreach (int i in arr) {
                Console.WriteLine(i);
            }







        }
    }
}
