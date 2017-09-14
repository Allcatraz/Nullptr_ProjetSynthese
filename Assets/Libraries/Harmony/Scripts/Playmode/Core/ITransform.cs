using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente le positionnement d'un objet dans l'espace.
    /// </summary>
    public interface ITransform : IComponent
    {
        /// <summary>
        /// Position de l'objet par rapport à son parent.
        /// </summary>
        Vector3 LocalPosition { get; set; }

        /// <summary>
        /// Rotation de l'objet par rapport à son parent.
        /// </summary>
        Quaternion LocalRotation { get; set; }

        /// <summary>
        /// Taille de l'objet par rapport à son parent.
        /// </summary>
        Vector3 LocalScale { get; set; }

        /// <summary>
        /// Position de l'objet dans le monde (en prenant en compte la position du parent).
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Rotation de l'objet dans le monde (en prenant en compte la position du parent).
        /// </summary>
        Quaternion Rotation { get; set; }

        /// <summary>
        /// Taille de l'objet dans le monde (en prenant en compte la position du parent).
        /// </summary>
        Vector3 Scale { get; }

        /// <summary>
        /// Vecteur poitant vers le haut de l'objet.
        /// </summary>
        Vector3 Up { get; }

        /// <summary>
        /// Déplace l'objet dans le monde.
        /// </summary>
        /// <param name="translation">Déplacement de l'objet.</param>
        void Translate(Vector3 translation);
    }
}