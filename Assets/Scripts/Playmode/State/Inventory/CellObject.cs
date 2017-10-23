using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ProjetSynthese
{
    public enum ButtonType {Weapon,Protection,Inventory, Ground}

    public class CellObject : GameScript
    {
        [SerializeField] public ButtonType buttonType;
        [SerializeField] private KeyCode keyWeaponSlot;
        [SerializeField] private KeyCode keyDroppingItem;
        private Button button;
        private bool droppingItemInventory = false;
        private GameObject itemToDrag;
        private Vector3 startPosition;
        private GameObject canvasMenu;
        private GameObject oldParent;
        private bool isDragging = false;
        private PlayerController player;
        private Inventory playerInventory;

        public EquipWeaponAt EquipAt { get; set; }
        public ButtonType DropAtType { get; set; }
        public InventoryController control;
        public Inventory Inventory { get; set; }
        public Cell IsItem { get; set; }
        public Image ImageBackground { get; private set; }
        public Text TextName { get; private set; }
        public Text TextNumber { get; private set; }


        public void InstantiateFromCell(Cell cell)
        {
            InstantiateVariable();

            string name = cell.GetItem().Type.ToString();
            if (cell.GetItem().Type == ItemType.AmmoPack)
            {
                name = (cell.GetItem() as AmmoPack).AmmoType.ToString();
            }
            IsItem = cell;
            int compteur = cell.GetCompteur();
            if (cell.GetItem().Level != 0)
            {
                name += " " + cell.GetItem().Level;
            }
            SetTextName(name);
            if (buttonType != ButtonType.Weapon)
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
       
        public void OnEndDrag()
        {
            isDragging = false;
            if (DropAtType == ButtonType.Ground && DropAtType != buttonType)
            {
                if (buttonType == ButtonType.Inventory && droppingItemInventory)
                {
                    ClickOnInventoryButton();
                }
            }
            else if (DropAtType == ButtonType.Inventory && DropAtType != buttonType)
            {
                if (buttonType == ButtonType.Ground)
                {
                    ClickOnGroundButton();
                }
                if (buttonType == ButtonType.Protection)
                {
                    ClickOnProtectionButton();
                }
                if (buttonType == ButtonType.Weapon)
                {
                    ClickOnWeaponButton();
                }
            }
            else if (DropAtType == ButtonType.Protection && DropAtType != buttonType)
            {
                if (buttonType == ButtonType.Inventory)
                {
                    ClickOnInventoryButton();
                }
            }
            else if (DropAtType == ButtonType.Weapon && DropAtType != buttonType)
            {
                if (buttonType == ButtonType.Inventory)
                {
                    ClickOnInventoryButton();
                }
            }
            Destroy(itemToDrag);
            Inventory.NotifyInventoryChange();            
        }

        public void Drag()
        {
            itemToDrag.transform.position = Input.mousePosition;
        }

        private void InstantiateVariable()
        {
            player = control.Player.GetComponent<PlayerController>();
            playerInventory = player.GetInventory();
            itemToDrag = transform.parent.gameObject;
            oldParent = GetTransformCorrectGrid(this.buttonType).gameObject;
            startPosition = itemToDrag.transform.position;
            canvasMenu = control.gameObject;
        }

        private void InjectCellObject([EntityScope] Text textName,
                                    [EntityScope] Button button)
        {
            this.button = button;
            this.TextName = textName;
            button.onClick.AddListener(TaskOnClick);
            EquipAt = EquipWeaponAt.Primary;
            //this.ImageBackground = imageBackground;
        }
        private void TaskOnClick()
        {
            if (!isDragging)
            {
                if (buttonType == ButtonType.Inventory)
                {
                    ClickOnInventoryButton();
                }
                if (buttonType == ButtonType.Weapon)
                {
                    ClickOnWeaponButton();
                }
                if (buttonType == ButtonType.Protection)
                {
                    ClickOnProtectionButton();
                }
                if (buttonType == ButtonType.Ground)
                {
                    ClickOnGroundButton();
                }
            }
           
        }

        private Transform GetTransformCorrectGrid(ButtonType gridToFind)
        {
            if (gridToFind == ButtonType.Ground)
            {
                return control.GridNerbyItem;
            }
            else if (gridToFind == ButtonType.Inventory)
            {
                return control.GridInventoryPlayer;
            }
            else if (gridToFind == ButtonType.Protection)
            {
                return control.GridProtectionPlayer;
            }
            else if (gridToFind == ButtonType.Weapon)
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
            if (IsItem.GetItem() as Helmet)
            {
                Inventory.UnequipHelmet();
            }
            else if (IsItem.GetItem() as Vest)
            {
                Inventory.UnequipVest();
            }
            else if (IsItem.GetItem() as Bag)
            {
                Inventory.UnequipBag();
            }
        }

        private void ClickOnWeaponButton()
        {
            if (IsItem.GetItem() as Weapon)
            {
                Inventory.UnequipWeaponAt(EquipAt);
            }
        }

        private void ClickOnInventoryButton()
        {
            if (droppingItemInventory)
            {
                Inventory.Drop(IsItem);
            }
            else if (IsItem.GetItem() as Weapon)
            {
                Inventory.EquipWeaponAt(EquipAt, IsItem);
            }
            else if (IsItem.GetItem() as Helmet)
            {
                Inventory.EquipHelmet(IsItem);
            }
            else if (IsItem.GetItem() as Vest)
            {
                Inventory.EquipVest(IsItem);
            }
            else if (IsItem.GetItem() as Heal || IsItem.GetItem() as Boost)
            {
                //Bug fix temporaire
                IsItem.GetItem().Player = player.gameObject;
                //Fin bug fix temporaire
                IsItem.GetItem().Use(); 
                Inventory.CheckMultiplePresenceAndRemove(IsItem);
            }
            else if (IsItem.GetItem() as Bag)
            {
                Inventory.EquipBag(IsItem);
            }
        }

        private void ClickOnGroundButton()
        {
            GameObject toAdd = IsItem.GetItem().gameObject;
            if ((object)IsItem.GetItem() != null)
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
            IsItem.RemoveOneFromCompteur();
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
                if (!droppingItemInventory)
                {
                    droppingItemInventory = true;
                }
            }
            if (Input.GetKeyUp(keyDroppingItem))
            {
                if (droppingItemInventory)
                {
                    droppingItemInventory = false;
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
    }
}