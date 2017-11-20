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
        [Tooltip("Le prefab utilisé pour instancié les bullets")]
        [SerializeField]
        private GameObject bulletPrefab;

        [Tooltip("Prefab de l'objet représentant l'inventaire dans le monde")]
        [SerializeField]
        private GameObject cratePrefab;

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
        protected void CmdSpawnBullet(Vector3 spawnPointPosition, Quaternion rotation, Vector3 chamberPosition, float bulletSpeed, float livingTime, int dommage)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = spawnPointPosition;
            bullet.transform.rotation = rotation;

            Vector3 direction = Vector3.Normalize(spawnPointPosition - chamberPosition);
            Vector3 velocity = direction * bulletSpeed;
            bullet.GetComponent<Rigidbody>().velocity = velocity;

            NetworkServer.Spawn(bullet);

            RpcSetDommage(bullet, dommage);
            Destroy(bullet, livingTime);
        }

        [Command]
        protected void CmdSpawnCrate()
        {
            GameObject crate = Instantiate(cratePrefab);
            NetworkServer.Spawn(crate);
        }

        [Command]
        public void CmdDestroy(GameObject item)        {
            RpcDestroy(item);
        }

        [Command]
        protected void CmdDestroyTime(GameObject item, float time)
        {
            Destroy(item, time);
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

        [Command]
        protected void CmdSetDommage(GameObject item, int dommage)
        {
            RpcSetDommage(item, dommage);
        }

		[Command]
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
        protected void RpcSetDommage(GameObject item, int dommage)
        {
            item.GetComponent<BulletController>().Dommage = dommage;
        }

        [ClientRpc]
        protected void RpcDestroy(GameObject item)
        {
            Destroy(item);
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
