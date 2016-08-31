using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch10CardLib;
using System.Threading.Tasks;
using static System.Console;
namespace Fun
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck myDeck = new Deck();
            myDeck.Shuffle();
            for (int i = 0; i != 52; ++i)
            {
                Card tempCard = myDeck.GetCard(i);
                if (i % 5 == 0)
                    WriteLine();
                Write(tempCard.ToString());
                if (i != 51)
                    Write(", ");
                else
                    WriteLine();
                

            }

            for (int i = 0; i != 50; ++i) {

                bool flush = true;
                Suit curSuit = myDeck.GetCard(i).suit;
                ++i;
                while (i % 5 != 0 ) {
                    if (myDeck.GetCard(i).suit != curSuit)
                    {
                        i += (5 - i % 5);
                        flush = false;
                        break;
                    }
                        ++i;
                }
                if (flush) {
                    WriteLine("Flush!!!!");
                    break;
                } else {
                    WriteLine("No FLush!");
                }
                if (i == 50)
                    break;
            }
            ReadKey();
        }
    }
}
