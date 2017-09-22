using UnityEngine;
using Harmony;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/Spin")]
    public class Spin : GameScript
    {
        private TranslateMover translateMover;

        private void InjectSpin([EntityScope] TranslateMover translateMover)
        {
            this.translateMover = translateMover;
        }

        private void Awake()
        {
            InjectDependencies("InjectSpin");
        }

        private void Update()
        {
            translateMover.RotateClockwise();
        }
    }
}