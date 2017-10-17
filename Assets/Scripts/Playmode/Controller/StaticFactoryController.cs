﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class StaticFactoryController : GameScript
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
            M16A4Factory.M16A4Prefab = M16A4Prefab;
            AWMFactory.AWMPrefab = AWMPrefab;
            SAIGA12Factory.SAIGA12Prefab = SAIGA12Prefab;
            M1911Factory.M1911Prefab = M1911Prefab;

            // AmmoPack
            AmmoFactory.AmmoPackPrefab = ammoPackPrefab;
        }
    }
}

