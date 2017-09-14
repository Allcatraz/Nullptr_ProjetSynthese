using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un Renderer pour des lignes à l'écran.
    /// </summary>
    public interface ILineRenderer : IRenderer
    {
        /// <summary>
        /// Indique si les lignes de ce ILineRenderer forment une boucle. Autrement dit, si le dernier point
        /// est relié au premier point.
        /// </summary>
        bool Loop { get; set; }

        /// <summary>
        /// Retourne les points de la ligne.
        /// </summary>
        /// <returns>Liste des points de la ligne.</returns>
        [NotNull]
        IList<Vector3> GetPoints();

        /// <summary>
        /// Modifie les points de la ligne.
        /// </summary>
        /// <param name="points">Liste des nouveaux points de la ligne.</param>
        void SetPoints([NotNull] IList<Vector3> points);
    }
}