using Harmony;

namespace ProjetSynthese
{
    public class DeathEvent : IEvent
    {
        public R.E.Prefab DeadPrefab { get; private set; }

        public DeathEvent(R.E.Prefab deadPrefab)
        {
            DeadPrefab = deadPrefab;
        }
    }
}