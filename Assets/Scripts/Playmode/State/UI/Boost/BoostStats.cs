using UnityEngine;

namespace ProjetSynthese
{
    public delegate void BoostChangedEventHandler(float oldBoostPoints, float newBoostPoints);

    public class BoostStats : GameScript
    {
        [SerializeField] private float initialBoostPoints;
        [SerializeField] private float maxBoostPoints;

        public event BoostChangedEventHandler OnBoostChanged;

        private float boostPoints;

        public float BoostPoints
        {
            get { return boostPoints; }
            private set
            {
                float oldBoostPoints = boostPoints;
                boostPoints = value < 0 ? 0 : (value > maxBoostPoints ? maxBoostPoints : value);

                if (OnBoostChanged != null) OnBoostChanged(oldBoostPoints, boostPoints);
            }
        }

        public float MaxBoostPoints
        {
            get { return maxBoostPoints; }
            set { maxBoostPoints = value; }
        }

        public void Awake()
        {
            boostPoints = initialBoostPoints;
        }

        public void Hit(float hitPoints)
        {
            BoostPoints -= hitPoints;
        }

        public void Heal(float healPoints)
        {
            BoostPoints += healPoints;
        }

        public void Reset()
        {
            BoostPoints = initialBoostPoints;
        }

    }
}


