using UnityEngine;
using Harmony.Injection;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Control/MoveFoward")]
    public class MoveFoward : GameScript
    {
        private TranslateMover translateMover;

        public void InjectBulletController([EntityScope] TranslateMover translateMover)
        {
            this.translateMover = translateMover;
        }

        public void Awake()
        {
            InjectDependencies("InjectBulletController");
        }

        public void Update()
        {
            translateMover.MoveFoward();
        }
    }
}