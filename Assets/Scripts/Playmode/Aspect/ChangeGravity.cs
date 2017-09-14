using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Aspect/ChangeGravity")]
    public class ChangeGravity : GameScript
    {
        [SerializeField]
        private Vector2 gravity;

        private IPhysics2D physics2D;

        private Vector2 initialGravity;

        public void InjectSetGravityAtStart(Vector2 gravity,
                                            [ApplicationScope] IPhysics2D physics2D)
        {
            this.gravity = gravity;
            this.physics2D = physics2D;
        }

        public void Awake()
        {
            InjectDependencies("InjectSetGravityAtStart", gravity);
        }

        public void OnEnable()
        {
            initialGravity = physics2D.Gravity;
            physics2D.Gravity = Vector2.zero;
        }

        public void OnDisable()
        {
            physics2D.Gravity = initialGravity;
        }
    }
}