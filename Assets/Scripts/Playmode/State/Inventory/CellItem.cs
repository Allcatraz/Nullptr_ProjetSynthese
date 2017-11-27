using UnityEngine;

namespace ProjetSynthese
{
    public class ItemContainedInventory : ObjectContainedInventory
    {
        private Item item;

        //BEN_CORRECTION : Ces deux constructeurs sont inutiles. Le constructeur
        //                 avec paramêtre est jamais utilisé et le constructeur par défaut
        //                 ne fait rien.
        
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