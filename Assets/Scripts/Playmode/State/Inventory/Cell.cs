using UnityEngine;

namespace ProjetSynthese
{
    public class ObjectContainedInventory
    {
        private int compteur;

        public ObjectContainedInventory()
        {
            compteur = 1;
        }

        public void AddCompteur()
        {
            compteur++;
        }

        public int GetCompteur()
        {
            return compteur;
        }

        public void RemoveOneFromCompteur()
        {
            compteur--;
        }

        public virtual void SetItem(GameObject game)
        {
        }

        public virtual Item GetItem()
        {
            return null;
        }

        public virtual void SetImage()
        {

        }

        public static bool operator ==(ObjectContainedInventory one, ObjectContainedInventory two)
        {
            if ((object)one == null && (object)two == null)
            {
                return true;
            }
            if ((object)one == null || (object)two == null)
                return false;
            return one.GetItem() == two.GetItem();
        }

        public static bool operator != (ObjectContainedInventory one, ObjectContainedInventory two)
        {
            return !(one == two);
        }

        public override bool Equals(object obj)
        {
            var cell = obj as ObjectContainedInventory;
            return cell == this;
        }

        public override int GetHashCode()
        {
            return 2064421790 + compteur.GetHashCode();
        }
    }
}
