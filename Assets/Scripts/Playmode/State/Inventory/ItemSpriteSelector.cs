using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class ItemSpriteSelector : GameScript
    {
        [Tooltip("Le sprite représentant le type d'ammo 556.")]
        [SerializeField] private Sprite ammo556;
        [Tooltip("Le sprite représentant le type d'ammo winchester.")]
        [SerializeField] private Sprite ammoWinchester;
        [Tooltip("Le sprite représentant le type d'ammo 12.")]
        [SerializeField] private Sprite ammoCalibre12;
        [Tooltip("Le sprite représentant le type d'ammo 45acp.")]
        [SerializeField] private Sprite ammo45acp;
        [Tooltip("Le sprite représentant le type d'item vest level 1.")]
        [SerializeField] private Sprite vestLevel1;
        [Tooltip("Le sprite représentant le type d'item vest level 2.")]
        [SerializeField] private Sprite vestLevel2;
        [Tooltip("Le sprite représentant le type d'item vest level 3.")]
        [SerializeField] private Sprite vestLevel3;
        [Tooltip("Le sprite représentant le type d'item bag level 1.")]
        [SerializeField] private Sprite bagLevel1;
        [Tooltip("Le sprite représentant le type d'item bag level 2.")]
        [SerializeField] private Sprite bagLevel2;
        [Tooltip("Le sprite représentant le type d'item bag level 3.")]
        [SerializeField] private Sprite bagLevel3;
        [Tooltip("Le sprite représentant le type d'item helmet level 1.")]
        [SerializeField] private Sprite helmetLevel1;
        [Tooltip("Le sprite représentant le type d'item bag level 2.")]
        [SerializeField] private Sprite helmetLevel2;
        [Tooltip("Le sprite représentant le type d'item bag level 3.")]
        [SerializeField] private Sprite helmetLevel3;
        [Tooltip("Le sprite représentant le type d'item heal level 1.")]
        [SerializeField] private Sprite healLevel1;
        [Tooltip("Le sprite représentant le type d'item heal level 2.")]
        [SerializeField] private Sprite healLevel2;
        [Tooltip("Le sprite représentant le type d'item heal level 3.")]
        [SerializeField] private Sprite healLevel3;
        [Tooltip("Le sprite représentant le type d'item boost level 1.")]
        [SerializeField] private Sprite boostLevel1;
        [Tooltip("Le sprite représentant le type d'item boost level 2.")]
        [SerializeField] private Sprite boostLevel2;
        [Tooltip("Le sprite représentant le type d'item boost level 3.")]
        [SerializeField] private Sprite boostLevel3;
        [Tooltip("Le sprite représentant le type d'item Saiga12.")]
        [SerializeField] private Sprite saiga12;
        [Tooltip("Le sprite représentant le type d'item AWM.")]
        [SerializeField] private Sprite awm;
        [Tooltip("Le sprite représentant le type d'item M1911.")]
        [SerializeField] private Sprite m1911;
        [Tooltip("Le sprite représentant le type d'item M16A4.")]
        [SerializeField] private Sprite m16A4;
        [Tooltip("Le sprite représentant le type d'item Grenade.")]
        [SerializeField] private Sprite grenade;

        public Sprite GetSpriteForType(ItemType itemType, int level = 0, AmmoType ammoType = AmmoType.Ammo9mm, bool isAmmo = false)
        {
            if (isAmmo)
            {
                return GetSpriteForAmmoType(ammoType);
            }
            else if (itemType == ItemType.Vest)
            {
                return GetSpriteForVest(level);
            }
            else if (itemType == ItemType.Bag)
            {
                return GetSpriteForBag(level);
            }
            else if (itemType == ItemType.Helmet)
            {
                return GetSpriteForHelmet(level);
            }
            else if (itemType == ItemType.Boost)
            {
                return GetSpriteForBoost(level);
            }
            else if (itemType == ItemType.Heal)
            {
                return GetSpriteForHeal(level);
            }
            else if (itemType == ItemType.Saiga12)
            {
                return saiga12;
            }
            else if (itemType == ItemType.MP5)
            {
                return m1911;
            }
            else if (itemType == ItemType.M16A4)
            {
                return m16A4;
            }
            else if (itemType == ItemType.AWM)
            {
                return awm;
            }
            else if (itemType == ItemType.Grenade)
            {
                return grenade;
            }
            else
            {
                return null;
            }
        }

        private Sprite GetSpriteForHeal(int level)
        {
            if (level == 1)
            {
                return healLevel1;
            }
            else if (level == 2)
            {
                return healLevel2;
            }
            else if (level == 3)
            {
                return healLevel3;
            }
            else
            {
                return null;
            }
        }

        private Sprite GetSpriteForBoost(int level)
        {
            if (level == 1)
            {
                return boostLevel1;
            }
            else if (level == 2)
            {
                return boostLevel2;
            }
            else if (level == 3)
            {
                return boostLevel3;
            }
            else
            {
                return null;
            }
        }

        private Sprite GetSpriteForHelmet(int level)
        {
            if (level == 1)
            {
                return helmetLevel1;
            }
            else if (level == 2)
            {
                return helmetLevel2;
            }
            else if (level == 3)
            {
                return helmetLevel3;
            }
            else
            {
                return null;
            }
        }

        private Sprite GetSpriteForBag(int level)
        {
            if (level == 1)
            {
                return bagLevel1;
            }
            else if (level == 2)
            {
                return bagLevel2;
            }
            else if (level == 3)
            {
                return bagLevel3;
            }
            else
            {
                return null;
            }
        }

        private Sprite GetSpriteForVest(int level)
        {
            if (level == 1)
            {
                return vestLevel1;
            }
            else if (level == 2)
            {
                return vestLevel2;
            }
            else if (level == 3)
            {
                return vestLevel3;
            }
            else
            {
                return null;
            }
        }

        private Sprite GetSpriteForAmmoType(AmmoType ammoType)
        {
            if (ammoType == AmmoType.Ammo9mm)
            {
                return ammo45acp;
            }
            else if(ammoType == AmmoType.Ammo556)
            {
                return ammo556;
            }
            else if (ammoType == AmmoType.AmmoCalibre12)
            {
                return ammoCalibre12;
            }
            else if (ammoType == AmmoType.AmmoWinchester)
            {
                return ammoWinchester;
            }
            else
            {
                return null;
            } 
        }
    }
}