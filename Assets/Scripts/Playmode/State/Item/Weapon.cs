using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnMunitionChanged();

    public class Weapon : Usable
    {
        private const int Weight = 0;

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

        public event OnMunitionChanged OnMunitionChanged;

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
                BulletFactory.CmdCreateBullet(bulletSpawnPoint, chamber, bulletSpeed, bulletLivingTime);
                MagazineAmount -= 1;
                NotidyMunitionChanged(); 
            }
        }

        public void Reload()
        {
            MagazineAmount = MagazineMax;
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


