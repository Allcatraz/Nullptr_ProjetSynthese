using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace ProjetSynthese
{
    public class InventoryController : GameScript
    {
        [Tooltip("La location de la grille d'affichage de l'inventaire du joueur")]
        [SerializeField] private Transform gridInventoryPlayer;
        [Tooltip("La location de la grille d'affichage des armes du joueur")]
        [SerializeField] private Transform gridWeaponEquippedByPlayer;
        [Tooltip("La location de la grille d'affichage des objets de protection du joueur")]
        [SerializeField] private Transform gridProtectionPlayer;
        [Tooltip("La location de la grille d'affichage de l'inventaire du sol")]
        [SerializeField] private Transform gridGroundInventory;

        [Tooltip("Le prefab utilisé pour la création des boutons utilisé par les armes du joueur")]
        [SerializeField] private GameObject cellEquippedWeaponPrefabs;
        [Tooltip("Le prefab utilisé pour la création des boutons utilisé par les objets de protection du joueur")]
        [SerializeField] private GameObject cellProtectionItemPrefabs;
        [Tooltip("Le prefab utilisé pour la création des boutons utilisé par les objets de l'inventaire du joueur")]
        [SerializeField] private GameObject cellInventoryPrefab;
        [Tooltip("Le prefab utilisé pour la création des boutons utilisé par les objets de l'inventaire du sol")]
        [SerializeField] private GameObject cellGroundPrefab;

        [Tooltip("Contien l'inventaire du sol")]
        [SerializeField] private Inventory inventoryGround;
        [Tooltip("Contien le sensor a utilisé pour detecter les objets proches")]
        [SerializeField] private ItemSensor sensorItem;

        private PlayerMoveEventChannel playerMoveEventChannel;       
        private InventoryChangedEventChannel inventoryChangedEventChannel;

        private Inventory inventory;
        public GameObject Player { get; set; }

        public Transform GridInventoryPlayer
        {
            get
            {
                return gridInventoryPlayer;
            }

            private set
            {
                gridInventoryPlayer = value;
            }
        }
        public Transform GridEquippedByPlayer
        {
            get
            {
                return gridWeaponEquippedByPlayer;
            }

            private set
            {
                gridWeaponEquippedByPlayer = value;
            }
        }
        public Transform GridProtectionPlayer
        {
            get
            {
                return gridProtectionPlayer;
            }

            private set
            {
                gridProtectionPlayer = value;
            }
        }
        public Transform GridNerbyItem
        {
            get
            {
                return gridGroundInventory;
            }

            private set
            {
                gridGroundInventory = value;
            }
        }

        private void InjectEventSensor([EventChannelScope] PlayerMoveEventChannel playerMoveEventChannel,
                                        [EventChannelScope] InventoryChangedEventChannel inventoryChangedEventChannel)
        {
            this.playerMoveEventChannel = playerMoveEventChannel;
            this.inventoryChangedEventChannel = inventoryChangedEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectEventSensor");
            playerMoveEventChannel.OnEventPublished += OnPlayerMoved;
            inventoryChangedEventChannel.OnEventPublished += OnInventoryChanged;

        }

        private void OnDestroy()
        {
            playerMoveEventChannel.OnEventPublished -= OnPlayerMoved;
            inventoryChangedEventChannel.OnEventPublished -= OnInventoryChanged;
        }

        private void InstantiateCellObject(GameObject cellPrefabs, Transform grid, ObjectContainedInventory item, EquipWeaponAt equipWeaponAt = EquipWeaponAt.Primary)
        {
            GameObject gameCellObject = Instantiate(cellPrefabs);
            gameCellObject.transform.SetParent(grid, false);
            CellObject cellObject = gameCellObject.GetComponentInChildren<CellObject>();
            cellObject.Inventory = inventory;
            cellObject.Control = this;
            cellObject.InstantiateCellObjectFromCell(item);
            if (cellPrefabs == cellEquippedWeaponPrefabs)
            {
                cellObject.EquipAt = equipWeaponAt;
            }
        }

        public void CreateCellsForInventoryPlayer()
        {
            ClearGrid(gridInventoryPlayer);
            foreach (ObjectContainedInventory item in inventory.ListInventory)
            {
                InstantiateCellObject(cellInventoryPrefab, gridInventoryPlayer, item);
            }
        }

        public void CreateCellsForWeaponByPlayer()
        {
            ClearGrid(gridWeaponEquippedByPlayer);
            if (inventory.GetPrimaryWeapon() != null)
            {
                InstantiateCellObject(cellEquippedWeaponPrefabs, gridWeaponEquippedByPlayer, inventory.GetPrimaryWeapon(), EquipWeaponAt.Primary);
            }
            if (inventory.GetSecondaryWeapon() != null)
            {
                InstantiateCellObject(cellEquippedWeaponPrefabs, gridWeaponEquippedByPlayer, inventory.GetSecondaryWeapon(), EquipWeaponAt.Secondary);
            }
            if (inventory.GetGrenade() != null)
            {
                InstantiateCellObject(cellEquippedWeaponPrefabs, gridWeaponEquippedByPlayer, inventory.GetGrenade());
            }
        }

        public void CreateCellsForProtectionPlayer()
        {
            ClearGrid(gridProtectionPlayer);
            if (inventory.GetVest() != null)
            {
                InstantiateCellObject(cellProtectionItemPrefabs, gridProtectionPlayer, inventory.GetVest());
            }
            if (inventory.GetHelmet() != null)
            {
                InstantiateCellObject(cellProtectionItemPrefabs, gridProtectionPlayer, inventory.GetHelmet());
            }
            if (inventory.GetBag() != null)
            {
                InstantiateCellObject(cellProtectionItemPrefabs, gridProtectionPlayer, inventory.GetBag());
            }
        }

        public void CreateCellsForNearbyItem()
        {
            ClearGrid(gridGroundInventory);
            CreateInventoryGround();
            if (inventoryGround.ListInventory != null)
            {
                foreach (ObjectContainedInventory item in inventoryGround.ListInventory)
                {
                    InstantiateCellObject(cellGroundPrefab, gridGroundInventory, item);
                }
            }
        }

        private void OnInventoryChanged(InventoryChangeEvent newEvent)
        {
            inventory = newEvent.Inventory;
            Player = inventory.Parent;
            CreateCellsForInventoryPlayer();
            CreateCellsForWeaponByPlayer();
            CreateCellsForProtectionPlayer();
            CreateCellsForNearbyItem();
        }

        private void OnPlayerMoved(PlayerMoveEvent newEvent)
        {
            CreateCellsForNearbyItem();
        }

        private void CreateInventoryGround()
        {
            if (inventory != null)
            {
                inventoryGround.ResetInventory();
                List<GameObject> listTemp = sensorItem.GetAllItems(inventory.transform);
                foreach (GameObject item in listTemp)
                {
                    inventoryGround.Add(item);
                }
            }        
        }

        private void ClearGrid(Transform grid)
        {
            foreach (Transform child in grid)
            {
                Destroy(child.gameObject);
            }
        }
    }
}