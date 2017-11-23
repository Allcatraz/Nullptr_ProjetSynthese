using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class AchivementUpdaterForInventory : GameScript
    {
        private Inventory inventory;
        private ProtectionOfPlayerRepository protectionOfPlayerRepository;
        private AchivementController achivementController;
        private AchivementRepository achivementRepository;

        private void InjectAchivementUpdaterForInventory([EntityScope] Inventory inventory,
                                                         [ApplicationScope] ProtectionOfPlayerRepository protectionOfPlayerRepository,
                                                         [ApplicationScope] AchivementController achivementController,
                                                         [ApplicationScope] AchivementRepository achivementRepository)
        {
            this.inventory = inventory;
            this.protectionOfPlayerRepository = protectionOfPlayerRepository;
            this.achivementController = achivementController;
            this.achivementRepository = achivementRepository;
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
            protectionOfPlayer.PlayerId = achivementController.GetPlayer().Id;
            protectionOfPlayerRepository.AddProtectionOfPlayer(protectionOfPlayer);
            achivementController.CheckAchivements();
        }
    }
}