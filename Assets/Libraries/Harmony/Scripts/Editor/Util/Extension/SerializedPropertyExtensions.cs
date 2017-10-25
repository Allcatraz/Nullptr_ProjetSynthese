using System;
using UnityEditor;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les SerializedProperty.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        /// <summary>
        /// Indique si une propriété est valide et utilisable.
        /// </summary>
        /// <param name="property">SerializedProperty à vérifier.</param>
        /// <returns>Vrai si la propriété est valide, faux sinon.</returns>
        public static bool IsValid(this SerializedProperty property)
        {
            //If this throws an Exception, the property is invalid.
            try
            {
                string ignore = property.name;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Indique si une propriété a besoin d'être récréée, car elle n'est plus valide.
        /// </summary>
        /// <param name="property">SerializedProperty à vérifier.</param>
        /// <returns>Vrai si la propriété n'est plus valide et doit être recréée, faux sinon.</returns>
        public static bool NeedRefresh(this SerializedProperty property)
        {
            return !property.IsValid();
        }
    }
}