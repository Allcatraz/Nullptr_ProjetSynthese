using Harmony;

namespace ProjetSynthese
{
    public class GameScript : Script
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
