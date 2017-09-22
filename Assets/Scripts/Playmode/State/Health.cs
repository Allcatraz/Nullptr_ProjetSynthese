using UnityEngine;

namespace ProjetSynthese
{
    public delegate void HealthChangedEventHandler(int oldHealthPoints, int newHealthPoints);
    public delegate void DeathEventHandler();

    [AddComponentMenu("Game/State/Health")]
    public class Health : GameScript
    {
        [SerializeField]
        private int initialHealthPoints;
        [SerializeField]
        private int maxHealthPoints;

        private int healthPoints;

        public event HealthChangedEventHandler OnHealthChanged;
        public event DeathEventHandler OnDeath;

        public int HealthPoints
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

        public int MaxHealthPoints
        {
            get { return maxHealthPoints; }
            set { maxHealthPoints = value; }
        }

        public void Awake()
        {
            healthPoints = initialHealthPoints;
        }

        public void Hit(int hitPoints)
        {
            HealthPoints -= hitPoints;
        }

        public void Heal(int healPoints)
        {
            HealthPoints += healPoints;
        }

        public void Reset()
        {
            HealthPoints = initialHealthPoints;
        }
    }
}