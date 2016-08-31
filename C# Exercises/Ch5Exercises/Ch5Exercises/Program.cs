using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch5Exercises
{
    class Program
    {
        static void Main(string[] args)
        {

            string fun;
            Console.WriteLine("Please give string");
            fun = Console.ReadLine();
            char[] funArr = fun.ToCharArray();

            for (int i = 0; i != fun.Length / 2; ++i) {
                char temp = funArr[i];
                funArr[i] = funArr[fun.Length - 1 - i];
                funArr[fun.Length - 1 - i] = temp;
            }
            string revString = new string(funArr);//returns System.Char[]

            Console.WriteLine($"Reversed String is {revString}");


            string myString;
            Console.WriteLine("Gimmie another string");
            myString = Console.ReadLine();
            myString = myString.Replace("no", "yes");
            Console.WriteLine(myString);

            //puts double Quotes around each word in a string

            char[] arr = { ' ' };
                       
            Console.WriteLine("Give me a third string");
            myString = Console.ReadLine();
            string[] StringArr = myString.Split(arr);
            string newString = "";

            foreach (string word in StringArr) {
                newString += " \"" + word + "\"";
                    }
            newString = newString.Remove(0,1);

            Console.WriteLine(newString);

        }
    }
}
