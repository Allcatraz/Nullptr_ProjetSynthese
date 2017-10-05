﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjetSynthese
{
    public class ItemSpawnerController : MonoBehaviour
    {
  
        private List<Item> items;

        public List<Item> Items { get; private set; }

        public const int MaxNumberOfItemsToSpawn = 4;
       
        /// <summary>
        /// Spawn les items
        /// </summary>
        void Start()
        {
            items = new List<Item>();
            System.Random rnd = new System.Random();
            int numberOfItemsToSpawn = rnd.Next(3, MaxNumberOfItemsToSpawn + 1);
            for (int i = 0; i < numberOfItemsToSpawn; i++)
            {
                ItemFactory.CreateItem(items, gameObject, rnd);
            }
        }


        /// <summary>
        /// Chercher un item dans la liste qui a le type passé en paramètre
        /// </summary>
        /// <param name="type">le type à chercher</param>
        /// <returns>Retourne l'index de l'item si un item a été trouvé, retourne le count de la liste si aucun item a été trouvé</returns>
        private int FindItemIndex(ItemType type)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Type == type)
                {
                    return i;
                }
            }
            return items.Count;
        }

        /// <summary>
        /// Retourne l'item dans la liste à l'index passé en paramètre.
        /// Supprime l'item de la liste et le retourne au caller.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Retourne l'item trouvé à l'index, retourne null si l'index n'est pas présent dans la liste</returns>
        public Item GetItem(int index)
        {
            if (index >= items.Count)
            {
                return null;
            }
            Item item = items[index];
            items.RemoveAt(index);
            return item;          
        }

        /// <summary>
        /// Cherche un objet dans la liste d'items du spawnPoint de type passé en paramètre.
        /// Supprime l'item de liste et le retourne au caller.
        /// </summary>
        /// <param name="itemType">Le type d'objet à chercher</param>
        /// <returns>Retourne le premier item du type trouvé dans la liste, retourne null si aucun item de ce type est présent dans la liste</returns>
        public Item GetItem(ItemType itemType)
        {
            int index = FindItemIndex(itemType);
            return GetItem(index);
        }


    }
}


