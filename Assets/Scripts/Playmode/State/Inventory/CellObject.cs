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
        [SerializeField] private ButtonType buttonType;
        [SerializeField] private KeyCode keyWeaponSlot;
        [SerializeField] private KeyCode keyDroppingItem;
        private Button button;
        private bool droppingItemInventory = false;
        public EquipWeaponAt equipAt { get; set; }


        public Inventory inventory { get; set; }

        public Cell IsItem { get; set; }

        public Image ImageBackground { get; private set; }

        public Text TextName { get; private set; }

        public Text TextNumber { get; private set; }

        public void InstantiateFromCell(Cell cell)
        {   
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

        private void InjectCellObject([EntityScope] Text textName,
                                    [EntityScope] Button button)
        {
            this.button = button;
            this.TextName = textName;
            button.onClick.AddListener(TaskOnClick);
            equipAt = EquipWeaponAt.Primary;
            //this.ImageBackground = imageBackground;
        }

        private void TaskOnClick()
        {
            if (buttonType == ButtonType.Inventory)
            {
                if (droppingItemInventory)
                {
                    inventory.Drop(IsItem);
                }
                else if (IsItem.GetItem() as Weapon)
                {
                    inventory.EquipWeaponAt(equipAt, IsItem);
                }
                else if (IsItem.GetItem() as Helmet)
                {
                    inventory.EquipHelmet(IsItem);
                }
                else if (IsItem.GetItem() as Vest)
                {
                    inventory.EquipVest(IsItem);
                }
                else if (IsItem.GetItem() as Heal || IsItem.GetItem() as Boost)
                {
                    PlayerController player = inventory.parent.GetComponent<PlayerController>();
                    IsItem.GetItem().Player = player.gameObject;
                    IsItem.GetItem().Use();
                    inventory.CheckMultiplePresenceAndRemove(IsItem);
                }
                else if (IsItem.GetItem() as Bag)
                {
                    inventory.EquipBag(IsItem);
                }
            }
            if (buttonType == ButtonType.Weapon)
            {
                if (IsItem.GetItem() as Weapon)
                {
                    inventory.UnequipWeaponAt(equipAt);
                }
            }
            if (buttonType == ButtonType.Protection)
            {
                if (IsItem.GetItem() as Helmet)
                {
                    inventory.UnequipHelmet();
                }
                else if (IsItem.GetItem() as Vest)
                {
                    inventory.UnequipVest();
                }
                else if (IsItem.GetItem() as Bag)
                {
                    inventory.UnequipBag();
                }
            }
            if (buttonType == ButtonType.Ground)
            {
                PickUpFromGroundInventoryClick();
            }
        }

        private void PickUpFromGroundInventoryClick()
        {
            GameObject toAdd = IsItem.GetItem().gameObject;
            InventoryController control = inventory.parent.GetComponent<InventoryController>();
            PlayerController player = control.Player.GetComponent<PlayerController>();
            Inventory playerInventory = player.GetComponent<PlayerController>().GetInventory();
            if ((object)IsItem.GetItem() != null)
            {
                playerInventory.Add(toAdd, player.gameObject);
                if (toAdd.GetComponent<Item>() is Weapon)
                {
                    toAdd.transform.SetParent(player.GetWeaponHolderTransform());
                }
                else
                {
                    toAdd.transform.SetParent(player.GetInventoryHolderTransform());
                }

                toAdd.SetActive(false);
            }
            IsItem.RemoveOneFromCompteur();
            control.CreateCellsForNearbyItem();
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

        private void ChangeWeaponSlotFromKeyPressed()
        {
            if (Input.GetKeyDown(keyWeaponSlot))
            {
                if (equipAt != EquipWeaponAt.Secondary)
                {
                    equipAt = EquipWeaponAt.Secondary;
                }
            }
            if (Input.GetKeyUp(keyWeaponSlot))
            {
                if (equipAt != EquipWeaponAt.Primary)
                {
                    equipAt = EquipWeaponAt.Primary;
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
    }
}
