using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Heal : Item
    {
        
        private static int level1Efficacity = 5;
        private static int level2Efficacity = 75;
        private static int level3Efficacity = 100;

        private static int[] efficacities = { level1Efficacity, level2Efficacity, level3Efficacity };

        private static int level1Weight = 1;
        private static int level2Weight = 10;
        private static int level3Weight = 15;

        private static int[] weights = { level1Weight, level2Weight, level3Weight };

        public int Efficacity
        {
            get
            {
                return efficacities[Level - 1];
            }
            private set { }
        }

        public override void Use()
        {
            Player.GetComponentInChildren<Health>().Heal(Efficacity);
            // TODO : Enlever un health dans l'inventaire
        }

        public override int GetWeight()
        {
            return weights[Level - 1];
        }
    }
}

