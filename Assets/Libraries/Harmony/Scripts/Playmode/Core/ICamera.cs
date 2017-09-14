using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente une Caméra.
    /// </summary>
    public interface ICamera : IDisableable
    {
        /// <summary>
        /// Position de la caméra dans le monde.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Hauteur (comprendre dimension, pas position) de la caméra en World Units.
        /// </summary>
        float CameraHeightInWorldUnit { get; }

        /// <summary>
        /// Largeur de la caméra en World Units.
        /// </summary>
        float CameraWidthInWorldUnit { get; }

        /// <summary>
        /// Pour la position dans le monde donné, retourne son équivalent dans la caméra.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Les positions à l'écran sont calculés non pas à partir du centre de l'écran, mais à 
        /// partir du coin inférieur gauche jusqu'au coin supérieur droit. Elles vont donc de 0 
        /// à 1 sur l'axe horizontal et l'axe vertical.
        /// </para>
        /// <para>
        /// Prenez note que la position en Z dans le monde est égale à la position en Z de la caméra.
        /// </para>
        /// </remarks>
        /// <param name="position">Position dans le monde.</param>
        /// <returns>Position correspondante dans la caméra.</returns>
        Vector3 WorldToViewportPoint(Vector3 position);

        /// <summary>
        /// Pour la position à l'écran donné, retourne son équivalent dans le monde.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Les positions à l'écran sont calculés non pas à partir du centre de l'écran, mais à 
        /// partir du coin inférieur gauche jusqu'au coin supérieur droit. Elles vont donc de 0 
        /// à 1 sur l'axe horizontal et l'axe vertical.
        /// </para>
        /// <para>
        /// Prenez note que la position en Z dans le monde est égale à la position en Z de la caméra.
        /// </para>
        /// </remarks>
        /// <param name="position">Position sur la caméra.</param>
        /// <returns>Position correspondante dans le monde.</returns>
        Vector3 ViewportToWorldPoint(Vector3 position);

        /// <summary>
        /// Pour la position à l'écran donné, en pixels, retourne son équivalent dans le monde.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Les positions à l'écran sont calculés non pas à partir du centre de l'écran, mais à 
        /// partir du coin inférieur gauche jusqu'au coin supérieur droit. Elles vont donc de 0 
        /// à la taille de l'écran en pixel sur l'axe horizontal et l'axe vertical.
        /// </para>
        /// <para>
        /// Prenez note que la position en Z dans le monde est égale à la position en Z de la caméra.
        /// </para>
        /// </remarks>
        /// <param name="position">Position sur la caméra.</param>
        /// <returns>Position correspondante dans le monde.</returns>
        Vector3 ScreenToWorldPoint(Vector3 position);
    }
}