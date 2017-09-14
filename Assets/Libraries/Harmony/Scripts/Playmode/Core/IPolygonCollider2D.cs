using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un collider 2D sous la forme d'un polygone.
    /// </summary>
    public interface IPolygonCollider2D : ICollider2D
    {
        /// <summary>
        /// Retourne les points du polygone de colision.
        /// </summary>
        /// <returns>Liste des points du polygone de colision.</returns>
        [NotNull]
        IList<Vector2> GetPoints();

        /// <summary>
        /// Modifie les points du polygone de colision.
        /// </summary>
        /// <param name="points">Liste des nouveaux points du polygone de colision.</param>
        void SetPoints([NotNull] IList<Vector2> points);
    }
}