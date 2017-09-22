using UnityEngine;

namespace ProjetSynthese
{
#if UNITY_EDITOR
    /// <summary>
    /// Le EditorApplicationConfiguration est une version de ApplicationConfiguration créée uniquement pour l'éditeur.
    /// 
    /// Créée une configuration par défaut lorsque le jeu est démarré dans l'éditeur.
    /// </summary>
    [AddComponentMenu("Game/Config/EditorApplicationConfiguration")]
    public class EditorApplicationConfiguration : ApplicationConfiguration
    {
        /// <summary>
        /// Évènement appellé lorsque le EditorApplicationConfiguration est construit.
        /// 
        /// S'il existe déjà une configuration, elle ne fait rien afin de ne pas écraser la configuration actuelle. Sinon, elle 
        /// créée une configuration de type "Éditeur".
        /// </summary>
        public new void Awake()
        {
            if (applicationConfiguration == null)
            {
                //EditorConfiguration is the same as the ApplicatonConfiguration...for now.
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
            if (IsUsingEditorConfiguration())
            {
                //The process of destroying a EditorConfiguration is the same as for the ApplicatonConfiguration...for now.
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
    [AddComponentMenu("Game/Config/EditorApplicationConfiguration")]
    public class EditorApplicationConfiguration : MonoBehaviour
    {
    }
#endif
}