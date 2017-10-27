using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class WeaponFactory
    {
        private const int PercentChanceSpawnM16A4 = 40;
        private const int PercentChanceSpawnAWM = 10;
        private const int PercentChanceSpawnSAIGA12 = 20;
        private const int PercentChanceSpawnM1911 = 20;

        private static Vector2 Range1 = new Vector2(0, PercentChanceSpawnM16A4);
        private static Vector2 Range2 = new Vector2(Range1.y, Range1.y + PercentChanceSpawnAWM);
        private static Vector2 Range3 = new Vector2(Range2.y, Range2.y + PercentChanceSpawnSAIGA12);
        private static Vector2 Range4 = new Vector2(Range3.y, Range3.y + PercentChanceSpawnM1911);

        public static void CreateItem(List<Item> itemList, Vector3 spawnPoint, System.Random random)
        {
            int item = random.Next(0, 101);

            if (item >= Range1.x && item < Range1.y)
            {
                M16A4Factory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range2.x && item < Range2.y)
            {
                AWMFactory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range3.x && item < Range3.y)
            {
                SAIGA12Factory.CreateItem(itemList, spawnPoint, random);
            }
            else if (item >= Range4.x && item <= Range4.y)
            {
                M1911Factory.CreateItem(itemList, spawnPoint, random);
            }
        }
    }
}


