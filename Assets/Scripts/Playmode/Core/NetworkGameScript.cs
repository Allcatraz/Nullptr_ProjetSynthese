using Harmony;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;

namespace ProjetSynthese
{
    /// <summary>
    /// Un NetworkGameScript est comme un <see cref="GameScript"/>, à la différence qu'il peut servir pour des jeu en réseau.
    /// </summary>
    public abstract class NetworkGameScript : NetworkScript
    {
        //Serialize field obligatoire à cause du networking
        [Tooltip("Le prefab utilisé pour instancié les bullets pour le joueur ou l'AI")]
        [SerializeField]
        private GameObject bulletPrefab;

        [Tooltip("Prefab de l'objet représentant l'inventaire dans le monde du joueur et de l'AI")]
        [SerializeField]
        private GameObject cratePrefab;

        [Tooltip("Le prefab utilisé pour instancié les grenades")]
        [SerializeField]
        private GameObject grenadePrefab;

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
        protected void CmdSpawnGrenade()
        {
        }


        //Obliger de faire les spawns de balles comme cela si l'on veut que le network les spawns correctememt
        [Command]
        protected void CmdSpawnBullet(Vector3 spawnPointPosition, Quaternion rotation, Vector3 chamberPosition, float bulletSpeed, float livingTime, int dommage, NetworkIdentity networkIdentity)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = spawnPointPosition;
            bullet.transform.rotation = rotation;

            Vector3 direction = Vector3.Normalize(spawnPointPosition - chamberPosition);
            Vector3 velocity = direction * bulletSpeed;
            bullet.GetComponent<Rigidbody>().velocity = velocity;

            NetworkServer.Spawn(bullet);

            RpcSetDommage(bullet, dommage);
            RpcChangeShooter(bullet, networkIdentity);
            Destroy(bullet, livingTime);
        }

        //Obliger de faire les spawns de crates comme cela si l'on veut que le network les spawns correctememt
        [Command]
        public void CmdSpawnGrenade(Vector3 spawnPointPosition, NetworkIdentity player)
        {
            GameObject grenade = Instantiate(grenadePrefab);
            grenade.transform.position = spawnPointPosition;
            NetworkServer.Spawn(grenade);
            player.GetComponent<PlayerController>().TargetFinishGrenadeThrow(player.connectionToClient, grenade.GetComponent<NetworkIdentity>());
        }
        protected void CmdSpawnCrate(Vector3 position)

        [Command]
        protected void CmdSpawnCrate()
        {
            GameObject crate = Instantiate(cratePrefab);
            crate.transform.position = position;
            NetworkServer.Spawn(crate);
        }

        //Obliger de faire le spawn des items que l'on drop dans le player si l'on veut que le network de Unity puissent les spawners correctememt.
        [Command]
        protected void CmdSpawnItemDrop(NetworkIdentity identity, Vector3 position)
        {
            Item itemToSpawn = identity.GetComponent<Item>();           
            if (itemToSpawn != null)
            {
                GameObject newItem = Instantiate(itemToSpawn.gameObject);

                Item spawner = newItem.GetComponent<Item>();
                if (spawner != null)
                {
                    if (itemToSpawn as AmmoPack && spawner as AmmoPack)
                    {
                        (spawner as AmmoPack).AmmoType = (itemToSpawn as AmmoPack).AmmoType;
                    }
                }

                newItem.GetComponent<Item>().Level = itemToSpawn.Level;

                newItem.gameObject.layer = LayerMask.NameToLayer(R.S.Layer.Item);
                List<GameObject> allItems = newItem.gameObject.GetAllChildrens().ToList();
                allItems.ForEach(obj => obj.layer = LayerMask.NameToLayer(R.S.Layer.Item));

                newItem.transform.position = position;
                newItem.SetActive(true);
                NetworkServer.Spawn(newItem);
            }
        }

        [Command]
        public void CmdDestroy(GameObject item)
        {
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

        [ClientRpc]
        private void RpcChangeShooter(GameObject bullet, NetworkIdentity networkIdentity)
        {
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.playerWhoShot = networkIdentity;
        }
    }
}
