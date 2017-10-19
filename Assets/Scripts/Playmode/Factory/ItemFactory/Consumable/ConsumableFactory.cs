using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class ConsumableFactory
    {
        private static int PercentChanceHeal = 50;
        private static int PercentChanceBoost = 50;

        private static Vector2 range1 = new Vector2(0, PercentChanceHeal);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + PercentChanceBoost);


        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random rnd)
        {
            int item = rnd.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                HealFactory.CreateItem(itemList, spawnPoint, rnd);
            }
            else if (item >= range2.x && item <= range2.y)
            {
                BoostFactory.CreateItem(itemList, spawnPoint, rnd);
            }
        }
    }
}

