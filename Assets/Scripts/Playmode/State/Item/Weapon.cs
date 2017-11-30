using System.Collections;
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
       
        public event OnMunitionChanged OnMunitionChanged;

        private const int Weight = 0;
        private float timerForShoot = 0;
        private PlaySound[] sounds;

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
            sounds = GetComponentsInChildren<PlaySound>();
            MagazineAmount = MagazineMax;
        }

        private void Update()
        {
            timerForShoot += Time.deltaTime;
        }

        public void ChangeWeaponSound()
        {
            sounds[3].Use();
        }

        public override bool Use()
        {
            if (MagazineAmount > 0 && timerForShoot >= shootTime)
            {
                sounds[0].Use();
                MagazineAmount -= 1;
                NotidyMunitionChanged();
                timerForShoot = 0;
                return true;
            }
            return false;
        }

        public bool Reload(Inventory inventory)
        {
            if (inventory.UseAmmoPack(ammoType))
            {
                sounds[1].Use();
                StartCoroutine("ComputeReload");
                return true;
            }
            return false;
        }

        private IEnumerator ComputeReload()
        {
            bool doReload = false;
            if (!doReload)
            {
                doReload = false;
                yield return new WaitForSeconds(reloadTime);
            }
            sounds[2].Use();
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
