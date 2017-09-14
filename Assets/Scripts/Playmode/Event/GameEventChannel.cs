using UnityEngine;
using Harmony.EventSystem;
using Harmony.Testing;
using Harmony.Unity;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    [AddComponentMenu("Game/Event/GameEventChannel")]
    public class GameEventChannel : UnityEventChannel<GameEvent>
    {
    }
}