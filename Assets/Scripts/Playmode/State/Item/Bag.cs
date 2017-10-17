using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Bag : Item
    {
        public int Level { get; set; }

        public int Capacity
        {
            get
            {
                return capacities[Level - 1];
            }
            private set {}
        }
        
        private static int level1Capacity = 100;
        private static int level2Capacity = 175;
        private static int level3Capacity = 300;

        private static int[] capacities = { level1Capacity , level2Capacity , level3Capacity };

        private static int weight = 0;

        public override void Use()
        {
            throw new System.NotImplementedException();
        }

        public override int GetWeight()
        {
            return weight;
        }
    }
}

