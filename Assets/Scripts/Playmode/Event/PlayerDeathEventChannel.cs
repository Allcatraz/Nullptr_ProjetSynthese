using Harmony.EventSystem;
using Harmony.Testing;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    [AddComponentMenu("Game/Event/PlayerDeathEventChannel")]
    public class PlayerDeathEventChannel : UnityEventChannel<PlayerDeathEvent>
    {
    }
}
