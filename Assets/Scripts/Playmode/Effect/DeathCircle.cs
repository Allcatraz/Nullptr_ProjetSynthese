using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "DeathCircle", menuName = "Game/Effect/DeathCircle")]
    public class DeathCircle : ScriptableObject
    {
        [Header("Phase")]
        [LabelOverride("Wait(s)")]
        [SerializeField]
        private int[] waitTimeInSecond;

        [LabelOverride("Move(s)")]
        [SerializeField]
        private int[] moveTimeInSecond;

        [LabelOverride("DPS")]
        [SerializeField]
        private float[] domagePerSecond;

        [LabelOverride("Skrink")]
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
