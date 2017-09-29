using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class EquipementFabric
    {
        private static int percentChanceSpawnVest = 30;
        private static int percentChanceSpawnHelmet = 30;
        private static int percentChanceSpawnBag = 40;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawnVest);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnHelmet);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnBag);

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            int item = rnd.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                VestFabric.CreateItem(itemList, spawnPoint, rnd);
            }
            else if (item >= range2.x && item < range2.y)
            {
                HelmentFabric.CreateItem(itemList, spawnPoint, rnd);
            }
            else if (item >= range3.x && item < range3.y)
            {
                BagFabric.CreateItem(itemList, spawnPoint, rnd);
            }
        }
    }
}

