using UnityEngine;
using UnityEngine.EventSystems;
using Harmony;

namespace ProjetSynthese
{
    public class DragHandler : GameScript
    {
        private static GameObject itemToDrag;
        private Vector3 startPosition;
        private GameObject oldParent;

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemToDrag = transform.parent.gameObject;
            oldParent = itemToDrag.transform.parent.gameObject;
            startPosition = transform.parent.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.parent.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            itemToDrag = null;
            transform.parent.position = startPosition;
        }

        public void Drag()
        {
            transform.parent.position = Input.mousePosition;
        }
    }
}