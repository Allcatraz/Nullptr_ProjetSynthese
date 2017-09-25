using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/DeathEventChannel")]
    public class DeathEventChannel : EventChannel<DeathEvent>
    {
    }
}