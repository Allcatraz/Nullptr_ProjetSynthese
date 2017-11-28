using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class AchivementUpdaterForInventory : GameScript
    {
        private Inventory inventory;
        private AchivementController achivementController;

        private void InjectAchivementUpdaterForInventory([EntityScope] Inventory inventory,
                                                         [ApplicationScope] AchivementController achivementController)
        {
            this.inventory = inventory;
            this.achivementController = achivementController;
        }

        public void Awake()
        {
            InjectDependencies("InjectAchivementUpdaterForInventory");
        }

        public void OnEnable()
        {
            inventory.ProtectionEquipped += OnProtectionEquipped;
            inventory.OnWeaponEquip += OnWeaponEquip;
        }

        public void OnDisable()
        {
            inventory.ProtectionEquipped -= OnProtectionEquipped;
            inventory.OnWeaponEquip -= OnWeaponEquip;
        }

        private void OnWeaponEquip()
        {
            achivementController.TriggerFirstWeapon();
            achivementController.CheckAchivements();
        }

        private void OnProtectionEquipped(ProtectionOfPlayer protectionOfPlayer)
        {
            achivementController.AddProtectionOfPlayer(protectionOfPlayer);
        }
    }
}