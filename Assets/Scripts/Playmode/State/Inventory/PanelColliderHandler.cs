using UnityEngine;

namespace ProjetSynthese
{
    public class PanelColliderHandler : GameScript
    {

        [SerializeField] private ButtonType panelType;

        void OnTriggerEnter2D(Collider2D col)
        {
            CellObject temp = col.gameObject.GetComponentInChildren<CellObject>();
            if (temp != null)
            {
                temp.dropAtType = panelType;
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            CellObject temp = col.gameObject.GetComponentInChildren<CellObject>();
            if (temp != null && temp.dropAtType == panelType)
            {
                temp.dropAtType = temp.buttonType;
            }
        }
        
    }
}

