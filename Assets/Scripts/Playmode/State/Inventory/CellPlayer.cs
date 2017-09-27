using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjetSynthese
{
    public class CellPlayer : Cell
    {
        private Item item;

        public CellPlayer()
        {

        }

        public override void SetItem(GameObject game)
        {
            item = game.GetComponent<Item>();
        }

        public override Item GetItem()
        {
            return item;
        }

    }
}
