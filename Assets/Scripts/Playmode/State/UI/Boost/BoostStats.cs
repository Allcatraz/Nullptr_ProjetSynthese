using System.Collections;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void BoostChangedEventHandler(float oldBoostPoints, float newBoostPoints);

    public delegate void BoostHealEventHandler(float health);

    public class BoostStats : GameScript
    {
        [Tooltip("Le nombre de points initial pour le boost.")]
        [SerializeField] private float initialBoostPoints;
        [Tooltip("Le nombre de points maximum pour le boost.")]
        [SerializeField] private float maxBoostPoints;

        private const float healthPointPerBoost = 0.5f;

        public event BoostChangedEventHandler OnBoostChanged;
        public event BoostHealEventHandler OnBoostHeal;

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

        private void Awake()
        {
            boostPoints = initialBoostPoints;
            StartCoroutine("ComputeBoost");
        }

        private void OnDestroy()
        {
            StopCoroutine("ComputeBoost");
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

        private IEnumerator ComputeBoost()
        {
            for (;;)
            {
                if (boostPoints > 0)
                {
                   if (OnBoostHeal != null) OnBoostHeal(healthPointPerBoost); 
                }
                BoostPoints--;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}