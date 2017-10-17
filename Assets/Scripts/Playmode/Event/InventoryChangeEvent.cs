using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    public class InventoryChangeEvent : IEvent
    {
        public Inventory Inventory { get; private set; }

        public InventoryChangeEvent(Inventory inventory)
        {
            Inventory = inventory;
        }
    }
}

