using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Boost : Item
    {
        public int Efficacity
        {
            get
            {
                return efficacities[Level - 1];
            }
            private set { }
        }

        public static int level1Efficacity = 5;
        public static int level2Efficacity = 15;
        public static int level3Efficacity = 35;

        private static int[] efficacities = { level1Efficacity, level2Efficacity, level3Efficacity };

        private static int level1Weight = 1;
        private static int level2Weight = 10;
        private static int level3Weight = 15;

        private static int[] weights = { level1Weight, level2Weight, level3Weight };

        public override void Use()
        {
            //throw new System.NotImplementedException();
        }

        public override int GetWeight()
        {
            return weights[Level - 1];
        }
    }
}
