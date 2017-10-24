using UnityEngine;

namespace ProjetSynthese
{
    public class PanelColliderHandler : GameScript
    {
        [Tooltip("Le type de bouton contenue dans la grille du panel. Utilisé pour change la valeur DroppedAt lors du drag and drop")]
        [SerializeField] private CellObjectType panelType;

        void OnTriggerEnter2D(Collider2D col)
        {
            CellObject temp = col.gameObject.GetComponentInChildren<CellObject>();
            if (temp != null)
            {
                temp.DropAtType = panelType;
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            CellObject temp = col.gameObject.GetComponentInChildren<CellObject>();
            if (temp != null && temp.DropAtType == panelType)
            {
                temp.DropAtType = temp.cellObjectType;
            }
        }
        
    }
}

