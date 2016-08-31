using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch10CardLib
{
    public class Card
    {
        public readonly Rank rank;
        public readonly Suit suit;

        private Card()
        {
            throw new System.NotImplementedException();
        }

        public Card(Suit newSuit, Rank newRank)
        {
            suit = newSuit;
            rank = newRank;
        }

        public Rank Rank
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public Suit Suit
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public override string ToString()
        {
            return "The " + rank + " of " + suit + "s";
        }
    }
}