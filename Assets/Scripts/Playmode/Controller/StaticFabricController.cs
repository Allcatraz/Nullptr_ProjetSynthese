using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class StaticFabricController : GameScript
    {
        [SerializeField]
        GameObject bulletPrefab;

        // Consumable
        [SerializeField]
        GameObject healPrefab;
        [SerializeField]
        GameObject boostPrefab;

        // Equipement
        [SerializeField]
        GameObject vestPrefab;
        [SerializeField]
        GameObject helmentPrefab;
        [SerializeField]
        GameObject bagPrefab;

        // Weapon
        [SerializeField]
        GameObject M16A4Prefab;
        [SerializeField]
        GameObject AWMPrefab;
        [SerializeField]
        GameObject SAIGA12Prefab;
        [SerializeField]
        GameObject M1911Prefab;

        // AmmoPack
        [SerializeField]
        GameObject ammoPackPrefab;

        private void Awake()
        {
            BulletFabric.BulletPrefab = bulletPrefab;
            InitializeItemFabrics();
        }

        private void InitializeItemFabrics()
        {
            // Consumable
            HealFabric.HealPrefab = healPrefab;
            BoostFabric.BoostPrefab = boostPrefab;

            // Equipement
            VestFabric.VestPrefab = vestPrefab;
            HelmentFabric.HelmetPrefab = helmentPrefab;
            BagFabric.BagPrefab = bagPrefab;

            //Weapon
            M16A4Fabric.M16A4Prefab = M16A4Prefab;
            AWMFabric.AWMPrefab = AWMPrefab;
            SAIGA12Fabric.SAIGA12Prefab = SAIGA12Prefab;
            M1911Fabric.M1911Prefab = M1911Prefab;

            // AmmoPack
            AmmoFabric.AmmoPackPrefab = ammoPackPrefab;
        }
    }
}


