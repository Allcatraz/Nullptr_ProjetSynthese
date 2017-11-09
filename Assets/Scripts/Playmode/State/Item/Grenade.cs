using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Grenade : Item
    {
        private const int Weight = 0;

        public override int GetWeight()
        {
            return Weight;
        }

    }
}