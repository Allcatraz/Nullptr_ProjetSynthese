using UnityEngine.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour le gestionaire de scènes.
    /// </summary>
    public static class SceneManagerExtensions
    {
        /// <summary>
        /// Indique si une scène donnée est chargée ou non.
        /// </summary>
        /// <param name="sceneName">Nom de la scène.</param>
        /// <returns>Vrai si la scène est chargée (ou en cours de chargement), faux sinon.</returns>
        public static bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}