using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public abstract class Throwable : Item
    {
        protected float force;

        public abstract void Throw(NetworkIdentity identity, float force);
        public abstract void Release();
    }
}


