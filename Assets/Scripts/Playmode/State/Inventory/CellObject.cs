using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ProjetSynthese
{
    public enum CellObjectType {Weapon,Protection,Inventory, Ground}

    public class CellObject : GameScript
    {
        [SerializeField] public CellObjectType cellObjectType;
        [SerializeField] private KeyCode keyWeaponSlot;
        [SerializeField] private KeyCode keyDroppingItem;
        private Button button;
        private bool willDropItem = false;
        private GameObject itemToDrag;
        private Vector3 startPosition;
        private GameObject canvasMenu;
        private GameObject oldParent;
        private bool isDragging = false;
        private PlayerController player;
        private Inventory playerInventory;

        public EquipWeaponAt EquipAt { get; set; }
        public CellObjectType DropAtType { get; set; }
        public InventoryController control;
        public Inventory Inventory { get; set; }
        public Cell CellContained { get; set; }
        public Image ImageBackground { get; private set; }
        public Text TextName { get; private set; }
        public Text TextNumber { get; private set; }

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

        private void InjectCellObject([EntityScope] Text textName,
                                    [EntityScope] Button button)
        {
            this.button = button;
            TextName = textName;
            // Avec unity editor
            button.onClick.AddListener(OnClickButton);
            EquipAt = EquipWeaponAt.Primary;
            //this.ImageBackground = imageBackground;
        }

        public void InstantiateCellObjectFromCell(Cell cell)
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
            itemToDrag.transform.position = Input.mousePosition;
        }

        public void OnEndDrag()
        {
            isDragging = false;
            if (WasDroppedAt(CellObjectType.Ground))
            {
                if (cellObjectType == CellObjectType.Inventory && willDropItem)
                {
                    ClickOnInventoryButton();
                }
            }
            else if (WasDroppedAt(CellObjectType.Inventory))
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
            else if (WasDroppedAt(CellObjectType.Protection))
            {
                if (cellObjectType == CellObjectType.Inventory)
                {
                    ClickOnInventoryButton();
                }
            }
            else if (WasDroppedAt(CellObjectType.Weapon))
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
            if (containedInCell.Level != 0)
            {
                name += " " + containedInCell.Level;
            }

            return name;
        }

        private bool WasDroppedAt(CellObjectType cellObjectType)
        {
            return DropAtType == cellObjectType && DropAtType != cellObjectType;
        }

        private void InstantiateCellObjectVariables()
        {
            player = control.Player.GetComponent<PlayerController>();
            playerInventory = player.GetInventory();
            itemToDrag = transform.parent.gameObject;
            oldParent = GetTransformCorrectGrid(this.cellObjectType).gameObject;
            startPosition = itemToDrag.transform.position;
            canvasMenu = control.gameObject;
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
                return control.GridNerbyItem;
            }
            else if (gridToFind == CellObjectType.Inventory)
            {
                return control.GridInventoryPlayer;
            }
            else if (gridToFind == CellObjectType.Protection)
            {
                return control.GridProtectionPlayer;
            }
            else if (gridToFind == CellObjectType.Weapon)
            {
                return control.GridEquippedByPlayer;
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
        }

        private void ClickOnInventoryButton()
        {
            if (willDropItem)
            {
                Inventory.Drop(CellContained);
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
                CellContained.GetItem().Use(); 
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
            if ((object)CellContained.GetItem() != null)
            {
                playerInventory.Add(toAdd, player.gameObject);
                if (toAdd.GetComponent<Item>() is Weapon)
                {
                    toAdd.transform.SetParent(player.GetWeaponHolderTransform());
                }
                else
                {
                    toAdd.transform.SetParent(player.GetInventoryTransform());
                }

                toAdd.SetActive(false);
            }
            CellContained.RemoveOneFromCompteur();
            control.CreateCellsForNearbyItem();
        }

        private void ChangeWeaponSlotFromKeyPressed()
        {
            if (Input.GetKeyDown(keyWeaponSlot))
            {
                if (EquipAt != EquipWeaponAt.Secondary)
                {
                    EquipAt = EquipWeaponAt.Secondary;
                }
            }
            if (Input.GetKeyUp(keyWeaponSlot))
            {
                if (EquipAt != EquipWeaponAt.Primary)
                {
                    EquipAt = EquipWeaponAt.Primary;
                }
            }
        }

        private void ChangeDropItemInInventory()
        {
            if (Input.GetKeyDown(keyDroppingItem))
            {
                if (!willDropItem)
                {
                    willDropItem = true;
                }
            }
            if (Input.GetKeyUp(keyDroppingItem))
            {
                if (willDropItem)
                {
                    willDropItem = false;
                }
            }
        }

        private void SetImageBackground()
        {
            
        }

        private void SetTextName(string name)
        {
            TextName.text = name;
        }

        private void SetTextNumber(int compteur)
        {
            TextName.text += " " + compteur;
        }  
    }
}