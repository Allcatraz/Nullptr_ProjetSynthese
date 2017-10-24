using UnityEngine;

namespace ProjetSynthese
{
    public class ItemContainedInventory : ObjectContainedInventory
    {
        private Item item;

        public ItemContainedInventory()
        {

        }

        public ItemContainedInventory(Item item)
        {
            this.item = item;
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