using Harmony;
using UnityEngine;
using UnityEngine.Networking;

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
        protected void CmdSpawnObject(GameObject item)
        {
            NetworkServer.Spawn(item);
        }

        [Command]
        protected void CmdDestroy(GameObject item)
        {
            Destroy(item);
        }

        [Command]
        protected void CmdDestroyTime(GameObject item, float time)
        {
            Destroy(item, time);
        }

        [Command]
        protected void CmdSetTransform(GameObject item, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            RpcSetTransform(item, position, rotation, scale);
        }

        [Command]
        protected void CmdChangeParent(GameObject item, GameObject parent)
        {
            RpcChangeParent(item, parent);
        }

        [Command]
        protected void CmdSetActive(GameObject item, bool isActive)
        {
            RpcSetActive(item, isActive);
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
        protected void RpcChangeParent(GameObject item, GameObject parent)
        {
            item.transform.SetParent(parent.transform);
        }

        [ClientRpc]
        protected void RpcSetActive(GameObject item, bool isActive)
        {
            item.SetActive(isActive);
        }
    }
}