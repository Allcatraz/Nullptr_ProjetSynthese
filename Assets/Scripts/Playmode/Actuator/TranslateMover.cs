using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/TranslateMover")]
    public class TranslateMover : GameScript
    {
        [SerializeField]
        private int speed;

        [SerializeField]
        private int rotationSpeed;

        private Transform topParentTranform;
        private Rigidbody2D topParentRigidbody2D;

        private void InjectTranslateMover([TopParentScope] Transform topParentTranform,
                                          [TopParentScope] Rigidbody2D topParentRigidbody2D)
        {
            this.topParentTranform = topParentTranform;
            this.topParentRigidbody2D = topParentRigidbody2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectTranslateMover");
        }

        public void MoveFoward()
        {
            topParentRigidbody2D.Translate(topParentTranform.up * speed * Time.deltaTime);
        }

        public void RotateClockwise()
        {
            topParentRigidbody2D.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}