using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void BoostChangedEventHandler(int oldBoostPoints, int newBoostPoints);

    public class BoostStats : GameScript
    {
        [SerializeField] private int initialBoostPoints;
        [SerializeField] private int maxBoostPoints;

        public event BoostChangedEventHandler OnBoostChanged;

        private int boostPoints;

        public int BoostPoints
        {
            get { return boostPoints; }
            private set
            {
                int oldBoostPoints = boostPoints;
                boostPoints = value < 0 ? 0 : (value > maxBoostPoints ? maxBoostPoints : value);

                if (OnBoostChanged != null) OnBoostChanged(oldBoostPoints, boostPoints);
            }
        }

        public int MaxBoostPoints
        {
            get { return maxBoostPoints; }
            set { maxBoostPoints = value; }
        }

        public void Awake()
        {
            boostPoints = initialBoostPoints;
        }

        public void Hit(int hitPoints)
        {
            BoostPoints -= hitPoints;
        }

        public void Heal(int healPoints)
        {
            BoostPoints += healPoints;
        }

        public void Reset()
        {
            BoostPoints = initialBoostPoints;
        }

    }
}


