using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente le moteur physique.
    /// </summary>
    public interface IPhysics2D
    {
        /// <summary>
        /// Gravité actuellment utilisée pour les calculs de physique.
        /// </summary>
        Vector2 Gravity { get; set; }
    }
}