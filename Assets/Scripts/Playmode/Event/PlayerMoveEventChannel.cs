using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Event/PlayerMoveEventChannel")]
    public class PlayerMoveEventChannel : EventChannel<PlayerMoveEvent>
    {
    }
}
