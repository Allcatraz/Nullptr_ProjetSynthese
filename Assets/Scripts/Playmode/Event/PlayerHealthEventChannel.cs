using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerHealthEventChannel")]
    public class PlayerHealthEventChannel : EventChannel<PlayerHealthEvent>
    {
    }
}