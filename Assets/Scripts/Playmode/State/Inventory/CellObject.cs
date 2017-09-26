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

        private void InjectCellObject([EntityScope] Text textName)
        {
            this.TextName = textName;
            //this.ImageBackground = imageBackground;
        }

        private void Awake()
        {
            InjectDependencies("InjectCellObject");
        }

        private void Start ()
        {
            SetTextNumber();
            SetTextName();
            SetImageBackground();
        }
	 
	    private void Update ()
        {
		
	    }

        private void SetImageBackground()
        {
            
        }

        private void SetTextName()
        {
            TextName.text = "Allo Test 01";
        }

        private void SetTextNumber()
        {

        }


    }
}
