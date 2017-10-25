using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class BagFactory : ItemFactoryMaster
    {
        private const int PercentChanceSpawnLevel1 = 40;
        private const int PercentChanceSpawnLevel2 = 40;
        private const int PercentChanceSpawnLevel3 = 20;

        private static Vector2 Range1 = new Vector2(0, PercentChanceSpawnLevel1);
        private static Vector2 Range2 = new Vector2(Range1.y, Range1.y + PercentChanceSpawnLevel2);
        private static Vector2 Range3 = new Vector2(Range2.y, Range2.y + PercentChanceSpawnLevel3);

        public static GameObject BagPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {
            GameObject gameObject = SpawnObject(spawnPoint, BagPrefab);

            Bag bag = gameObject.GetComponent<Bag>();

            int item = random.Next(0, 101);

            if (item >= Range1.x && item < Range1.y)
            {
                bag.Level = 1;
            }
            else if (item >= Range2.x && item < Range2.y)
            {
                bag.Level = 2;
            }
            else if (item >= Range3.x && item <= Range3.y)
            {
                bag.Level = 3;
            }

            CmdSpawnObject(gameObject);
        }
    }

}
