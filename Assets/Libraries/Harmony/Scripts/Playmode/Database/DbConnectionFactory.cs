using System.Data.Common;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Fabrique de DbConnection pour une base de données relationelle.
    /// </summary>
    /// <seealso cref="DbRepository{T}"/>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Retourne une DbConnection à une base de données. Cette connexion peut être une nouvelle
        /// connexion comme être une connexion préexistante.
        /// </summary>
        /// <returns>DbConnection à une base de données.</returns>
        [NotNull]
        DbConnection GetConnection();
    }
}