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

