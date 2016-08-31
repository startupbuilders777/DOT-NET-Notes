using System;

namespace Ch05Ex02
{

	enum orientation : byte {
		North = 1,
		South = 2,
		East = 3,
		West = 4,
		south = South
	}

	struct route{

		public orientation direction;
		public double distance;

	} 

	class MainClass
	{
		public static void Main (string[] args)
		{

			double number = 3.14;
			int number2 = (int)number;
			float floatingNum = checked((float)number);
			Console.WriteLine (Convert.ToString("Hello World!"));
			orientation direction = orientation.North;
			Console.WriteLine (Convert.ToString (direction));
			Console.WriteLine ("This is the number {0}", floatingNum);
			Console.WriteLine ((orientation)2);

			string myString = "North";
			direction = (orientation)Enum.Parse (typeof(orientation), myString);
			Console.WriteLine (direction);

			route myRoute;
			myRoute.direction = orientation.North;
			myRoute.distance = Convert.ToDouble (Console.ReadLine());

			Console.WriteLine("myRoute specifies a direction of {0}" + " and a distance of {1}", myRoute.direction, myRoute.distance);

			int[] myIntArray1 = { 1, 2, 3, 4 };
			int[] myIntArray2 = new int[5];
			int[] myIntArray3 = new int[3]{ 1, 2, 3 };

			const int ArraySize = 3; //must be const if specifying array size
			int[] myIntArray4 = new int[ArraySize]{1,2,3};

			Console.WriteLine (myIntArray4.Length);

			string[] names = { "John", "Mike", "Susan" };
			foreach(string name in names){
				Console.WriteLine (name);
			}


		}
	}
}
