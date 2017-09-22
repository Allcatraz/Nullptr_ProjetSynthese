using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerDeathEventChannel")]
    public class PlayerDeathEventChannel : EventChannel<PlayerDeathEvent>
    {
    }
}
