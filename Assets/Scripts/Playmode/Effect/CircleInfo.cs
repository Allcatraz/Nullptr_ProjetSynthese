using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "LineRendererCircle", menuName = "Game/Effect/CircleInfo")]
    public class CircleInfo : ScriptableObject
    {
        [Header("Infos")]
        [LabelOverride("Angle")]
        [SerializeField]
        private float angle;

        [LabelOverride("Segments")]
        [SerializeField]
        private int segments;

        public float Angle
        {
            get { return angle; }
        }

        public int Segment
        {
            get { return segments; }
        }
    }
}
