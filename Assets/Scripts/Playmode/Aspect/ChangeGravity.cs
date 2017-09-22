using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/ChangeGravity")]
    public class ChangeGravity : GameScript
    {
        [SerializeField]
        private Vector2 gravity;

        private Vector2 initialGravity;

        private void OnEnable()
        {
            initialGravity = Physics2D.gravity;
            Physics2D.gravity = gravity;
        }

        private void OnDisable()
        {
            Physics2D.gravity = initialGravity;
        }
    }
}