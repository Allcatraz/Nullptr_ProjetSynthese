using Harmony.Injection;
using Harmony.Testing;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    /// <summary>
    /// Le ApplicationConfiguration est la configuration du jeu, c'est à dire la logique d'initialisation.
    /// Il configure notamment le <see cref="Injector"/> utilisé au travers de l'application.
    /// </summary>
    [NotTested(Reason.Configuration)]
    [AddComponentMenu("Game/Config/ApplicationConfiguration")]
    public class ApplicationConfiguration : GameScript
    {
        protected static ApplicationConfiguration applicationConfiguration;

        private Injector injector;

        /// <summary>
        /// Injecte les dépendances du GameScript reçu en paramètre.
        /// </summary>
        /// <param name="target">
        /// Le UnityScript où effectuer l'injection des dépendances.
        /// </param>
        /// <param name="injectMethodName">
        /// Nom de la méthode où l'injection doit être effectuée.
        /// </param>
        /// <param name="valueDependencies">
        /// Les dépendences de <i>valeur</i> à envoyer à la méthode <i>Inject</i>. L'ordre est important.
        /// </param>
        public static void InjectDependencies(GameScript target, string injectMethodName, params object[] valueDependencies)
        {
            //This null check is only usefull for test purposes. If no "ApplicationConfiguration" exists,
            //but this method is called, this must mean that we might be in a unit test.
            if (applicationConfiguration != null)
            {
                applicationConfiguration.injector.InjectDependencies(target, injectMethodName, valueDependencies);
            }
        }

        /// <summary>
        /// Évènement appellé lorsque le ApplicationConfiguration est construit. Initialise le jeu.
        /// </summary>
        public void Awake()
        {
            applicationConfiguration = this;

            injector = new Injector(new ApplicationInjectionContext());
        }

        /// <summary>
        /// Évènement appellé lorsque le ApplicationConfiguration est détruit. Détruit le jeu.
        /// </summary>
        public void OnDestroy()
        {
            applicationConfiguration = null;
        }
    }
}
