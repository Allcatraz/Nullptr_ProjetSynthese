using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    public class DeathEvent : IEvent
    {
        public R.E.Prefab DeadPrefab { get; private set; }

        public DeathEvent(R.E.Prefab deadPrefab)
        {
            DeadPrefab = deadPrefab;
        }
    }
}