using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace ProjetSynthese
{
    //BEN_CORRECTION : J'arrête sur cette classe pour ce dossier. Mes commentaires précédents commencent à se répéter.
    
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

        public void CreateCellsForInventoryPlayer()
        {
            ClearGrid(gridInventoryPlayer);
            foreach (ObjectContainedInventory item in inventory.ListInventory)
            {
                GameObject gameCellObject = Instantiate(cellInventoryPrefab);
                gameCellObject.transform.SetParent(gridInventoryPlayer, false);
                CellObject cellObject = gameCellObject.GetComponentInChildren<CellObject>();
                cellObject.Inventory = inventory;
                cellObject.Control = this;
                cellObject.InstantiateCellObjectFromCell(item);
            }
        }

        public void CreateCellsForWeaponByPlayer()
        {
            ClearGrid(gridWeaponEquippedByPlayer);
            if (inventory.GetPrimaryWeapon() != null)
            {
                GameObject cellWeaponTemp1 = Instantiate(cellEquippedWeaponPrefabs);
                cellWeaponTemp1.transform.SetParent(gridWeaponEquippedByPlayer, false);
                CellObject cellObject = cellWeaponTemp1.GetComponentInChildren<CellObject>();
                cellObject.Inventory = inventory;
                cellObject.Control = this;
                cellObject.InstantiateCellObjectFromCell(inventory.GetPrimaryWeapon());
                cellObject.EquipAt = EquipWeaponAt.Primary;
            }
            if (inventory.GetSecondaryWeapon() != null)
            {
                GameObject cellWeaponTemp2 = Instantiate(cellEquippedWeaponPrefabs);
                cellWeaponTemp2.transform.SetParent(gridWeaponEquippedByPlayer, false);
                CellObject cellObject = cellWeaponTemp2.GetComponentInChildren<CellObject>();
                cellObject.Inventory = inventory;
                cellObject.Control = this;
                cellObject.InstantiateCellObjectFromCell(inventory.GetSecondaryWeapon());
                cellObject.EquipAt = EquipWeaponAt.Secondary;
            }
        }

        public void CreateCellsForProtectionPlayer()
        {
            ClearGrid(gridProtectionPlayer);
            if (inventory.GetVest() != null)
            {
                GameObject cellProtectionTemp1 = Instantiate(cellProtectionItemPrefabs);
                cellProtectionTemp1.transform.SetParent(gridProtectionPlayer, false);
                CellObject cellObject = cellProtectionTemp1.GetComponentInChildren<CellObject>();
                cellObject.Inventory = inventory;
                cellObject.Control = this;
                cellObject.InstantiateCellObjectFromCell(inventory.GetVest());
            }

            if (inventory.GetHelmet() != null)
            {
                GameObject cellProtectionTemp2 = Instantiate(cellProtectionItemPrefabs);
                cellProtectionTemp2.transform.SetParent(gridProtectionPlayer, false);
                CellObject cellObject = cellProtectionTemp2.GetComponentInChildren<CellObject>();
                cellObject.Inventory = inventory;
                cellObject.Control = this;
                cellObject.InstantiateCellObjectFromCell(inventory.GetHelmet());
            }
            if (inventory.GetBag() != null)
            {
                GameObject cellProtectionTemp3 = Instantiate(cellProtectionItemPrefabs);
                cellProtectionTemp3.transform.SetParent(gridProtectionPlayer, false);
                CellObject cellObject = cellProtectionTemp3.GetComponentInChildren<CellObject>();
                cellObject.Inventory = inventory;
                cellObject.Control = this;
                cellObject.InstantiateCellObjectFromCell(inventory.GetBag());
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
                    GameObject gameCellObject = Instantiate(cellGroundPrefab);
                    gameCellObject.transform.SetParent(gridGroundInventory, false);
                    CellObject cellObject = gameCellObject.GetComponentInChildren<CellObject>();
                    cellObject.Inventory = inventoryGround;
                    cellObject.Control = this;
                    cellObject.InstantiateCellObjectFromCell(item);
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