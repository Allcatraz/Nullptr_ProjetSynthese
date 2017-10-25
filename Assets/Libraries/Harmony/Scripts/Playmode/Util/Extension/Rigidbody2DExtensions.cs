using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Contient nombre de méthodes d'extensions pour les Rigidbody2D.
    /// </summary>
    public static class Rigidbody2DExtensions
    {
        public static void Translate(this Rigidbody2D rigidbody2D, Vector2 translation)
        {
            rigidbody2D.MovePosition(rigidbody2D.position + translation);
        }

        public static void Rotate(this Rigidbody2D rigidbody2D, float angle)
        {
            rigidbody2D.MoveRotation(rigidbody2D.rotation + angle);
        }
    }
}