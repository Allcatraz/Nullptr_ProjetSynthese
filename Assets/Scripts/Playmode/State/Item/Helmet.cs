using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class Helmet : Item
    {
        public int ProtectionValue
        {
            get
            {
                return protectionValues[Level - 1];
            }
            private set
            {

            }
        }


        private static int protectionValueLevel1 = 15;
        private static int protectionValueLevel2 = 30;
        private static int protectionValueLevel3 = 50;

        private static int[] protectionValues = { protectionValueLevel1, protectionValueLevel2, protectionValueLevel3 };

        private static int weight = 0;

        public override int GetWeight()
        {
            return weight;
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}
