using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public delegate void HealthChangedEventHandler(float oldHealthPoints, float newHealthPoints);
    public delegate void DeathEventHandler();

    [AddComponentMenu("Game/State/Health")]
    public class Health : NetworkGameScript
    {
        [Tooltip("Le nombre de points de vie initial.")]
        [SerializeField] private int initialHealthPoints;
        [Tooltip("Le nombre de points de vie maximal.")]
        [SerializeField] private int maxHealthPoints;

        [SyncVar]
        private float healthPoints;

        public event HealthChangedEventHandler OnHealthChanged;
        public event DeathEventHandler OnDeath;

        public float HealthPoints
        {
            get { return healthPoints; }
            private set
            {
                float oldHealthPoints = healthPoints;
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

        public void Hit(float hitPoints, bool isAI)
        {
            //BEN_CORRECTION : Pourquoi "isAI" ne serait pas un "SerializedField" ?
            //
            //                 Vous avez des "Prefabs" pour vos "Ai" non ? Alors pourquoi cette information
            //                 ne serait pas contenue là au lieu d'être calculée, à chaque fois, avec ce bout
            //                 de code :
            
            /*
             *       bool isAI = false;
             *
             *       IInventory inventoryController = other.gameObject.GetComponentInChildren<PlayerController>();        
             *       if (inventoryController == null)
             *       {
             *           isAI = true;
             *       }
             */
            
            if (!isLocalPlayer && !isAI)
            {
                return;
            }

            HealthPoints -= hitPoints;
        }

        public void Heal(float healPoints)
        {
            HealthPoints += healPoints;
        }

        public void Reset()
        {
            HealthPoints = initialHealthPoints;
        }
    }
}