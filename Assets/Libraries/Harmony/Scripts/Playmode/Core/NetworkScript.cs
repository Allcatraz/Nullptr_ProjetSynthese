using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Networking;

//We do not need to override "Equals" or "Hashcode" here : we would just be calling the "base" method.
//Warning is thus disabled in this file.
#pragma warning disable 660, 661

namespace Harmony
{
    /// <summary>
    /// Représente un Script Unity. Extension des <i>NetworkBehaviour</i>. Ajoute de nombreuses fonctionalitées en 
    /// plus de régler certains bogues.
    /// </summary>
    /// <remarks>
    /// La classe Script ajoute plusieurs moyens d'obtenir des <i>Components</i> ou des <i>GameObjects</i>. Par exemple, 
    /// il est désormais possible d'obtenir tous les enfants d'un <i>GameObject</i>. Consultez les différentes méthodes pour les détails.
    /// </remarks>
    public class NetworkScript : NetworkBehaviour, IScript
    {
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public new T[] GetComponents<T>() where T : class
        {
            return base.GetComponents<T>();
        }

        public new T[] GetComponentsInChildren<T>() where T : class
        {
            return base.GetComponentsInChildren<T>();
        }

        public virtual T GetComponentInTopParent<T>() where T : class
        {
            return gameObject.GetComponentInTopParent<T>();
        }

        public virtual Component GetComponentInTopParent([NotNull] Type type)
        {
            return gameObject.GetComponentInTopParent(type);
        }

        public virtual T[] GetComponentsInTopParent<T>() where T : class
        {
            return gameObject.GetComponentsInTopParent<T>();
        }

        public virtual Component[] GetComponentsInTopParent([NotNull] Type type)
        {
            return gameObject.GetComponentsInTopParent(type);
        }

        public virtual T GetComponentInChildrensParentsOrSiblings<T>() where T : class
        {
            return gameObject.GetComponentInChildrensParentsOrSiblings<T>();
        }

        public virtual Component GetComponentInChildrensParentsOrSiblings(Type type)
        {
            return gameObject.GetComponentInChildrensParentsOrSiblings(type);
        }

        public virtual T[] GetComponentsInChildrensParentsOrSiblings<T>() where T : class
        {
            return gameObject.GetComponentsInChildrensParentsOrSiblings<T>();
        }

        public virtual Component[] GetComponentsInChildrensParentsOrSiblings([NotNull] Type type)
        {
            return gameObject.GetComponentsInChildrensParentsOrSiblings(type);
        }

        public virtual GameObject GetTopParent()
        {
            return gameObject.GetTopParent();
        }

        public virtual IList<GameObject> GetAllChildrens()
        {
            return gameObject.GetAllChildrens();
        }

        public virtual IList<GameObject> GetAllHierachy()
        {
            return gameObject.GetAllHierachy();
        }

        //#Dirty Hack : Unity overrides the "==" operator on Components. For Unity, if the Component was destroyed or haven't been activated
        //in his lifespan, it's considered equal to "null". This strange behaviour can cause strange errors.
        //Here, we simply revert what they have done.
        public static bool operator ==(NetworkScript a, NetworkScript b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(NetworkScript a, NetworkScript b)
        {
            return !(a == b);
        }
    }
}