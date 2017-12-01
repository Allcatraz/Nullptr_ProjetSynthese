using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnMunitionChanged();

    public class Weapon : Usable
    { 
        [Tooltip("Type de munition approprié pour l'arme.")]
        [SerializeField]
        private AmmoType ammoType;

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

        [Tooltip("Temps avant que le joueur puisse tirer une autre fois.")]
        [SerializeField]
        private float shootTime;

        [Tooltip("Temps que le rechargement de l'arme va prendre.")]
        [SerializeField]
        private float reloadTime;

        [Tooltip("Temps que le rechargement de l'arme va prendre.")]
        [SerializeField]
        private PlaySound[] sounds;

        public event OnMunitionChanged OnMunitionChanged;

        private const int Weight = 0;
        private float timerForShoot = 0;
        private bool isReloading = false;

        public float BulletSpeed { get { return bulletSpeed; } }
        public float LivingTime { get { return bulletLivingTime; } }
        public GameObject Chamber { get { return chamber; } }
        public GameObject SpawnPoint { get { return bulletSpawnPoint; } }

        public int Dommage
        {
            get { return dommage; }
        }

        public int MagazineMax
        {
            get { return magazineMaxAmount; }
        }

        public int MagazineAmount { get; set; }

        public AmmoType WeaponAmmoType{ get { return ammoType; } private set { ammoType = value; } }

        public float EffectiveWeaponRange
        {
            get { return bulletLivingTime * bulletSpeed; }
        }

        private void Awake()
        {
            MagazineAmount = MagazineMax;
        }

        private void Update()
        {
            timerForShoot += Time.deltaTime;
        }

        public void ChangeWeaponSound()
        {
            sounds[3].Use(0);
            ComputeBoltUpSound();
        }

        public override bool Use()
        {
            if (MagazineAmount > 0 && timerForShoot >= shootTime && !isReloading)
            {
                sounds[0].Use(0);
                if (type == ItemType.M110)
                {
                    sounds[5].Use(0.3f);
                    sounds[6].Use(reloadTime - 0.3f);
                }
                MagazineAmount -= 1;
                NotidyMunitionChanged();
                timerForShoot = 0;
                return true;
            }
            return false;
        }

        public bool Reload(Inventory inventory)
        {
            if (inventory.UseAmmoPack(ammoType)  && !isReloading && MagazineAmount != MagazineMax)
            {
                ComputeSoundPreReload();
                StartCoroutine("ComputeReload");
                return true;
            }
            return false;
        }

        private void ComputeSoundPreReload()
        {
            sounds[1].Use(0);
            if (type == ItemType.BenelliM4)
            {
                float delay = reloadTime / MagazineMax;
                sounds[1].Use(delay);
                sounds[1].Use(delay * 2);
                sounds[1].Use(delay * 3);
                sounds[1].Use(delay * 4);
            }
        }

        private void ComputeSoundAfterReload()
        {
            sounds[2].Use(0);
            if (sounds[4] != null)
                sounds[4].Use(0.1f);
        }

        private void ComputeBoltUpSound()
        {
            if (sounds.Length > 4)
            {
                if (sounds[5] != null)
                {
                    sounds[5].Use(0.1f);
                }
                if (sounds[6] != null)
                {
                    sounds[6].Use(0.1f);
                }
            }
        }

        private IEnumerator ComputeReload()
        {
            if (!isReloading)
            {
                isReloading = true;
                yield return new WaitForSeconds(reloadTime);
            }

            ComputeSoundAfterReload();
            ComputeBoltUpSound();
            MagazineAmount = MagazineMax;
            NotidyMunitionChanged();
            yield return null;
        }

        public void UpdateBullets()
        {
            NotidyMunitionChanged();
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
