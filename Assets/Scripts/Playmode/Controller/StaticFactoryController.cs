using UnityEngine;

namespace ProjetSynthese
{
    public class StaticFactoryController : GameScript
    {
        // Consumable
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les heals")]
        private GameObject[] healPrefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les boosts")]
        private GameObject[] boostPrefab;

        // Equipement
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les vests")]
        private GameObject[] vestPrefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les helmets")]
        private GameObject[] helmetPrefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les bags")]
        private GameObject[] bagPrefab;

        // Weapon
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les M16A4")]
        private GameObject m16A4Prefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les M110")]
        private GameObject awmPrefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les BenelliM4")]
        private GameObject saiga12Prefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les M1911")]
        private GameObject m1911Prefab;

        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les grenades")]
        private GameObject grenadePrefab;

        // AmmoPack
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les ammo packs")]
        private GameObject[] ammoPackPrefab;

        private void Awake()
        {
            InitializeItemFabrics();
        }

        private void InitializeItemFabrics()
        {
            // Consumable
            HealFactory.HealPrefab = healPrefab;
            BoostFactory.BoostPrefab = boostPrefab;

            // Equipement
            VestFactory.VestPrefab = vestPrefab;
            HelmentFactory.HelmetPrefab = helmetPrefab;
            BagFactory.BagPrefab = bagPrefab;

            //Weapon
            M16A4Factory.M16A4Prefab = m16A4Prefab;
            AWMFactory.AWMPrefab = awmPrefab;
            SAIGA12Factory.Saiga12Prefab = saiga12Prefab;
            M1911Factory.M1911Prefab = m1911Prefab;
            GrenadeFactory.GrenadePrefab = grenadePrefab;

            // AmmoPack
            AmmoFactory.AmmoPackPrefab = ammoPackPrefab;
        }
    }
}