using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Fournisseur de nombres aléatoires.
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// Retourne une position aléatoire entre les bornes données. Les positions négatives sont supportées.
        /// </summary>
        /// <param name="minX">Valeur minimale pour X.</param>
        /// <param name="maxX">Valeur maximale pour X.</param>
        /// <param name="minY">Valeur minimale pour Y.</param>
        /// <param name="maxY">Valeur maximale pour Y.</param>
        /// <returns>Vecteur aléatoire.</returns>
        Vector2 GetRandomPosition(float minX, float maxX, float minY, float maxY);

        /// <summary>
        /// Retourne une position aléatoire sur les bords d'un rectangle donné.
        /// </summary>
        /// <param name="center">Centre du rectangle.</param>
        /// <param name="height">Hauteur du rectangle.</param>
        /// <param name="width">Largeur du rectangle.</param>
        /// <returns>Position aléatoire sur l'un des bords du rectangle donné.</returns>
        /// <exception cref="System.ArgumentException">Si Height ou Width sont négatifs.</exception>
        Vector2 GetRandomPositionOnRectangleEdge(Vector2 center, float height, float width);

        /// <summary>
        /// Retourne une direction aléatoire. Le vecteur retournée est normalisé et a donc une longueur de 1.
        /// </summary>
        /// <returns>Vecteur représentant une direction aléatoire.</returns>
        Vector2 GetRandomDirection();

        /// <summary>
        /// Retourne 1 ou -1 de manière aléatoire. Il y a 50 % de chance pour chaque option.
        /// </summary>
        /// <returns>1 ou -1</returns>
        int GetOneOrMinusOneAtRandom();

        /// <summary>
        /// Retourne un nombre entier aléatoire entre deux valeurs.
        /// </summary>
        /// <param name="min">Valeur minimal. Inclusif.</param>
        /// <param name="max">Valeur maximale. Inclusif.</param>
        /// <returns>Nombre aléatoire entre deux valeurs.</returns>
        /// <exception cref="System.ArgumentException">Si Min est plus grand que Max.</exception>
        uint GetRandomUInt(uint min, uint max);

        /// <summary>
        /// Retourne un nombre entier aléatoire entre deux valeurs. Les valeurs négatives sont supportées.
        /// </summary>
        /// <param name="min">Valeur minimal. Inclusif.</param>
        /// <param name="max">Valeur maximale. Inclusif.</param>
        /// <returns>Nombre aléatoire entre deux valeurs.</returns>
        /// <exception cref="System.ArgumentException">Si Min est plus grand que Max.</exception>
        int GetRandomInt(int min, int max);

        /// <summary>
        /// Retourne un nombre à virgule flotante aléatoire entre deux valeurs. Les valeurs négatives sont supportées.
        /// </summary>
        /// <param name="min">Valeur minimal. Inclusif.</param>
        /// <param name="max">Valeur maximale. Inclusif.</param>
        /// <returns>Nombre aléatoire entre deux valeurs.</returns>
        /// <exception cref="System.ArgumentException">Si Min est plus grand que Max.</exception>
        float GetRandomFloat(float min, float max);
    }
}