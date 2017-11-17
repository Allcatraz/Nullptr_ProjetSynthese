using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Harmony;

namespace ProjetSynthese
{
    public class Grenade : Throwable
    {
        [SerializeField]
        [Tooltip("Le prefab de l'explosion lorsque la grenade explose")]
        private GameObject explosionPrefab;

        [SerializeField]
        [Tooltip("Le temps avant que la grenade explose après avoir été lancée")]
        private float explosionTime;

        private const int Weight = 6;

        public override int GetWeight()
        {
            return Weight;
        }

        public override void Throw(NetworkIdentity identity, float force)
        {
            CmdChangeLayerForAllChildrens(gameObject, R.S.Layer.FreeItem);
            CmdSetActive(gameObject, true);
            CmdChangeGrenadeParentAndResetPositioIfParentNotNull(gameObject, identity);
            this.force = force;
            Destroy(gameObject, explosionTime);
        }

        public override void Release()
        {
            Matrix4x4 rootTransform = transform.root.transform.localToWorldMatrix;
            CmdChangeGrenadeParentAndResetPositioIfParentNotNull(gameObject, null);
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.velocity = rootTransform.GetColumn(2) * force;
            rb.drag = 0.5f;
        }

        private void OnDestroy()
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }


        [Command]
        private void CmdChangeGrenadeParentAndResetPositioIfParentNotNull(GameObject grenade, NetworkIdentity identity)
        {
            RpcChangeGrenadeParentAndResetPositionIfParentNotNull(grenade, identity);
        }

        [ClientRpc]
        private void RpcChangeGrenadeParentAndResetPositionIfParentNotNull(GameObject grenade, NetworkIdentity identity)
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