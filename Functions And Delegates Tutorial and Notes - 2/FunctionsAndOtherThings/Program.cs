using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;//Used for Debug.WriteLine which work just in Debug mode, 
//Trace.WriteLine also works in debug and release mode
using System.Threading.Tasks;

namespace FunctionsAndOtherThings
{
    class Program
    {

        static string myString = "myString in global scope";
        const int car = 5;
        //The constant poo cannot be marked static => static const int poo = 3;

        struct CustomerName
        {
            public string firstName, lastName;
            public string fullName() => firstName + " " + lastName;//function in struct

        }


        static void Write()
        {
            string myString = "myString in Write()";
            Console.WriteLine($"Local: {myString}");
            Console.WriteLine($"Global: {Program.myString}"); //Program. fully classifies the name
            return;
        } //When global and local variable have same name, global variable "hidden"

        static void WriteScope()
        {

            int i = 0;
            string text; //memory not allocated
            for (i = 0; i < 10; ++i)
            {
                text = "Line " + Convert.ToString(i);
                Console.WriteLine(text);
            }
            //Console.WriteLine(text); cant write this 
            // cause it wasnt initialized outside for loop scope so its not initialized
            Console.WriteLine();
            string text2 = ""; //memory allocated
            for (i = 0; i < 10; ++i)
            {
                text2 = "Line " + Convert.ToString(i);
                Console.WriteLine(text2);
            }
            Console.WriteLine(text2);//Now you can write it

        }

        static int maxVal(int[] arr, out int maxIndex)
        {

            int maxVal = arr[0];
            maxIndex = 0;
            for (int i = 1; i != arr.Length; ++i)
            {

                if (maxVal < arr[i])
                {
                    maxVal = arr[i];
                    maxIndex = i;
                }
            }
            return maxVal;
        }

        static double maxVal(double[] arr, out double maxIndex) //Overload - You cant overload with return type
        {

            double maxVal = arr[0];
            maxIndex = 0;
            for (int i = 1; i != arr.Length; ++i)
            {

                if (maxVal < arr[i])
                {
                    maxVal = arr[i];
                    maxIndex = i;
                }
            }
            return maxVal;
        }

        static int sums(params int[] vals)
        {//params array must be last parameter 
            //in function definition and allows unlimited parameters

            int sum = 0;
            foreach (int num in vals)
            {
                sum += num;
            }
            return sum;
        }

        static void add1(ref int val)
        { // cant use constant variables or uninitialized values for ref parameter
            val++;
            return;
        }


        delegate double ProcessDelegate(double param1, double param2); //Function Pointer Type Declaration

        delegate string ReadsReadsReads();

        static double Multiply(double myVal1, double myVal2) => myVal1 * myVal2;
        static double Divide(double myVal1, double myVal2) => myVal1 / myVal2;

        static void outputSomeMaths(double myVal1, double myVal2, ProcessDelegate process)
        {
            Console.Write($"{myVal1} operation with {myVal2} is {process(myVal1, myVal2)}\n");
        }


        static void quickSortRecur(ref int[] arr, int start, int end, int pivot)
        {
            Debug.Assert(start < end, "start is greater than end", "Assertion occured in quickSort");
            Trace.Assert(start < end, "start is greater than end", "Assertion occured in quickSort");

            int newPivotLocation = start;

            for (int i = start; i != end; ++i)
            {
                if (arr[pivot] > arr[i]) //Tracepoint Diamon
                {
                    ++newPivotLocation;
                }
            }

            swap(ref arr, pivot, newPivotLocation); //BreakPoint

            int largerCur = newPivotLocation + 1;
            int smallerCur = start;
            
            for (int i = start; i != end; ++i)
            {
                if (arr[i] > arr[newPivotLocation] && i < newPivotLocation)
                {
                    swap(ref arr, i, largerCur);
                    ++largerCur;
                    --i;
                }
                else if (arr[i] < arr[newPivotLocation] && i > newPivotLocation)
                {
                    swap(ref arr, i, smallerCur);
                    ++smallerCur;
                    --i;
                }


            }


            if (newPivotLocation + 1 != end)
            {
                quickSortRecur(ref arr, newPivotLocation + 1, end, newPivotLocation + 1);
            }

            if (start != newPivotLocation)
            {
                quickSortRecur(ref arr, start, newPivotLocation, start);
            }

        }

        static void swap(ref int[] arr, int index1, int index2)
        {
            int temp = arr[index1];
            arr[index1] = arr[index2];
            arr[index2] = temp;
        }


        static void Main(string[] args)
        {
            ProcessDelegate process;
            ReadsReadsReads read = new ReadsReadsReads(Console.ReadLine);
            double p1, p2;
            string input;
            Console.WriteLine("Please input 2 integers seperated by a comma");
            input = Console.ReadLine().Trim(new char[] { ' ' });
            p1 = Convert.ToDouble(input.Substring(0, input.IndexOf(',')));
            p2 = Convert.ToDouble(input.Substring(input.IndexOf(',') + 1, input.Length - input.IndexOf(',') - 1));
            Console.WriteLine("NoW PLEASE GIVE OPERATION OF EITHER M OR D");
            if (read() == "M")
            {
                process = new ProcessDelegate(Multiply);
            }
            else
            {
                process = new ProcessDelegate(Divide);
            }

            outputSomeMaths(p1, p2, process);


            int[] arr = { 10, 3,5,3,7,12, 2, 8, 3, 6,32, 4, 3,9,6 }; //Pinned Tooltip
            //quickSortRecur(ref arr, arr.Length, 0, 0); Assertion fail
            quickSortRecur(ref arr, 0, arr.Length, 0);

            foreach (int j in arr){
                Console.Write($",{j} ");
            }
            Console.WriteLine();


            Console.WriteLine("There " + ((args.Length < 2) ? "is" : "are") + $" {args.Length} argument(s). They are: ");
            foreach (string argument in args)
            {
                Console.WriteLine(argument);
            }


            WriteScope();

            string myString = "myStrin in main()";
            Console.WriteLine("Local is {0}", myString);
            Console.WriteLine("Global is " + Program.myString);
            Write();


            int[] numbers = { 1, 2, 12, 3, 4, 5 };


            int maxIndex;
            int max = maxVal(numbers, out maxIndex); //value of out parameter at end of execution 
                                                     //is returned to the variable used in the function call 
                                                     //you can use unassigned parameter in variable call unlike ref for out
                                                     //out parameter treated as an unassigned value, or if you use an assigned variable, its value lost
                                                     //after function execution completes

            Console.WriteLine($"Max Value  occurs {maxIndex} and max val is {max}");
            Console.WriteLine(sums(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            Console.WriteLine(sums(12, 23, 21));
            int val = 10;
            add1(ref val);//Had to initialize val to use as reference parameter. Cannot do int val; and then pass val. 
            Console.WriteLine(val);


        }
    }
}
