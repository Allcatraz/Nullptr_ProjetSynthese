using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "LineRendererCircle", menuName = "Game/Effect/CircleInfo")]
    public class CircleInfo : ScriptableObject
    {
        [Header("Infos")]
        [LabelOverride("Angle")]
        [Tooltip("L'angle entre chaques lignes pour le cercle.")]
        [SerializeField]
        private float angle;

        [LabelOverride("Segments")]
        [Tooltip("Le nombre de segments pour former le cercle.")]
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
