using UnityEngine;

namespace ProjetSynthese
{
    //BEN_REVIEW : C'est vraiment utile ça ? Je veux dire, il y en a qu'un seul dans votre projet
    //             et, selon moi, cela pourrait être directement mis dans "LineRendererCircle".
    //
    //             Séparer pour séparer ?
    
    //BEN_CORRECTION : Ceci n'est pas un Effect, mais il se trouve dans "Effect". Faites vous un dossier.
    
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
