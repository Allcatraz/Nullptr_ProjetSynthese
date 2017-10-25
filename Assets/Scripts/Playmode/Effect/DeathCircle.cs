using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "DeathCircle", menuName = "Game/Effect/DeathCircle")]
    public class DeathCircle : ScriptableObject
    {
        [Header("Phase")]
        [LabelOverride("Wait(s)")]
        [Tooltip("Le temps d'attente avant l'affichage du chrono.")]
        [SerializeField]
        private int[] waitTimeInSecond;

        [LabelOverride("Move(s)")]
        [Tooltip("Le temps d'attente avant que le cercle bouge.")]
        [SerializeField]
        private int[] moveTimeInSecond;

        [LabelOverride("DPS")]
        [Tooltip("Le dommage que prend les joueurs par secondes")]
        [SerializeField]
        private float[] domagePerSecond;

        [LabelOverride("Skrink")]
        [Tooltip("Combien de fois plus petit sera le prochain cercle.")]
        [Range(0, 1)]
        [SerializeField]
        private float[] shrink;

        public int[] WaitTimeInSecond
        {
            get { return waitTimeInSecond; }
        }

        public int[] MoveTimeInSecond
        {
            get { return moveTimeInSecond; }
        }

        public float[] DomagePerSecond
        {
            get { return domagePerSecond; }
        }

        public float[] Shrink
        {
            get { return shrink; }
        }
    }
}
