using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/GameEventChannel")]
    public class GameEventChannel : EventChannel<GameEvent>
    {
    }
}