using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerBoostEventChannel")]
    public class PlayerBoostEventChannel : EventChannel<PlayerBoostEvent>
    {
    }
}

