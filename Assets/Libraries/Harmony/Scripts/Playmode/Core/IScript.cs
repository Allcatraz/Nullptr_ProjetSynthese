using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un Unity.
    /// </summary>
    /// <remarks>
    /// La classe Script ajoute plusieurs moyens d'obtenir des <i>Components</i> ou des <i>GameObjects</i>. Par exemple, 
    /// il est désormais possible d'obtenir tous les enfants d'un <i>GameObject</i>. Consultez les différentes méthodes pour les détails.
    /// </remarks>
    public interface IScript
    {
        /// <summary>
        /// Nom du GameObejct contenant le script.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// GameObject contenant le script.
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <see cref="GameObject"/> et les 
        /// retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        T[] GetComponents<T>() where T : class;

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <see cref="GameObject"/> et les 
        /// retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        Component[] GetComponents(Type type);

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/> et 
        /// retourne le premier trouvé.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        T GetComponentInRoot<T>() where T : class;

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/> et 
        /// retourne le premier trouvé.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        Component GetComponentInRoot([NotNull] Type type);

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        T[] GetComponentsInRoot<T>() where T : class;

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        Component[] GetComponentsInRoot([NotNull] Type type);

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans les <i>Parents</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        T[] GetComponentsInParent<T>() where T : class;

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans les <i>Parents</i> du 
        /// <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        Component[] GetComponentsInParent(Type type);

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le frères et soeurs  du <see cref="GameObject"/>,
        /// et retourne le premier trouvé.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        T GetComponentInSiblings<T>() where T : class;

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le frères et soeurs du <see cref="GameObject"/> 
        /// et retourne le premier trouvé.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        Component GetComponentInSiblings(Type type);

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans dans le frères et soeurs 
        /// du <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        T[] GetComponentsInSiblings<T>() where T : class;

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans dans le frères et soeurs  
        /// du <see cref="GameObject"/> et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        Component[] GetComponentsInSiblings(Type type);

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <see cref="GameObject"/>, ses enfants,
        /// et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        T[] GetComponentsInChildren<T>() where T : class;

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <see cref="GameObject"/>, ses enfants,
        /// et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        Component[] GetComponentsInChildren(Type type);

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/>,
        /// tous ses enfants, et retourne le premier trouvé.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        T GetComponentInChildrensParentsOrSiblings<T>() where T : class;

        /// <summary>
        /// Retourne le <see cref="Component"/> du type spécifié. Recherche dans le <i>TopParent</i> du <see cref="GameObject"/>,
        /// tous ses enfants, et retourne le premier trouvé.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Un <see cref="Component"/> du type demandé, ou null s'il en existe aucun.</returns>
        Component GetComponentInChildrensParentsOrSiblings(Type type);

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> 
        /// du <see cref="GameObject"/>, tous ses enfants, et les retourne tous.
        /// </summary>
        /// <typeparam name="T">Type du <see cref="Component"/> à obtenir.</typeparam>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        T[] GetComponentsInChildrensParentsOrSiblings<T>() where T : class;

        /// <summary>
        /// Retourne tous les <see cref="Component">Components</see> du type spécifié. Recherche dans le <i>TopParent</i> 
        /// du <see cref="GameObject"/>, tous ses enfants, et les retourne tous.
        /// </summary>
        /// <param name="type">Type du <see cref="Component"/> à obtenir.</param>
        /// <returns>Tableau contenant tous les <see cref="Component">Components</see> trouvés.</returns>
        Component[] GetComponentsInChildrensParentsOrSiblings([NotNull] Type type);

        /// <summary>
        /// Retourne le <i>Root</i> du <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// <i>Root</i> du <see cref="GameObject"/>. Si le <see cref="GameObject"/> ne possède pas de parent, 
        /// c'est lui même qui est retourné par cette méthode.
        /// </returns>
        [NotNull]
        GameObject GetRoot();

        /// <summary>
        /// Retourne la hirachie complète de ce <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// Tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>, récursivement, en incluant
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        IList<GameObject> GetAllHierachy();

        /// <summary>
        /// Retourne tous les <see cref="GameObject"/> parents de ce <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// Tous les <see cref="GameObject"/> parents de ce <see cref="GameObject"/>, récursivement, sans inclure 
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        IList<GameObject> GetAllParents();

        /// <summary>
        /// Retourne tous les frères et soeurs <see cref="GameObject"/> de ce <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// Tous les frères et soeurs <see cref="GameObject"/> de ce <see cref="GameObject"/>.
        /// </returns>
        IList<GameObject> GetAllSiblings();

        /// <summary>
        /// Retourne tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>.
        /// </summary>
        /// <returns>
        /// Tous les <see cref="GameObject"/> enfants de ce <see cref="GameObject"/>, récursivement, sans inclure 
        /// le <see cref="GameObject"/> courant.
        /// </returns>
        IList<GameObject> GetAllChildrens();
    }
}