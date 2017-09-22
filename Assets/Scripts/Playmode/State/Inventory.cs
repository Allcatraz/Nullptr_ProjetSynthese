using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public enum InventoryOf {Player, Item}

    [AddComponentMenu("Game/State/Health")]
    public class Inventory : GameScript
    {

        [SerializeField]
        private InventoryOf inventoryOf;

        private List<GameObject> listInventory;

	    // Use this for initialization
	    void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        public void Add(GameObject game)
        {
            listInventory.Add(game);
        }

        public void Remove(GameObject game)
        {
            listInventory.Remove(game);
        }
    }
}

