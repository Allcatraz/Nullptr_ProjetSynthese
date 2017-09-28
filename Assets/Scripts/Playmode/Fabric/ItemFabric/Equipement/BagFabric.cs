using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class BagFabric : ItemFabricMaster
    {
        private static int percentChanceSpawnLevel1 = 70;
        private static int percentChanceSpawnLevel2 = 20;
        private static int percentChanceSpawnLevel3 = 10;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawnLevel1);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnLevel2);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnLevel3);

        private static GameObject bagPrefab;

        public static void CreateItem(List<Item> itemList, GameObject spawnPoint, System.Random rnd)
        {
            int item = rnd.Next(0, 101);

            GameObject _object = Object.Instantiate(bagPrefab);
            _object.transform.position = spawnPoint.transform.position;
            _object.transform.rotation = Random.rotation;

            Bag bag = _object.GetComponent<Bag>();


            if (item >= range1.x && item < range1.y)
            {
                bag.Level = 1;
            }
            else if (item >= range2.x && item < range2.y)
            {
                bag.Level = 2;
            }
            else if (item >= range3.x && item < range3.y)
            {
                bag.Level = 3;
            }
        }
    }

}
