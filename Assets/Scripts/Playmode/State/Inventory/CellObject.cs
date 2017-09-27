using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/State/Inventory/CellObject")]
    public class CellObject : GameScript
    {
        public Item IsItem { get; set; }

        public Image ImageBackground { get; private set; }

        public Text TextName { get; private set; }

        public Text TextNumber { get; private set; }

        public void InstantiateFromCell(Cell cell)
        {
            string name = cell.GetItem().Type.ToString();
            int compteur = cell.GetCompteur();
            SetTextName(name);
            SetTextNumber(compteur);
            SetImageBackground();
        }

        private void InjectCellObject([EntityScope] Text textName)
        {
            this.TextName = textName;
            //this.ImageBackground = imageBackground;
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
