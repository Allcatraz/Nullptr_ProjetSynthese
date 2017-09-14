using Harmony.Testing;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
using UnityEngine;

namespace ProjetSynthese
{
    /// <summary>
    /// Un GameScript est une toute petite unité de logique du jeu. Elle ne participe qu'à une seule chose
    /// durant le jeu, tel que jouer un son lorsqu'un évènement survient ou déplacer un personnage lorsqu'une
    /// touche est appuyée.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Les GameBehaviours nécessitent qu'il existe un (et un seul) <see cref="ApplicationConfiguration"/> dans une 
    /// des scènes chargées. Ce <see cref="ApplicationConfiguration"/> doit donc aussi être le tout premier script qui 
    /// s'exécute.
    /// </para>
    /// <para>
    /// GameScript se sert de <see cref="ApplicationConfiguration"/> pour obtenir certains objets, dont un
    /// <see cref="Harmony.Injection.Injector"/> pour effectuer de l'injection de dépendances. Consultez la documentation 
    /// de <see cref="Harmony.Injection.Injector"/> pour les détails.
    /// </para>
    /// </remarks>
    /// <seealso cref="Harmony.Injection.Injector"/>
    /// <seealso cref="Harmony.UnityScript"/>
    [NotTested(Reason.Wrapper)]
    public abstract class GameScript : Harmony.UnityScript
    {
        /// <summary>
        /// Injecte les dépendances de ce GameScript.
        /// </summary>
        /// <param name="injectMethodName">
        /// Nom de la méthode où l'injection doit être effectuée.
        /// </param>
        /// <param name="valueDependencies">
        /// Les dépendences de <i>valeur</i> à envoyer à la méthode <i>Inject</i>. L'ordre est important.
        /// </param>
        protected void InjectDependencies(string injectMethodName, params object[] valueDependencies)
        {
            ApplicationConfiguration.InjectDependencies(this, injectMethodName, valueDependencies);
        }
    }
}