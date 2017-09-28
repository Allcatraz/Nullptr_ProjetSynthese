using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/State/Inventory/CellObject")]
    public class CellObject : GameScript
    {
        private Button button;

        public Inventory inventory { get; set; }

        public Cell IsItem { get; set; }

        public Image ImageBackground { get; private set; }

        public Text TextName { get; private set; }

        public Text TextNumber { get; private set; }

        public void InstantiateFromCell(Cell cell)
        {
            string name = cell.GetItem().Type.ToString();
            this.IsItem = cell;
            int compteur = cell.GetCompteur();
            SetTextName(name);
            SetTextNumber(compteur);
            SetImageBackground();
        }

        private void InjectCellObject([EntityScope] Text textName,
                                    [EntityScope] Button button)
        {
            this.button = button;
            this.TextName = textName;
            button.onClick.AddListener(TaskOnClick);
            //this.ImageBackground = imageBackground;
        }

        private void TaskOnClick()
        {
            if (IsItem.GetItem() as Weapon)
            {
                inventory.EquipWeaponAt(EquipWeaponAt.Primary, IsItem);
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

        private void Awake()
        {
            InjectDependencies("InjectCellObject");
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
