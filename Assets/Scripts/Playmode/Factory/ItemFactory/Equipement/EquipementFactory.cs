using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class EquipementFactory
    {
        private const int PercentChanceSpawnVest = 30;
        private const int PercentChanceSpawnHelmet = 30;
        private const int PercentChanceSpawnBag = 40;

        private static Vector2 Range1 = new Vector2(0, PercentChanceSpawnVest);
        private static Vector2 Range2 = new Vector2(Range1.y, Range1.y + PercentChanceSpawnHelmet);
        private static Vector2 Range3 = new Vector2(Range2.y, Range2.y + PercentChanceSpawnBag);

        public static void CreateItem(List<GameObject> itemList, Vector3 spawnPoint, System.Random random)
        {
            int item = random.Next(0, 101);

            if (item >= Range1.x && item < Range1.y)
            {
                VestFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range2.x && item < Range2.y)
            {
                HelmentFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range3.x && item <= Range3.y)
            {
                BagFactory.CreateItem(itemList, spawnPoint, random);
            }
        }
    }
}

