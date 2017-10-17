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
        private int magazineMaxAmount;

        public event OnMunitionChanged OnMunitionChanged;

        private static int weight = 0;

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
                BulletFactory.CreateBullet(bulletSpawnPoint, bulletSpeed, bulletLivingTime);
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
            return weight;
        }
    }
}


