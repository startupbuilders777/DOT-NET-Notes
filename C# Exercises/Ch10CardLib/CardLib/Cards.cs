using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch10CardLib;
using System.Collections;

namespace Ch11
{
    class Cards: CollectionBase
    {
        public void Add(Card newCard) {
            List.Add(newCard);
        }
        public void Remove(Card oldCard) {
            List.Remove(oldCard);
        }
        public Card this[int cardIndex] {
            get { return (Card)List[cardIndex]; }
            set { List[cardIndex] = value; }
        }
        /// <summary>
        /// Utility method for copying card instances into another Cards instance used
        /// in Deck.Shuffle(). 
        /// Requires: source and target collections same size
        /// </summary>
        /// <param name="targetCards"></param>
        public void CopyTo(Cards targetCards) {
            for (int index = 0; index != this.Count; index++) {
                targetCards[index] = this[index];
            }
        }

        /// <summary>
        ///Checks to see if card in cardCollection 
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool Contains(Card card) => InnerList.Contains(card);

    }
}
