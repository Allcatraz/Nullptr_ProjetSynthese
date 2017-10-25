using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AmmoFactory : ItemFactoryMaster
    {
        private const int PercentChanceSpawn30Ammo = 60;
        private const int PercentChanceSpawn60Ammo = 30;
        private const int PercentChanceSpawn90Ammo = 10;

        private static Vector2 Range1 = new Vector2(0, PercentChanceSpawn30Ammo);
        private static Vector2 Range2 = new Vector2(Range1.y, Range1.y + PercentChanceSpawn60Ammo);
        private static Vector2 Range3 = new Vector2(Range2.y, Range2.y + PercentChanceSpawn90Ammo);

        public static GameObject AmmoPackPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random, AmmoType ammoType)
        {
            GameObject gameObject = SpawnObject(spawnPoint, AmmoPackPrefab);

            AmmoPack ammoPack = gameObject.GetComponent<AmmoPack>();
            ammoPack.AmmoType = ammoType;
            itemList.Add(ammoPack);

            int item = random.Next(0, 101);

            if (item >= Range1.x && item < Range1.y)
            {
                ammoPack.NumberOfAmmo = 30;
            }
            else if (item >= Range2.x && item < Range2.y)
            {
                ammoPack.NumberOfAmmo = 60;
            }
            else if (item >= Range3.x && item < Range3.y)
            {
                ammoPack.NumberOfAmmo = 90;
            }
            CmdSpawnObject(gameObject);
        }
    }
}


