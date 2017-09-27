﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjetSynthese
{
    public class CellItem : Cell
    {

        private Item item;

        public CellItem()
        {

        }

        public CellItem(Item item)
        {
            this.item = item;
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
