using Harmony.EventSystem;
using Harmony.Testing;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    [AddComponentMenu("Game/Event/PlayerHealthEventChannel")]
    public class PlayerHealthEventChannel : UnityUpdatableEventChannel<PlayerHealthEvent, PlayerHealthUpdate>
    {
    }
}