using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Filtre de dépendance utilisant le nom du GameObject contenant la dépendance.
    /// </summary>
    public class Named : Filter
    {
        private readonly string name;

        /// <summary>
        /// Construit un nouveau filtre pour les GameObject avec le nom spécifié.
        /// </summary>
        /// <param name="name">Nom du GameObject accpeté par ce filtre.</param>
        public Named(string name)
        {
            this.name = name;
        }

        protected override bool FilterDependency(object dependency)
        {
            //Theses conditions are ordered by "most likely to happen".
            if (dependency is Component)
            {
                return ((Component) dependency).gameObject.name == name;
            }
            if (dependency is GameObject)
            {
                return ((GameObject) dependency).name == name;
            }
            return false;
        }
    }
}