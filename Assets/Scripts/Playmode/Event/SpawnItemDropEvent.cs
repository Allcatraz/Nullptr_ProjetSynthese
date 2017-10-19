using Harmony;

namespace ProjetSynthese
{
    public class SpawnItemDropEvent : IEvent
    {
        public Cell ItemToSpawn { get; private set; }

        public SpawnItemDropEvent(Cell itemToSpawn)
        {
            ItemToSpawn = itemToSpawn;
        }
    }
}

