using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Cell
    {
        private int compteur;

        public Cell()
        {
            compteur = 1;
        }

        public Cell(int compteur)
        {
            this.compteur = compteur;
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

        public static bool operator ==(Cell one, Cell two)
        {
            if ((object)one == null && (object)two == null)
            {
                return true;
            }
            if ((object)one == null || (object)two == null)
                return false;
            return one.GetItem() == two.GetItem();
        }

        public static bool operator != (Cell one, Cell two)
        {
            return !(one == two);
        }

        public override bool Equals(object obj)
        {
            var cell = obj as Cell;
            return cell != null &&
                   compteur == cell.compteur;
        }

        public override int GetHashCode()
        {
            return 2064421790 + compteur.GetHashCode();
        }
    }
}
