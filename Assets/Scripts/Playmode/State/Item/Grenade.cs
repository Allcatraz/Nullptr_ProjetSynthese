using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Harmony;

namespace ProjetSynthese
{
    public class Grenade : Throwable
    {
        private const int Weight = 6;
        private const float Drag = 0.5f;


        [SerializeField]
        [Tooltip("Le prefab de l'explosion lorsque la grenade explose")]
        private GameObject explosionPrefab;

        [SerializeField]
        [Tooltip("Le temps avant que la grenade explose après avoir été lancée")]
        private float explosionTime;

        private bool grenadeHasBeenThrowned = false;

        public override int GetWeight()
        {
            return Weight;
        }

        public override void Throw(NetworkIdentity identity, float force)
        {
            NetworkIdentity grenadeIdentity = GetComponent<NetworkIdentity>();
            CmdChangeLayerForAllChildrens(gameObject, R.S.Layer.FreeItem);
            CmdSetActive(gameObject, true);
            CmdChangeGrenadeParentAndResetPositioIfParentNotNull(grenadeIdentity, identity);
            CmdSetForce(grenadeIdentity, force);
            CmdDestroygrenade(grenadeIdentity, explosionTime);
            grenadeHasBeenThrowned = true;
        }

        public override void Release()
        {
            Matrix4x4 rootTransform = transform.root.transform.localToWorldMatrix;
            CmdChangeGrenadeParentAndResetPositioIfParentNotNull(GetComponent<NetworkIdentity>(), null);
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.velocity = rootTransform.GetColumn(2) * force;
            rb.drag = Drag;
        }

        private void OnDestroy()
        {
            if (grenadeHasBeenThrowned == true)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
        }

        [Command]
        private void CmdDestroygrenade(NetworkIdentity grenade, float destroyTime)
        {
            RpcDestroygrenade(grenade, destroyTime);
        }

        [ClientRpc]
        private void RpcDestroygrenade(NetworkIdentity grenade, float destroyTime)
        {
            Destroy(grenade.gameObject, destroyTime);
        }

        [Command]
        private void CmdSetForce(NetworkIdentity grenade, float force)
        {
            RpcSetForce(grenade, force);
        }

        [ClientRpc]
        private void RpcSetForce(NetworkIdentity grenade, float force)
        {
            grenade.GetComponent<Grenade>().force = force;
        }

        [Command]
        private void CmdChangeGrenadeParentAndResetPositioIfParentNotNull(NetworkIdentity grenade, NetworkIdentity identity)
        {
            RpcChangeGrenadeParentAndResetPositionIfParentNotNull(grenade, identity);
        }

        [ClientRpc]
        private void RpcChangeGrenadeParentAndResetPositionIfParentNotNull(NetworkIdentity grenade, NetworkIdentity identity)
        {
            if (identity != null)
            {
                grenade.transform.SetParent(identity.GetComponent<PlayerController>().GetGrenadeHolder().transform);
                grenade.transform.position = Vector3.zero;
                grenade.transform.localPosition = Vector3.zero;
            }
            else
            {
                grenade.transform.SetParent(null);
            }
        }
    }
}