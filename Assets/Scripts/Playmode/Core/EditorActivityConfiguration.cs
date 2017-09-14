using Harmony;
using Harmony.Injection;
using Harmony.Testing;
using Harmony.Unity;
using UnityEngine;

namespace ProjetSynthese
{
    /// <summary>
    /// Le EditorActivityConfiguration permet de démarrer une activité spécifique à partir de l'éditeur.
    /// </summary>
    [NotTested(Reason.Configuration)]
    [AddComponentMenu("Game/Config/EditorActivityConfiguration")]
    public class EditorActivityConfiguration : GameScript
    {
#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private UnityActivity activity;

        private IActivityStack activityStack;

        /// <summary>
        /// Activité à charger au lancement.
        /// </summary>
        public UnityActivity Activity
        {
            get { return activity; }
            set { activity = value; }
        }

        /// <summary>
        /// Point d'injection des dépendances de EditorActivityConfiguration.
        /// </summary>
        /// <param name="activityStack">IActivityStack à utiliser pour charger l'activité au lancement</param>
        public void InjectEditorActivityConfigurationr([ApplicationScope] IActivityStack activityStack)
        {
            this.activityStack = activityStack;
        }

        /// <summary>
        /// Initialise le EditorActivityConfiguration. Démarre l'injection de dépendances.
        /// </summary>
        public void Awake()
        {
            InjectDependencies("InjectEditorActivityConfigurationr");
        }

        /// <summary>
        /// Démarre l'activité si on est en mode éditeur et qu'il existe une activité à charger.
        /// </summary>
        public void Start()
        {
            //Only load specified activity if the Application scene is the only one loaded
            if (EditorApplicationConfiguration.IsUsingEditorConfiguration() && activity != null)
            {
                activityStack.StartActivity(activity);
            }
            else
            {
                Debug.LogWarning("You haven't started the game using the \"Start Activity\" button. Your activity might not start like " +
                                 "you expect it to do.");
            }
        }
#endif
    }
}