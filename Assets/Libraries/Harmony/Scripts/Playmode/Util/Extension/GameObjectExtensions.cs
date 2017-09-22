using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les GameObjects.
    /// </summary>
    public static class GameObjectExtensions 
    {
        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/> et 
        /// retourne le premier trouvé.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir le <see cref="Component"/> dans le <i>TopParent</i>.</param>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public static T GetComponentInTopParent<T>(this GameObject gameObject) where T : class
        {
            return gameObject.GetTopParent().GetComponent<T>();
        }

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/> et 
        /// retourne le premier trouvé.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir le <see cref="Component"/> dans le <i>TopParent</i>.</param>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public static Component GetComponentInTopParent(this GameObject gameObject, Type type)
        {
            return gameObject.GetTopParent().GetComponent(type);
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir les <see cref="Component">Components</see> dans le <i>TopParent</i>.</param>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public static T[] GetComponentsInTopParent<T>(this GameObject gameObject) where T : class
        {
            return gameObject.GetTopParent().GetComponents<T>();
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir les <see cref="Component">Components</see> dans le <i>TopParent</i>.</param>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public static Component[] GetComponentsInTopParent(this GameObject gameObject, [NotNull] Type type)
        {
            return gameObject.GetTopParent().GetComponents(type);
        }

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/>,
        /// tous ses enfants, et retourne le premier trouvé.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir le <see cref="Component"/>.</param>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public static T GetComponentInChildrensParentsOrSiblings<T>(this GameObject gameObject) where T : class
        {
            return gameObject.GetTopParent().GetComponentInChildren<T>();
        }

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/>,
        /// tous ses enfants, et retourne le premier trouvé.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir le <see cref="Component"/>.</param>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        public static Component GetComponentInChildrensParentsOrSiblings(this GameObject gameObject, Type type)
        {
            return gameObject.GetTopParent().GetComponentInChildren(type);
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> 
        /// du <see cref="GameObject"/>, tous ses enfants, et les retourne tous.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir les <see cref="Component">Components</see>.</param>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public static T[] GetComponentsInChildrensParentsOrSiblings<T>(this GameObject gameObject) where T : class
        {
            return gameObject.GetTopParent().GetComponentsInChildren<T>();
        }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> 
        /// du <see cref="GameObject"/>, tous ses enfants, et les retourne tous.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir les <see cref="Component">Components</see>.</param>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        [NotNull]
        public static Component[] GetComponentsInChildrensParentsOrSiblings(this GameObject gameObject, [NotNull] Type type)
        {
            return gameObject.GetTopParent().GetComponentsInChildren(type);
        }

        /// <summary>
        /// Retourne le <i>TopParent</i> du <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir le <i>TopParent</i>.</param>
        /// <returns>
        /// <i>TopParent</i> du <see cref="GameObject"/>. Si le <see cref="GameObject"/> ne possède pas de parent, 
        /// c'est lui même qui est retourné par cette méthode.
        /// </returns>
        [NotNull]
        public static GameObject GetTopParent(this GameObject gameObject)
        {
            Transform parent = gameObject.transform;
            while (parent.parent != null)
            {
                parent = parent.parent;
            }
            return parent.gameObject;
        }

        /// <summary>
        /// Retourne tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir les enfants.</param>
        /// <returns>
        /// Tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>, récursivement, sans inclure 
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        [NotNull]
        public static IList<GameObject> GetAllChildrens(this GameObject gameObject)
        {
            IList<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject currentChildren = gameObject.transform.GetChild(i).gameObject;
                gameObjects.Add(currentChildren);

                foreach (GameObject childrenGameObject in currentChildren.GetAllChildrens())
                {
                    gameObjects.Add(childrenGameObject);
                }
            }
            return gameObjects;
        }

        /// <summary>
        /// Retourne la hirachie complète de ce <see cref="GameObject"/>.
        /// </summary>
        /// <param name="gameObject">GameObject où obtenir les enfants ainsi que lui même.</param>
        /// <returns>
        /// Tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>, récursivement, en incluant
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        [NotNull]
        public static IList<GameObject> GetAllHierachy(this GameObject gameObject)
        {
            IList<GameObject> gameObjects = gameObject.GetAllChildrens();
            gameObjects.Insert(0, gameObject.gameObject); //Parent will allways be first
            return gameObjects;
        }

        /// <summary>
        /// Détruit le GameObject (ainsi que tous ses enfants et ses composants).
        /// </summary>
        /// <param name="gameObject">GameObject à détruire.</param>
        public static void Destroy(this GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}