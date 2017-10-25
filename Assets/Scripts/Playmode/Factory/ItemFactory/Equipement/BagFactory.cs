﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class BagFactory : ItemFactoryMaster
    {
        private static int percentChanceSpawnLevel1 = 40;
        private static int percentChanceSpawnLevel2 = 40;
        private static int percentChanceSpawnLevel3 = 20;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawnLevel1);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnLevel2);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnLevel3);

        public static GameObject BagPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {
            GameObject gameObject = SpawnObject(spawnPoint, BagPrefab);

            Bag bag = gameObject.GetComponent<Bag>();

            int item = random.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                bag.Level = 1;
            }
            else if (item >= range2.x && item < range2.y)
            {
                bag.Level = 2;
            }
            else if (item >= range3.x && item <= range3.y)
            {
                bag.Level = 3;
            }

            CmdSpawnObject(gameObject);
        }
    }

}
