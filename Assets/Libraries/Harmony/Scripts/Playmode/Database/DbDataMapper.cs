using System.Data.Common;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Permet de transformer l'enregistrement courant dans un DbDataReader en un objet.
    /// </summary>
    /// <typeparam name="T">Structure de données produite.</typeparam>
    /// <seealso cref="DbRepository{T}"/>
    public interface IDbDataMapper<out T>
    {
        /// <summary>
        /// Retourne la dernière clée primaire créée sur cette connexion pour ce type d'objet.
        /// </summary>
        /// <param name="connection">Connexion sur laquelle une insertion vient de se produire.</param>
        /// <returns>Dernière clé primaire générée, sous forme d'entier numérique, sur la connexion donnée.</returns>
        long GetPrimaryKeyFromConnection([NotNull] DbConnection connection);

        /// <summary>
        /// Retourne l'objet correspondant à l'enregistrement donné.
        /// </summary>
        /// <param name="reader">Curseur (DbDataReader) contenant l'enregistrement à transformer.</param>
        /// <returns>Un objet représentant les données contenues dans le DbDataReader.</returns>
        [NotNull]
        T GetObjectFromReader([NotNull] DbDataReader reader);
    }
}