using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public static class ItemFactory
    {
        private static int percentChanceSpawnConsumable = 40;
        private static int percentChanceSpawnEquipement = 30;
        private static int percentChanceSpawnWeapon = 30;


        private static Vector2 range1 = new Vector2(0, percentChanceSpawnConsumable);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawnEquipement);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawnWeapon);

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {           
            int item = random.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                ConsumableFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= range2.x && item < range2.y)
            {
                EquipementFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= range3.x && item <= range3.y)
            {
                WeaponFactory.CreateItem(itemList, spawnPoint, random);
            }
        }

    }
}

