using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "Torus", menuName = "Game/Effect/TorusInfo")]
    public class TorusInfo : ScriptableObject
    {
        [Header("Torus Infomations")]
        [LabelOverride("Raidus 1")]
        [SerializeField]
        private float radius1;

        [LabelOverride("Radius 2")]
        [SerializeField]
        private float radius2;

        [LabelOverride("Number of radian per segment")]
        [SerializeField]
        private int numberRadSeg;

        [LabelOverride("Number of side")]
        [SerializeField]
        private int numberSide;

        public float Radius1
        {
            get { return radius1; }
        }

        public float Radius2
        {
            get { return radius2; }
        }

        public int NumberRadSeg
        {
            get { return numberRadSeg; }
        }

        public int NumberSide
        {
            get { return numberSide; }
        }
    }
}

