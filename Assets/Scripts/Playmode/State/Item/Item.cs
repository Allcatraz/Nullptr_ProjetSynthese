using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public abstract class Item : NetworkGameScript
    {
        public ItemType Type { get; set; }
        public abstract void Use();


        public static bool operator==(Item item1, Item item2)
        {
            if ((object)item1 == null || (object)item2 == null)
                return false;

            return item1.Type == item2.Type;
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !(item1 == item2);
        }
    }
}
