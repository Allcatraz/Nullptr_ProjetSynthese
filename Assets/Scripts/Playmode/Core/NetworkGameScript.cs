using Harmony;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace ProjetSynthese
{
    /// <summary>
    /// Un NetworkGameScript est comme un <see cref="GameScript"/>, à la différence qu'il peut servir pour des jeu en réseau.
    /// </summary>
    public abstract class NetworkGameScript : NetworkScript
    {
        /// <summary>
        /// Injecte les dépendances de ce NetworkGameScript.
        /// </summary>
        /// <param name="injectMethodName">
        /// Nom de la méthode où l'injection doit être effectuée.
        /// </param>
        protected void InjectDependencies(string injectMethodName)
        {
            ApplicationConfiguration.InjectDependencies(this, injectMethodName);
        }

        //////////////////////////////
        /// COMMAND CALL ON SERVER ///
        //////////////////////////////

        [Command]
        public void CmdSpawnObject(GameObject item)
        {
            NetworkServer.Spawn(item);
        }

        [Command]
        public void CmdDestroy(GameObject item)
        {
            NetworkServer.Destroy(item);
        }

        [Command]
        public void CmdSetTransform(GameObject item, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            RpcSetTransform(item, position, rotation, scale);
        }

        [Command]
        public void CmdChangeTransformPosition(GameObject gameObject, Vector3 position)
        {
            RpcChangeTransformPosition(gameObject, position);
        }

        [Command]
        public void CmdSetActive(GameObject item, bool isActive)
        {
            RpcSetActive(item, isActive);
        }

        public void CmdChangeLayerForAllChildrens(GameObject root, string layer)
        {
            RpcChangeLayerForAllChildrens(root, layer);
        }

        //////////////////////////////////
        /// COMMAND CALL ON ALL CLIENT ///
        //////////////////////////////////

        [ClientRpc]
        protected void RpcSetTransform(GameObject item, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            item.transform.position = position;
            item.transform.rotation = rotation;
            item.transform.localScale = scale;
        }

        [ClientRpc]
        protected void RpcSetActive(GameObject item, bool isActive)
        {
            item.SetActive(isActive);
        }

        [ClientRpc]
        private void RpcChangeTransformPosition(GameObject gameObject, Vector3 position)
        {
            gameObject.transform.position = position;
        }

        [ClientRpc]
        private void RpcChangeLayerForAllChildrens(GameObject root, string layer)
        {
            int layerMask = LayerMask.NameToLayer(layer);
            root.layer = layerMask;
            IList<GameObject> allChildrens = root.GetAllChildrens();
            for (int i = 0; i < allChildrens.Count; i++)
            {
                allChildrens[i].layer = layerMask;
            }
        }
    }
}
