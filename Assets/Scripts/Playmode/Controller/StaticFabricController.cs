using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetSynthese
{
    public class StaticFabricController : MonoBehaviour
    {
        [SerializeField]
        GameObject bulletPrefab;


        // Use this for initialization
        void Start()
        {
            BulletFabric.BulletPrefab = bulletPrefab;
        }
    }
}


