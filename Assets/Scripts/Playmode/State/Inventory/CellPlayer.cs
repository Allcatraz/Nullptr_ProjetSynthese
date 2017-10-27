using UnityEngine;


namespace ProjetSynthese
{
    public class CellPlayer : ObjectContainedInventory
    {
        private Item item;

        public CellPlayer()
        {

        }

        public override void SetItem(GameObject game)
        {
            item = game.GetComponent<Item>();
        }

        public override Item GetItem()
        {
            return item;
        }

    }
}
