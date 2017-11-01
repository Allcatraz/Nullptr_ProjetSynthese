using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public static class ItemFactory
    {
        private const int PercentChanceSpawnConsumable = 40;
        private const int PercentChanceSpawnEquipement = 30;
        private const int PercentChanceSpawnWeapon = 30;


        private static Vector2 Range1 = new Vector2(0, PercentChanceSpawnConsumable);
        private static Vector2 Range2 = new Vector2(Range1.y, Range1.y + PercentChanceSpawnEquipement);
        private static Vector2 Range3 = new Vector2(Range2.y, Range2.y + PercentChanceSpawnWeapon);

        public static void CreateItem(List<GameObject> itemList, Vector3 spawnPoint, System.Random random)
        {
            int item = random.Next(0, 101);

            if (item >= Range1.x && item < Range1.y)
            {
                ConsumableFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range2.x && item < Range2.y)
            {
                EquipementFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range3.x && item <= Range3.y)
            {
                WeaponFactory.CreateItem(itemList, spawnPoint, random);
            }
        }
    }
}
