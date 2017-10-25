using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class HelmentFactory : ItemFactoryMaster
    {
        private static int percentChanceSpawnLevel1 = 70;
        private static int percentChanceSpawnLevel2 = 20;
        private static int percentChanceSpawnLevel3 = 10;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawnLevel1);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnLevel2);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnLevel3);

        public static GameObject HelmetPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {
            GameObject gameObject = SpawnObject(spawnPoint, HelmetPrefab);

            Helmet helmet = gameObject.GetComponent<Helmet>();

            int item = random.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                helmet.Level = 1;
            }
            else if (item >= range2.x && item < range2.y)
            {
                helmet.Level = 2;
            }
            else if (item >= range3.x && item <= range3.y)
            {
                helmet.Level = 3;
            }

            CmdSpawnObject(gameObject);
        }
    }

}
