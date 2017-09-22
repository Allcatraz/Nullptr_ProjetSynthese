using Harmony;

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
    /// <see cref="Injector"/> pour effectuer de l'injection de dépendances. Consultez la documentation 
    /// de <see cref="Injector"/> pour les détails.
    /// </para>
    /// </remarks>
    /// <seealso cref="Injector"/>
    /// <seealso cref="Script"/>
    public abstract class GameScript : Script
    {
        /// <summary>
        /// Injecte les dépendances de ce GameScript.
        /// </summary>
        /// <param name="injectMethodName">
        /// Nom de la méthode où l'injection doit être effectuée.
        /// </param>
        protected void InjectDependencies(string injectMethodName)
        {
            ApplicationConfiguration.InjectDependencies(this, injectMethodName);
        }
    }
}