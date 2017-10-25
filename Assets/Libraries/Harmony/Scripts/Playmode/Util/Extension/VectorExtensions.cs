using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour des vecteurs Unity.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Effectue la rotation d'un point autour d'un pivot avec des angles en degrés.
        /// </summary>
        /// <param name="point">Point ayant à subir la rotation.</param>
        /// <param name="pivot">Pivot de rotation.</param>
        /// <param name="x">Angle de rotation en degrés sur l'axe des X.</param>
        /// <param name="y">Angle de rotation en degrés sur l'axe des Y.</param>
        /// <param name="z">Angle de rotation en degrés sur l'axe des Z.</param>
        /// <returns>Nouveau point avec la rotation souhaité.</returns>
        public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, float x = 0, float y = 0, float z = 0)
        {
            return Quaternion.Euler(x, y, z) * (point - pivot) + pivot;
        }
    }
}