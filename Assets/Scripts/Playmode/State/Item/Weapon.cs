﻿using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnMunitionChanged();

    public class Weapon : Usable
    {
        [Tooltip("L'endroit ou doivent spawner les balles.")]
        [SerializeField]
        private GameObject bulletSpawnPoint;

        [Tooltip("L'endroit ou la balle est charger pour calculer la direction que celle-ci va prendre.")]
        [SerializeField]
        private GameObject chamber;

        [Tooltip("La vitesse que la balle va avoir lorsqu'elle sera tiré.")]
        [SerializeField]
        private float bulletSpeed;

        [Tooltip("Le temps de vie que la balle a.")]
        [SerializeField]
        private float bulletLivingTime;

        [Tooltip("Le nombre de balles maximale pour un chargeur.")]
        [SerializeField]
        private int magazineMaxAmount;

        [Tooltip("Dommage que l'arme fait a l'autre joueur.")]
        [SerializeField]
        private int dommage;

        public event OnMunitionChanged OnMunitionChanged;

        private const int Weight = 0;

        public int MagazineMax
        {
            get { return magazineMaxAmount; }
        }

        public int MagazineAmount { get; set; }

        public float EffectiveWeaponRange
        {
            get { return bulletLivingTime * bulletSpeed; }
        }

        private void Awake()
        {
            MagazineAmount = MagazineMax;
        }

        public override void Use()
        {
            if (MagazineAmount > 0)
            {
                CmdSpawnObject(BulletFactory.CreateBullet(bulletSpawnPoint, chamber, bulletSpeed, bulletLivingTime, dommage));
                MagazineAmount -= 1;
                NotidyMunitionChanged(); 
            }
        }

        public bool Reload()
        {
            MagazineAmount = MagazineMax;
            NotidyMunitionChanged();
            if (MagazineAmount > 0)
            {
                return true;
            }
            return false;
        }

        private void NotidyMunitionChanged()
        {
            if (OnMunitionChanged != null) OnMunitionChanged();
        }

        public override int GetWeight()
        {
            return Weight;
        }
    }
}


