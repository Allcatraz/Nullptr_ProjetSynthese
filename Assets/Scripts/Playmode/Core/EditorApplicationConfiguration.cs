using Harmony.Testing;
using UnityEngine;

namespace ProjetSynthese
{

#if UNITY_EDITOR
    /// <summary>
    /// Le EditorApplicationConfiguration est une version de ApplicationConfiguration créée uniquement pour l'éditeur.
    /// 
    /// Créée une configuration par défaut lorsque le jeu est démarré dans l'éditeur.
    /// </summary>
    [NotTested(Reason.Configuration)]
    [AddComponentMenu("Game/Config/EditorApplicationConfiguration")]
    public class EditorApplicationConfiguration : ApplicationConfiguration
    {
        /// <summary>
        /// Évènement appellé lorsque le EditorApplicationConfiguration est construit.
        /// 
        /// S'il existe déjà une configuration de l'application, elle ne fait rien. Sinon, elle créée une configuration
        /// pour l'éditeur.
        /// </summary>
        public new void Awake()
        {
            if (applicationConfiguration == null)
            {
                base.Awake();
            }
        }

        /// <summary>
        /// Évènement appellé lorsque le ApplicationConfiguration est détruit. Détruit le jeu.
        /// 
        /// Ne fait rien si la configuration n'est pas une configuration d'éditeur.
        /// </summary>
        public new void OnDestroy()
        {
            if (applicationConfiguration is EditorApplicationConfiguration)
            {
                base.OnDestroy();
            }
        }

        /// <summary>
        /// Indique si c'est une configuration en mode éditeur qui est utilisée ou si c'est une configuration normale.
        /// </summary>
        /// <returns>Vrai si c'est une configuration en mode éditeur, Faux sinon.</returns>
        public static bool IsUsingEditorConfiguration()
        {
            return applicationConfiguration is EditorApplicationConfiguration;
        }
    }
#else
    /// <summary>
    /// Version de EditorApplicationConfiguration lorsque compilée en mode release.
    /// 
    /// Vidée de toute fonctionalitée.
    /// </summary>
    [NotTested(Reason.Configuration)]
    [AddComponentMenu("Game/Config/EditorApplicationConfiguration")]
    public class EditorApplicationConfiguration : MonoBehaviour
    {
    }
#endif
}