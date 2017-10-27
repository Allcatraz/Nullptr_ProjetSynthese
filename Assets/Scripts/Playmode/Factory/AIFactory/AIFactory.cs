using UnityEngine;
using UnityEngine.Networking;


namespace ProjetSynthese
{
    public class AIFactory
    {
        //BEN_REVIEW : Saleté de nommage Unity à la ***
        //             L'annotation, c'était pas suffisant ?!!?
        //
        //             Je vous blâme pas, je blâme les mauvaises décisions de Unity....
        [Command]
        public static GameObject CmdSpawnAI( GameObject prefab)
        {
            //BEN_REVIEW : _underscoreCasing ? En C# ? C'est nouveau ça...
            
            //BEN_CORRECTION : Voir AISpawnerController.
            //                 Instanciate a plusieurs surcharge. L'une d'entre elles permet d'indiquer
            //                 où sera créé l'objet (et même sa rotation).
            GameObject _object = Object.Instantiate(prefab);
            
            NetworkServer.Spawn(_object);

            return _object;
        }
    }
}


