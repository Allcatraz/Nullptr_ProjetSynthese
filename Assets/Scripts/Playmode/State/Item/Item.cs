using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public abstract class Item : GameScript
    {
        ItemType type;
        public abstract void Use();


        public static bool operator==(Item item1, Item item2)
        {
            return item1.type == item2.type;
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !(item1 == item2);
        }
    }
}


