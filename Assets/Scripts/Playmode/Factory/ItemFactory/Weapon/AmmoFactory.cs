using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class AmmoFactory : ItemFactoryMaster
    {
        private static int percentChanceSpawn30Ammo = 60;
        private static int percentChanceSpawn60Ammo = 30;
        private static int percentChanceSpawn90Ammo = 10;

        private static Vector2 range1 = new Vector2(0, percentChanceSpawn30Ammo);
        private static Vector2 range2 = new Vector2(range1.y, range1.y + percentChanceSpawn60Ammo);
        private static Vector2 range3 = new Vector2(range2.y, range2.y + percentChanceSpawn90Ammo);

        public static GameObject AmmoPackPrefab { get; set; }

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random rnd, AmmoType ammoType)
        {
            GameObject gameObject = CmdSpawnObject(spawnPoint, AmmoPackPrefab);

            AmmoPack ammoPack = gameObject.GetComponent<AmmoPack>();
            ammoPack.AmmoType = ammoType;
            itemList.Add(ammoPack);

            int item = rnd.Next(0, 101);

            if (item >= range1.x && item < range1.y)
            {
                ammoPack.NumberOfAmmo = 30;
            }
            else if (item >= range2.x && item < range2.y)
            {
                ammoPack.NumberOfAmmo = 60;
            }
            else if (item >= range3.x && item < range3.y)
            {
                ammoPack.NumberOfAmmo = 90;
            }
            CmdSpawnObject(_object);
        }
    }
}


