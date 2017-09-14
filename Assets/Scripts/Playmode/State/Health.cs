using System.Text.RegularExpressions;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void HealthChangedEventHandler(int oldHealthPoints, int newHealthPoints);
    public delegate void DeathEventHandler();

    [AddComponentMenu("Game/World/Object/State/Health")]
    public class Health : GameScript
    {
        [SerializeField]
        private int initialHealthPoints;
        [SerializeField]
        private int maxHealthPoints;

        private int healthPoints;

        public virtual event HealthChangedEventHandler OnHealthChanged;
        public virtual event DeathEventHandler OnDeath;

        public void InjectHealth(int initialHealthPoints, int maxHealthPoints)
        {
            this.initialHealthPoints = initialHealthPoints;
            this.maxHealthPoints = maxHealthPoints;
        }

        public virtual int HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                int oldHealthPoints = healthPoints;
                healthPoints = value < 0 ? 0 : (value > maxHealthPoints ? maxHealthPoints : value);
                if (OnHealthChanged != null) OnHealthChanged(oldHealthPoints, healthPoints);
                if (healthPoints <= 0 && OnDeath != null)
                {
                    OnDeath();
                }
            }
        }

        public virtual int MaxHealthPoints
        {
            get { return maxHealthPoints; }
            set { maxHealthPoints = value; }
        }

        public void Awake()
        {
            InjectHealth(initialHealthPoints, maxHealthPoints);

            healthPoints = initialHealthPoints;
        }

        public virtual void Hit(int hitPoints)
        {
            HealthPoints -= hitPoints;
        }

        public virtual void Heal(int healPoints)
        {
            HealthPoints += healPoints;
        }

        public virtual void Reset()
        {
            HealthPoints = initialHealthPoints;
        }
    }
}