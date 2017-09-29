using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AmmoPack : Item
    {
        public AmmoType AmmoType { get; set; }
        public int NumberOfAmmo { get; set; }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}

