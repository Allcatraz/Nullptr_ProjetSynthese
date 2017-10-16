using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public abstract class Item : NetworkGameScript
    {
        [SerializeField]
        private ItemType type;

        
        public ItemType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        
        

        public abstract void Use();


        public static bool operator==(Item item1, Item item2)
        {
            if ((object)item1 == null && (object)item2 == null)
            {
                return true;
            }

            if ((object)item1 == null || (object)item2 == null)
            {
                return false;
            }
            return item1.type == item2.type;
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !(item1 == item2);
        }
    }
}
