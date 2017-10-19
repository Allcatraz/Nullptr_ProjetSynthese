using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class VestFactory : ItemFactoryMaster
    {
        private static int percentChanceSpawnLevel1 = 70;
        private static int percentChanceSpawnLevel2 = 20;
        private static int percentChanceSpawnLevel3 = 10;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawnLevel1);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnLevel2);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnLevel3);

        public static GameObject VestPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random rnd)
        {
            GameObject _object = CmdSpawnObject(spawnPoint, VestPrefab);

            Vest vest = _object.GetComponent<Vest>();

            int item = rnd.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                vest.Level = 1;
            }
            else if (item >= range2.x && item < range2.y)
            {
                vest.Level = 2;
            }
            else if (item >= range3.x && item <= range3.y)
            {
                vest.Level = 3;
            }
        }
    }

}
