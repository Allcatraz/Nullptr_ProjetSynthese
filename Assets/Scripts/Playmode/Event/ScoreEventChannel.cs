using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/ScoreEventChannel")]
    public class ScoreEventChannel : EventChannel<ScoreEvent>
    {
    }
}