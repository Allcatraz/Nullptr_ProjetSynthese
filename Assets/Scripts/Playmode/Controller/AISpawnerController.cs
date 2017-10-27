using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Ah.....tout à coup, plein de contrôleurs...je vous l'avais dit...
    
    public class AISpawnerController : MonoBehaviour
    {
        //BEN_CORRECTION : camelCase partout...ici, PascalCase...
        [Tooltip("AI prefab contenant les combatants AI")]
        [SerializeField]
        private GameObject AIprefab;

        //BEN_CORRECTION : Un "const" ne peut pas être "Serialized".
        //                 Voir     "How to ensure a field in a script is serialized" 
        //                 à         https://docs.unity3d.com/Manual/script-Serialization.html
        [Tooltip("Nombre de combatants AI")]
        [SerializeField]
        const int AINumber = 1;

        //BEN_REVIEW : Est-ce vraiment impossible de créer un SerializedField pour ça ?
        //BEN_CORRECTION : Aussi, pas private.
        const float XMapCornerCoordinate = 0.0f;
        const float ZMapCornerCoordinate = 0.0f;
        const float DefaultHeighPosition = 1.0f;

        const float XOffset = 15.0f;
        const float ZOffset = -15.0f;

        //BEN_CORRECTION : Pas private.
        void Start()
        {
            SpawnAIs();
        }

        private void SpawnAIs()
        {
            Vector3[] position = new Vector3[AINumber];
            for (int i = 0; i < AINumber; i++)
            {
                //BEN_REVIEW : Pourquoi est-ce que le Vector3 n'est pas constant ?
                position[i] = new Vector3(XMapCornerCoordinate + XOffset, DefaultHeighPosition, ZMapCornerCoordinate + ZOffset);
            }
    
            for (int i = 0; i < AINumber; i++)
            {
                //BEN_REVIEW : Oulà.....pas certain que ça fait ce que vous voulez tout le temps ça.
                //             Il faut voir les prefabs comme des moules "constants". Ici, tu modifie ton moule,
                //             ce qui veut dire que tous les autres prefabs créés par la suite seront impactés.
                AIprefab.GetComponent<NetworkStartPosition>().transform.position = position[i];
                
                //BEN_CORRECTION : Les fabriques ne connaissent-elles pas leurs prefabs de toute façon ?
                //                 Pourquoi est-ce que cette fabrique a besoin de se faire envoyer le prefab ?
                //                 Pourquoi ne lui envoie tu pas la position où tu veux que le prefab soit
                //                 créé pour qu'elle s'en occupe ?
                AIFactory.CmdSpawnAI(AIprefab);
            }
        }
    }
}