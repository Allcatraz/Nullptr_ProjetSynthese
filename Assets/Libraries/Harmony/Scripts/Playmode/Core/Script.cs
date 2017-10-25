using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//We do not need to override "Equals" or "Hashcode" here : we would just be calling the "base" method.
//Warning is thus disabled in this file.
#pragma warning disable 660, 661

namespace Harmony
{
    /// <summary>
    /// Représente un Script Unity. Extension des <i>MonoBehaviour</i>. Ajoute de nombreuses fonctionalitées en 
    /// plus de régler certains bogues.
    /// </summary>
    /// <remarks>
    /// La classe Script ajoute plusieurs moyens d'obtenir des <i>Components</i> ou des <i>GameObjects</i>. Par exemple, 
    /// il est désormais possible d'obtenir tous les enfants d'un <i>GameObject</i>. Consultez les différentes méthodes pour les détails.
    /// </remarks>
    public abstract class Script : MonoBehaviour, IScript
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

        public T GetComponentInRoot<T>() where T : class
        {
            return gameObject.GetComponentInRoot<T>();
        }

        public Component GetComponentInRoot(Type type)
        {
            return gameObject.GetComponentInRoot(type);
        }

        public T[] GetComponentsInRoot<T>() where T : class
        {
            return gameObject.GetComponentsInRoot<T>();
        }

        public Component[] GetComponentsInRoot(Type type)
        {
            return gameObject.GetComponentsInRoot(type);
        }

        public new T[] GetComponentsInParent<T>() where T : class
        {
            return gameObject.GetComponentsInParent<T>();
        }

        public T GetComponentInSiblings<T>() where T : class
        {
            return gameObject.GetComponentInSiblings<T>();
        }

        public Component GetComponentInSiblings(Type type)
        {
            return gameObject.GetComponentInSiblings(type);
        }

        public T[] GetComponentsInSiblings<T>() where T : class
        {
            return gameObject.GetComponentsInSiblings<T>();
        }

        public Component[] GetComponentsInSiblings(Type type)
        {
            return gameObject.GetComponentsInSiblings(type);
        }

        public new T[] GetComponentsInChildren<T>() where T : class
        {
            return base.GetComponentsInChildren<T>();
        }

        public T GetComponentInChildrensParentsOrSiblings<T>() where T : class
        {
            return gameObject.GetComponentInChildrensParentsOrSiblings<T>();
        }

        public Component GetComponentInChildrensParentsOrSiblings(Type type)
        {
            return gameObject.GetComponentInChildrensParentsOrSiblings(type);
        }

        public T[] GetComponentsInChildrensParentsOrSiblings<T>() where T : class
        {
            return gameObject.GetComponentsInChildrensParentsOrSiblings<T>();
        }

        public Component[] GetComponentsInChildrensParentsOrSiblings([NotNull] Type type)
        {
            return gameObject.GetComponentsInChildrensParentsOrSiblings(type);
        }

        public GameObject GetRoot()
        {
            return gameObject.GetRoot();
        }

        public IList<GameObject> GetAllHierachy()
        {
            return gameObject.GetAllHierachy();
        }

        public IList<GameObject> GetAllParents()
        {
            return gameObject.GetAllParents();
        }

        public IList<GameObject> GetAllSiblings()
        {
            return gameObject.GetAllSiblings();
        }

        public IList<GameObject> GetAllChildrens()
        {
            return gameObject.GetAllChildrens();
        }

        //#Dirty Hack : Unity overrides the "==" operator on Components. For Unity, if the Component was destroyed or haven't been activated
        //in his lifespan, it's considered equal to "null". This strange behaviour can cause strange errors.
        //Here, we simply revert what they have done.
        public static bool operator ==(Script a, Script b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Script a, Script b)
        {
            return !(a == b);
        }
    }
}