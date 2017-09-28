using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class WeaponFabric
    {
        private static int percentChanceSpawnM16A4 = 40;
        private static int percentChanceSpawnAWM = 10;
        private static int percentChanceSpawnSAIGA12 = 20;
        private static int percentChanceSpawnM1911 = 20;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawnM16A4);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnAWM);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnSAIGA12);
        private static Vector2 range4 = new Vector2(range3.y, range3.y + percentChanceSpawnM1911);

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            int item = rnd.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                M16A4Fabric.CreateItem(itemList, spawnPoint, rnd);
            }
            else if (item >= range2.x && item < range2.y)
            {
                AWMFabric.CreateItem(itemList, spawnPoint, rnd);
            }
            else if (item >= range3.x && item < range3.y)
            {
                SAIGA12Fabric.CreateItem(itemList, spawnPoint, rnd);
            }
            else if (item >= range4.x && item < range4.y)
            {
                M1911Fabric.CreateItem(itemList, spawnPoint, rnd);
            }
        }
    }
}


