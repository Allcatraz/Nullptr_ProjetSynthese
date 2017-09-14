﻿using System;
using System.Collections.Generic;
using Harmony.Util;
using Harmony.Testing;
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
    /// La classe UnityScript ajoute plusieurs moyens d'obtenir des <i>Components</i> ou des <i>GameObjects</i>. Par exemple, 
    /// il est désormais possible d'obtenir tous les enfants d'un <i>GameObject</i>. Consultez les différentes méthodes pour les détails.
    /// </remarks>
    [NotTested(Reason.Wrapper)]
    public abstract class UnityScript : MonoBehaviour, IScript
    {
        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/> et 
        /// retourne le premier trouvé.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public virtual T GetComponentInTopParent<T>() where T : class
        {
            return gameObject.GetComponentInTopParent<T>();
        }

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/> et 
        /// retourne le premier trouvé.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public virtual Component GetComponentInTopParent([NotNull] Type type)
        {
            return gameObject.GetComponentInTopParent(type);
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public virtual T[] GetComponentsInTopParent<T>() where T : class
        {
            return gameObject.GetComponentsInTopParent<T>();
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public virtual Component[] GetComponentsInTopParent([NotNull] Type type)
        {
            return gameObject.GetComponentsInTopParent(type);
        }

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/>,
        /// tous ses enfants, et retourne le premier trouvé.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public virtual T GetComponentInChildrensParentsOrSiblings<T>() where T : class
        {
            return gameObject.GetComponentInChildrensParentsOrSiblings<T>();
        }

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/>,
        /// tous ses enfants, et retourne le premier trouvé.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public virtual Component GetComponentInChildrensParentsOrSiblings(Type type)
        {
            return gameObject.GetComponentInChildrensParentsOrSiblings(type);
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> 
        /// du <see cref="GameObject"/>, tous ses enfants, et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public virtual T[] GetComponentsInChildrensParentsOrSiblings<T>() where T : class
        {
            return gameObject.GetComponentsInChildrensParentsOrSiblings<T>();
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> 
        /// du <see cref="GameObject"/>, tous ses enfants, et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public virtual Component[] GetComponentsInChildrensParentsOrSiblings([NotNull] Type type)
        {
            return gameObject.GetComponentsInChildrensParentsOrSiblings(type);
        }

        /// <summary>
        /// Retourne le <i>TopParent</i> du <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// <i>TopParent</i> du <see cref="GameObject"/>. Si le <see cref="GameObject"/> ne possède pas de parent, 
        /// c'est lui même qui est retourné par cette méthode.
        /// </returns>
        [NotNull]
        public virtual GameObject GetTopParent()
        {
            return gameObject.GetTopParent();
        }

        /// <summary>
        /// Retourne tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// Tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>, récursivement, sans inclure 
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        [NotNull]
        public virtual IList<GameObject> GetAllChildrens()
        {
            return gameObject.GetAllChildrens();
        }

        /// <summary>
        /// Retourne la hirachie complète de ce <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// Tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>, récursivement, en incluant
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        [NotNull]
        public virtual IList<GameObject> GetAllHierachy()
        {
            return gameObject.GetAllHierachy();
        }

        //#Dirty Hack : Unity overrides the "==" operator on Components. For Unity, if the Component was destroyed or haven't been activated
        //in his lifespan, it's considered equal to "null". This strange behaviour can cause errors while in Unit Tests.
        //Here, we simply revert what they have done.
        public static bool operator ==(UnityScript a, UnityScript b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(UnityScript a, UnityScript b)
        {
            return !(a == b);
        }
    }
}