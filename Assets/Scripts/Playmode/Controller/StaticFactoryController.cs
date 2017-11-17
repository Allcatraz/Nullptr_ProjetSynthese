using UnityEngine;

namespace ProjetSynthese
{
    public class StaticFactoryController : GameScript
    {
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les bullets")]
        GameObject bulletPrefab;

        // Consumable
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les heals")]
        GameObject healPrefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les boosts")]
        GameObject boostPrefab;

        // Equipement
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les vests")]
        GameObject vestPrefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les helmets")]
        GameObject helmentPrefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les bags")]
        GameObject bagPrefab;

        // Weapon
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les M16A4")]
        GameObject m16A4Prefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les AWM")]
        GameObject awmPrefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les Saiga12")]
        GameObject saiga12Prefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les M1911")]
        GameObject m1911Prefab;
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les grenades")]
        GameObject grenadePrefab;

        // AmmoPack
        [SerializeField]
        [Tooltip("Le prefab utilisé pour instancié les ammo packs")]
        GameObject ammoPackPrefab;

        private void Awake()
        {
            BulletFactory.BulletPrefab = bulletPrefab;
            InitializeItemFabrics();
        }

        private void InitializeItemFabrics()
        {
            // Consumable
            HealFactory.HealPrefab = healPrefab;
            BoostFactory.BoostPrefab = boostPrefab;

            // Equipement
            VestFactory.VestPrefab = vestPrefab;
            HelmentFactory.HelmetPrefab = helmentPrefab;
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


