using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjetSynthese
{
    public static class GlobalRandom
    {
        private static System.Random random = new System.Random();
        public static System.Random Random
        {
            get
            {
                return random;
            }
        }
        public static int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

    }
}


