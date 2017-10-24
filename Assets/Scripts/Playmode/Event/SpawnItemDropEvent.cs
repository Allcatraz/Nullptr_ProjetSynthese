using Harmony;

namespace ProjetSynthese
{
    public class SpawnItemDropEvent : IEvent
    {
        public ObjectContainedInventory ItemToSpawn { get; private set; }

        public SpawnItemDropEvent(ObjectContainedInventory itemToSpawn)
        {
            ItemToSpawn = itemToSpawn;
        }
    }
}

