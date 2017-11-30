using Harmony;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public enum CellObjectType {Weapon,Protection,Inventory, Ground}

    public class CellObject : GameScript
    {
        [Tooltip("Le type de bouton que l'objet représente")]
        [SerializeField] public CellObjectType cellObjectType;
        [Tooltip("La zone de texte allant contenir la quantité de l'objet")]
        [SerializeField] public Text numberText;
        [Tooltip("La zone de texte allant contenir le nom de l'objet")]
        [SerializeField] private Text textName;

        private Button button;
        private bool willDropItem = false;
        private bool isDragging = false;
        private GameObject itemToDrag;
        private Vector3 startPosition;
        private GameObject canvasMenu;
        private GameObject oldParent;  
        private PlayerController player;
        private Inventory playerInventory;
        private MouseInputSensor mouseInputSensor;

        public EquipWeaponAt EquipAt { get; set; }
        public CellObjectType DropAtType { get; set; }
        public InventoryController Control { get; set; }
        public Inventory Inventory { get; set; }
        public ObjectContainedInventory CellContained { get; set; }
        public Image ImageBackground { get; private set; }

        private void Awake()
        {
            InjectDependencies("InjectCellObject");
        }

        private void Update()
        {
            ChangeWeaponSlotFromKeyPressed();
            ChangeDropItemInInventory();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        private void InjectCellObject([EntityScope] Button button,
                                    [EntityScope] Image image,
                                    [ApplicationScope] MouseInputSensor mouseInputSensor)
        {
            this.button = button;
            ImageBackground = image;
            this.mouseInputSensor = mouseInputSensor;
            EquipAt = EquipWeaponAt.Primary;
            button.onClick.AddListener(OnClickButton);
        }

        public void InstantiateCellObjectFromCell(ObjectContainedInventory cell)
        {
            InstantiateCellObjectVariables();
            CellContained = cell;
            int compteur = CellContained.GetCompteur();
            SetTextName(GetNameOfCellObject());
            if (cellObjectType != CellObjectType.Weapon)
            {
                SetTextNumber(compteur);
            }
            SetImageBackground();
        }

        public void OnBeginDrag()
        {
            isDragging = true;
            itemToDrag.transform.SetParent(canvasMenu.transform);
        }

        public void Drag()
        {
            itemToDrag.transform.position = mouseInputSensor.GetPosition();
        }

        public void OnEndDrag()
        {
            isDragging = false;
            if (WasDroppedAtAndObjectIsNot(CellObjectType.Ground))
            {
                if (cellObjectType == CellObjectType.Inventory && willDropItem)
                {
                    ClickOnInventoryButton();
                }
            }
            else if (WasDroppedAtAndObjectIsNot(CellObjectType.Inventory))
            {
                if (cellObjectType == CellObjectType.Ground)
                {
                    TransferToInentoryPlayerFromGround();
                }
                if (cellObjectType == CellObjectType.Protection)
                {
                    ClickOnProtectionButton();
                }
                if (cellObjectType == CellObjectType.Weapon)
                {
                    ClickOnWeaponButton();
                }
            }
            else if (WasDroppedAtAndObjectIsNot(CellObjectType.Protection))
            {
                if (cellObjectType == CellObjectType.Inventory)
                {
                    ClickOnInventoryButton();
                }
            }
            else if (WasDroppedAtAndObjectIsNot(CellObjectType.Weapon))
            {
                if (cellObjectType == CellObjectType.Inventory)
                {
                    ClickOnInventoryButton();
                }
            }
            Destroy(itemToDrag);
            Inventory.NotifyInventoryChange();
        }

        private string GetNameOfCellObject()
        {
            string name;
            Item containedInCell = CellContained.GetItem();
            if (containedInCell.Type == ItemType.AmmoPack)
            {
                name = (containedInCell as AmmoPack).AmmoType.ToString();
            }
            else
            {
                name = containedInCell.Type.ToString();
            }
            int level = containedInCell.Level;
            ItemType typeToCheck = containedInCell.Type;
            if (level != 0)
            {
                name = GetNameFromItemWithLevel(level, typeToCheck);
            }

            return name;
        }

        private string GetNameFromItemWithLevel(int level, ItemType typeToCheck)
        {
            string name = "";
            if (typeToCheck == ItemType.Bag)
            {
                name = GetNameFromType(typeof(BagType), level);
            }
            else if (typeToCheck == ItemType.Helmet)
            {
                name = GetNameFromType(typeof(HelmetType), level);
            }
            else if (typeToCheck == ItemType.Vest)
            {
                name = GetNameFromType(typeof(VestType), level);
            }
            else if (typeToCheck == ItemType.Heal)
            {
                name = GetNameFromType(typeof(HealType), level);
            }
            else if (typeToCheck == ItemType.Boost)
            {
                name = GetNameFromType(typeof(BoostType), level);
            }
            return name;
        }

        private string GetNameFromType(Type type, int level)
        {
            string name = "";
            int toUse = level - 1;
            if (typeof(BagType) == type)
            {
                name = ((BagType)toUse).ToString();
            }
            if (typeof(HelmetType) == type)
            {
                name = ((HelmetType)toUse).ToString();
            }
            if (typeof(VestType) == type)
            {
                name = ((VestType)toUse).ToString();
            }
            if (typeof(HealType) == type)
            {
                name = ((HealType)toUse).ToString();
            }
            if (typeof(BoostType) == type)
            {
                name = ((BoostType)toUse).ToString();
            }
            return name;
        }

        private bool WasDroppedAtAndObjectIsNot(CellObjectType cellObjectType)
        {
            return DropAtType == cellObjectType && DropAtType != this.cellObjectType;
        }

        private void InstantiateCellObjectVariables()
        {
            player = Control.Player.GetComponent<PlayerController>();
            playerInventory = player.GetInventory();
            itemToDrag = transform.parent.gameObject;
            oldParent = GetTransformCorrectGrid(this.cellObjectType).gameObject;
            startPosition = itemToDrag.transform.position;
            canvasMenu = Control.gameObject;
        }
      
        private void OnClickButton()
        {
            if (!isDragging)
            {
                if (cellObjectType == CellObjectType.Inventory)
                {
                    ClickOnInventoryButton();
                }
                if (cellObjectType == CellObjectType.Weapon)
                {
                    ClickOnWeaponButton();
                }
                if (cellObjectType == CellObjectType.Protection)
                {
                    ClickOnProtectionButton();
                }
                if (cellObjectType == CellObjectType.Ground)
                {
                    TransferToInentoryPlayerFromGround();
                }
            }
           
        }

        private Transform GetTransformCorrectGrid(CellObjectType gridToFind)
        {
            if (gridToFind == CellObjectType.Ground)
            {
                return Control.GridNerbyItem;
            }
            else if (gridToFind == CellObjectType.Inventory)
            {
                return Control.GridInventoryPlayer;
            }
            else if (gridToFind == CellObjectType.Protection)
            {
                return Control.GridProtectionPlayer;
            }
            else if (gridToFind == CellObjectType.Weapon)
            {
                return Control.GridEquippedByPlayer;
            }
            else
            {
                return null;
            }
        }

        private void ClickOnProtectionButton()
        {
            if (CellContained.GetItem() as Helmet)
            {
                Inventory.UnequipHelmet();
            }
            else if (CellContained.GetItem() as Vest)
            {
                Inventory.UnequipVest();
            }
            else if (CellContained.GetItem() as Bag)
            {
                Inventory.UnequipBag();
            }
        }

        private void ClickOnWeaponButton()
        {
            if (CellContained.GetItem() as Weapon)
            {
                Inventory.UnequipWeaponAt(EquipAt);
            }
            else if (CellContained.GetItem() as Grenade)
            {
                Inventory.UnequipGrenade();
            }
        }

        private void ClickOnInventoryButton()
        {
            if (willDropItem)
            {
                Inventory.Drop(CellContained);
            }
            else if (CellContained.GetItem() as Grenade)
            {
                Inventory.EquipGrenade(CellContained);
            }
            else if (CellContained.GetItem() as Weapon)
            {
                Inventory.EquipWeaponAt(EquipAt, CellContained);
            }
            else if (CellContained.GetItem() as Helmet)
            {
                Inventory.EquipHelmet(CellContained);
            }
            else if (CellContained.GetItem() as Vest)
            {
                Inventory.EquipVest(CellContained);
            }
            else if (CellContained.GetItem() as Heal || CellContained.GetItem() as Boost)
            {
                //Bug fix temporaire
                CellContained.GetItem().Player = player.gameObject;
                //Fin bug fix temporaire

                (CellContained.GetItem() as Usable).Use(); 
                Inventory.CheckMultiplePresenceAndRemove(CellContained);
            }
            else if (CellContained.GetItem() as Bag)
            {
                Inventory.EquipBag(CellContained);
            }
        }

        private void TransferToInentoryPlayerFromGround()
        {
            GameObject toAdd = CellContained.GetItem().gameObject;
            player.TakeItemFromInventory(toAdd);
            CellContained.RemoveOneFromCompteur();
            Control.CreateCellsForNearbyItem();
        }

        private void ChangeWeaponSlotFromKeyPressed()
        {
            if (Input.GetKeyDown(ActionKey.Instance.ChangeWeaponSlot))
            {
                if (EquipAt != EquipWeaponAt.Secondary)
                {
                    EquipAt = EquipWeaponAt.Secondary;
                }
            }
            if (Input.GetKeyUp(ActionKey.Instance.ChangeWeaponSlot))
            {
                if (EquipAt != EquipWeaponAt.Primary)
                {
                    EquipAt = EquipWeaponAt.Primary;
                }
            }
        }

        private void ChangeDropItemInInventory()
        {
            if (Input.GetKeyDown(ActionKey.Instance.DropItemTrigger))
            {
                if (!willDropItem)
                {
                    willDropItem = true;
                }
            }
            if (Input.GetKeyUp(ActionKey.Instance.DropItemTrigger))
            {
                if (willDropItem)
                {
                    willDropItem = false;
                }
            }
        }

        private void SetImageBackground()
        {
            ItemSpriteSelector itemSpriteSelector = Control.GetComponent<ItemSpriteSelector>();
            ItemType type = CellContained.GetItem().Type;
            Sprite newSprite = null;
            if (type == ItemType.AmmoPack)
            {
                newSprite = itemSpriteSelector.GetSpriteForType(type, 0,(CellContained.GetItem() as AmmoPack).AmmoType, true);
            }
            else
            {
                newSprite = itemSpriteSelector.GetSpriteForType(type, CellContained.GetItem().Level);
            }
            ImageBackground.sprite = newSprite;
        }

        private void SetTextName(string name)
        {
            if (textName != null)
            {
                textName.text = name;
            }
        }

        private void SetTextNumber(int compteur)
        {
            if (numberText != null)
            {
                numberText.text = compteur.ToString();
            }
        }  
    }
}