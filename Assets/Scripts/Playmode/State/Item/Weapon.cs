using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnMunitionChanged();

    public class Weapon : Item
    {
        [SerializeField]
        private GameObject bulletSpawnPoint;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float bulletLivingTime;
        [SerializeField]
        private GameObject bulletPrefab;
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
            Reload();            
        }

        public override void Use()
        {
            if (MagazineAmount > 0)
            {
                BulletFabric.BulletPrefab = bulletPrefab;
                BulletFabric.CreateBullet(bulletSpawnPoint, bulletSpeed, bulletLivingTime);
                MagazineAmount -= 1;
                NotidyMunitionChanged();
            }
        }

        public void Reload()
        {
            MagazineAmount = magazineMaxAmount;
            NotidyMunitionChanged();
        }

        private void NotidyMunitionChanged()
        {
            if (OnMunitionChanged != null) OnMunitionChanged();
        }
    }
}


