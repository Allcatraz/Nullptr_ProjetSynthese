using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ProjetSynthese
{
    public enum ButtonType {Weapon,Protection,Inventory}

    public class CellObject : GameScript
    {
        [SerializeField] private ButtonType buttonType;
        [SerializeField] private KeyCode key;
        private Button button;
        public EquipWeaponAt equipAt { get; set; }


        public Inventory inventory { get; set; }

        public Cell IsItem { get; set; }

        public Image ImageBackground { get; private set; }

        public Text TextName { get; private set; }

        public Text TextNumber { get; private set; }

        public void InstantiateFromCell(Cell cell)
        {   
            string name = cell.GetItem().Type.ToString();
            IsItem = cell;
            int compteur = cell.GetCompteur();
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
                if (IsItem.GetItem() as Weapon)
                {
                    inventory.EquipWeaponAt(equipAt, IsItem);
                }
                if (IsItem.GetItem() as Helmet)
                {
                    inventory.EquipHelmet(IsItem);
                }
                if (IsItem.GetItem() as Vest)
                {
                    inventory.EquipVest(IsItem);
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
                if (IsItem.GetItem() as Vest)
                {
                    inventory.UnequipVest();
                }
            }
            
        }

        private void Awake()
        {
            InjectDependencies("InjectCellObject");
        }

        private void Update()
        {
            ChangeWeaponSlotFromKeyPressed();
        }

        private void ChangeWeaponSlotFromKeyPressed()
        {
            if (Input.GetKeyDown(key))
            {
                if (equipAt != EquipWeaponAt.Secondary)
                {
                    equipAt = EquipWeaponAt.Secondary;
                }
            }
            if (Input.GetKeyUp(key))
            {
                if (equipAt != EquipWeaponAt.Primary)
                {
                    equipAt = EquipWeaponAt.Primary;
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
