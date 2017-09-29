using Harmony;

namespace ProjetSynthese
{
    /// <summary>
    /// Un NetworkGameScript est comme un <see cref="GameScript"/>, à la différence qu'il peut servir pour des jeu en réseau.
    /// </summary>
    public abstract class NetworkGameScript : NetworkScript
    {
        /// <summary>
        /// Injecte les dépendances de ce NetworkGameScript.
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