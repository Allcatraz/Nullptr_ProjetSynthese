using UnityEngine;

namespace ProjetSynthese
{
    public delegate void OnMunitionChanged();

    public class Weapon : Usable
    {
        private const int Weight = 0;

        [SerializeField]
        private GameObject bulletSpawnPoint;
        [SerializeField]
        private GameObject chamber;
        [SerializeField]
        private float bulletSpeed;
        [SerializeField]
        private float bulletLivingTime;
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


