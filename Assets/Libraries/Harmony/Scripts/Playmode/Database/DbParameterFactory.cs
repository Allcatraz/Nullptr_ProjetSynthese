using System.Data.Common;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Fabrique de DbParameter pour une base de données relationelles.
    /// </summary>
    /// <seealso cref="DbRepository{T}"/>
    public interface IDbParameterFactory
    {
        /// <summary>
        /// Crée un DbParameter pour la valeur donnée. Cette dernière doit obligatoirement être 
        /// un type de base.
        /// </summary>
        /// <param name="value">
        /// Valeur à stocker dans la base de données et dont il faut créer un DbParameter. La valeur
        /// <c>null</c> est autorisée.
        /// </param>
        /// <returns>Nouveau DbParameter pour la valeur donnée.</returns>
        [NotNull]
        DbParameter GetParameter([CanBeNull] object value);
    }
}