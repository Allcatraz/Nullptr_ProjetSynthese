using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class ConsumableFactory
    {
        private const int PercentChanceHeal = 50;
        private const int PercentChanceBoost = 50;

        private static Vector2 Pange1 = new Vector2(0, PercentChanceHeal);
        private static Vector2 Range2 = new Vector2(Pange1.y, Pange1.y + PercentChanceBoost);


        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {
            int item = random.Next(0, 101);

            if (item >= Pange1.x && item < Pange1.y)
            {
                HealFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range2.x && item <= Range2.y)
            {
                BoostFactory.CreateItem(itemList, spawnPoint, random);
            }
        }
    }
}

