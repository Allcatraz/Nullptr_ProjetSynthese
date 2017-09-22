using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/MoveFoward")]
    public class MoveFoward : GameScript
    {
        private TranslateMover translateMover;

        public void InjectMoveFoward([EntityScope] TranslateMover translateMover)
        {
            this.translateMover = translateMover;
        }

        private void Awake()
        {
            InjectDependencies("InjectMoveFoward");
        }

        private void Update()
        {
            translateMover.MoveFoward();
        }
    }
}