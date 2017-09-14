using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Actuator/TranslateMover")]
    public class TranslateMover : GameScript
    {
        [SerializeField]
        private int speed;

        private new ITransform transform;
        private new IRigidbody2D rigidbody2D;
        private ITime time;

        public void InjectTranslateMover(int speed,
                                         [TopParentScope] ITransform transform,
                                         [TopParentScope] IRigidbody2D rigidbody2D,
                                         [ApplicationScope] ITime time)
        {
            this.speed = speed;
            this.transform = transform;
            this.rigidbody2D = rigidbody2D;
            this.time = time;
        }

        public void Awake()
        {
            InjectDependencies("InjectTranslateMover", speed);
        }

        public virtual void MoveFoward()
        {
            rigidbody2D.Translate(transform.Up * speed * time.DeltaTime);
        }
    }
}