using UnityEngine;

namespace ProjetSynthese
{
    public abstract class Item : NetworkGameScript
    {
        [Tooltip("Le type de l'item.")]
        [SerializeField]
        private ItemType type;

        private GameObject player;
        public GameObject Player
        {
            get { return player; }
            set
            {
                player = value;
            }
        }

        public int Level { get; set; }

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
        public abstract int GetWeight();

        public static bool operator ==(Item item1, Item item2)
        {
            if ((object)item1 == null && (object)item2 == null)
            {
                return true;
            }

            if ((object)item1 == null || (object)item2 == null)
            {
                return false;
            }
            if ((item1 as AmmoPack) && item2 as AmmoPack)
            {
                return (item1 as AmmoPack).AmmoType == (item2 as AmmoPack).AmmoType;
            }
            return item1.type == item2.type;
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !(item1 == item2);
        }

        public override bool Equals(object obj)
        {
            var item = obj as Item;
            return item != null &&
                   base.Equals(obj) &&
                   type == item.type &&
                   Type == item.Type;
        }

        public override int GetHashCode()
        {
            var hashCode = -747960638;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
    }
}
