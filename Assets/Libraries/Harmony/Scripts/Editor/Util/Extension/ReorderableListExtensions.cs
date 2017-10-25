using UnityEditorInternal;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les ReorderableList.
    /// </summary>
    public static class ReorderableListExtensions
    {
        /// <summary>
        /// Indique si une propriété est valide et utilisable.
        /// </summary>
        /// <param name="property">SerializedProperty à vérifier.</param>
        /// <returns>Vrai si la propriété est valide, faux sinon.</returns>
        public static bool IsValid(this ReorderableList property)
        {
            return property.serializedProperty.IsValid();
        }

        /// <summary>
        /// Indique si une propriété a besoin d'être récréée, car elle n'est plus valide.
        /// </summary>
        /// <param name="property">SerializedProperty à vérifier.</param>
        /// <returns>Vrai si la propriété n'est plus valide et doit être recréée, faux sinon.</returns>
        public static bool NeedRefresh(this ReorderableList property)
        {
            return property.serializedProperty.NeedRefresh();
        }
    }
}