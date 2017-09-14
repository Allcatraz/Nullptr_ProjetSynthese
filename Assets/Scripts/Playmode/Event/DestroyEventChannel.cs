using Harmony.EventSystem;
using Harmony.Testing;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    [NotTested(Reason.EventChannel)]
    [AddComponentMenu("Game/Event/DestroyEventChannel")]
    public class DestroyEventChannel : UnityEventChannel<DestroyEvent>
    {
    }
}