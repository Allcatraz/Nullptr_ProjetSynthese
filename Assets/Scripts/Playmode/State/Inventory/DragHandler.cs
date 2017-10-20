using UnityEngine;
using UnityEngine.EventSystems;
using Harmony;

namespace ProjetSynthese
{
    public class DragHandler : GameScript
    {
        [SerializeField] private GameObject itemToDrag;
        private GameObject canvasMenu;
        private Vector3 startPosition;
        private GameObject oldParent;

        private void Awake()
        {
            InjectDependencies("InjectController");
        }

        private void InjectController([SceneScope] InventoryController inventoryController)
        {
            canvasMenu = inventoryController.gameObject;
        }

        public void OnBeginDrag()
        {
            oldParent = itemToDrag.transform.parent.gameObject;
            startPosition = itemToDrag.transform.position;
            itemToDrag.transform.parent = canvasMenu.transform;
        }

        public void OnEndDrag()
        {
            itemToDrag = null;
            itemToDrag.transform.position = startPosition;
        }

        public void Drag()
        {

            itemToDrag.transform.position = Input.mousePosition;

        }
    }
}