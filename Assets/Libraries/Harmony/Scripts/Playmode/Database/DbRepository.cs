using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Base pour créer facilement des <i>Repositories</i> pour une base de données relationelle.
    /// </summary>
    /// <typeparam name="T">Structure de données stockées par ce <i>repository</i>.</typeparam>
    /// <remarks>
    /// <para>
    /// Créer des classes servant à accéder à une base de données est une tâche assez lourde et pénible : il y a beaucoup de
    /// <i>Boilerplate Code</i>, ce qui rend l'écriture de <i>Repositories</i> fastidieux.
    /// </para>
    /// <para>
    /// Harmony fourni une classe abstraite nommée DbRepository pour faciliter l'écriture de <i>repositories</i>. Son 
    /// implémentation et son utilisation est très simple. Par exemple :
    /// </para>
    /// <code>
    /// private class Repository : DbRepository<Game>
    /// {
    ///     public Repository([NotNull] DbConnectionFactory connectionFactory,
    ///                       [NotNull] DbParameterFactory parameterFactory,
    ///                       [NotNull] DbDataMapper<Game> dataMapper)
    ///         : base(connectionFactory, parameterFactory, dataMapper)
    ///     {
    ///     }
    ///     
    ///     public void AddGame(Game game)
    ///     {
    ///         ExecuteInsert("INSERT INTO Games (points,time) VALUES (?,?);", new object[]
    ///         {
    ///                 game.Points,
    ///                 game.time,
    ///         });
    ///     }
    /// 
    /// 	public IList<Game> GetAllGames()
    ///     {
    ///         return ExecuteSelectAll("SELECT * FROM Games;", new object[] {});
    ///     }
    /// }
    /// </code>
    /// <para>
    /// La classe DbRepository s'occupe elle même d'ouvrir une connexion à la base de données, d'effectuer la requête (lecture 
    /// ou écriture), de retourner les données en provenance de la base de données et de fermer la connexion. Il suffit 
    /// d'appeler les différentes méthodes <i>protected</i> qu'elle contient (ExecuteScallar, ExecuteSelectOne, ExecuteSelectAll, 
    /// ExecuteInsert, ExecuteUpdate ou ExecuteDelete). Consultez leur documentation respective pour les détails.
    /// </para>
    /// <para>
    /// Par contre, il notez qu'il n'y a rien de magique et qu'une certaine quantité de préparation est tout de même nécessaire.
    /// En fait, DbRepository nécessite 3 dépendances qu'il faut lui fournir à la construction : une fabrique de connexions 
    /// (voir DbConnectionFactory), un fabrique de paramêtres (voir DbParameterFactory) et un <i>DataMapper</i> (voir DbDataMapper).
    /// </para>
    /// <para>
    /// La fabrique de connexion sert à fournir une connexion à la base de données sur laquelle le <i>repository</i> pourra faire ses 
    /// requêtes. Par exemple, pour une base de données SqLite :
    /// </para>
    /// <code>
    /// public class SqLiteConnectionFactory : DbConnectionFactory
    /// {
    ///     public DbConnection GetConnection()
    ///     {
    ///         return new SQLiteConnection("URI=file:"App/Data/Database.db");
    ///     }
    /// }
    /// </code>
    /// <para>
    /// La fabrique de paramêtre sert à créer des <i>DbParameter</i> à partir d'une valeur (tel qu'un int, un string ou un double). Par 
    /// exemple, pour une base de données SqLite :
    /// </para>
    /// <code>
    /// public class SqLiteParameterFactory : DbParameterFactory
    /// {
    ///     public DbParameter GetParameter(object value)
    ///     {
    ///         return new SQLiteParameter
    ///         {
    ///             Value = value
    ///         };
    ///     }
    /// }
    /// </code>
    /// <para>
    /// Enfin, le DbDataMapper sert à créer un objet à partir d'un <i>DbDataReader</i>. En fait, la classe DbRepository n'est pas 
    /// en mesure de créer automatiquement un objet à partir des données d'un curseur. Elle requiert donc les services d'un DbDataMapper 
    /// qui fera justement cette conversion pour lui. En temps normal, ce genre de conversion se ferait ainsi au sein du Repository :
    /// </para>
    /// <code>
    /// IList<Games> games = new List<Games>();
    /// DbDataReader cursor = command.ExecuteReader();
    /// while (cursor.Read())
    /// {
    ///     games.Add(new Game(cursor.GetInt32(0), cursor.GetInt32(1), cursor.GetInt64(2)));
    /// }
    /// </code>
    /// <para>
    /// Avec un DbDataMapper, la conversion se fait par contre ainsi. La méthode <i>GetObjectFromReader</i> est appellée par DbRepository
    /// pour chaque enregistrement dans la base de données.
    /// </para>
    /// <code>
    /// public class GamesMapper : DbDataMapper<Game>
    /// {
    ///     public Game GetObjectFromReader(DbDataReader reader)
    ///     {
    ///         return new Game
    ///         {
    ///             Id = reader.GetInt32(reader.GetOrdinal("id")),
    ///             Points = reader.GetInt32(reader.GetOrdinal("points")),
    ///             Time = reader.GetInt64(reader.GetOrdinal("time"))
    ///         };
    ///     }
    /// }
    /// </code>
    /// <para>
    /// <i>Prenez note que la classe DbRepository n'est pas un Component (comprendre MonoBehaviour).</i>
    /// </para>
    /// <para>
    /// <b>IMPORTANT! Au sujet de la structure des tables supportées.</b>
    /// </para>
    /// <para>
    /// DbRepository est assez souple pour supporter à peu près toutes les structures de bases de données.
    /// Il y a cependant une contrainte : les clés primaires des tables doivent être des clés synthétiques
    /// (comprendre un identifiant unique à base d'entier numérique).
    /// </para>
    /// <para>
    /// <b>Exemple</b>
    /// </para>
    /// <para>
    /// Voici un exemple d'implémentation d'un DbRepository complet.
    /// </para>
    /// <code>
    /// public class GameRepository : DbRepository<Game>
    /// {
    ///     public GameRepository([NotNull] DbConnectionFactory connectionFactory,
    ///                           [NotNull] DbParameterFactory parameterFactory,
    ///                           [NotNull] DbDataMapper<Game> dataMapper)
    ///         : base(connectionFactory, parameterFactory, dataMapper)
    ///     {
    ///     }
    /// 
    ///     public void CreateGame(Game game)
    ///     {
    ///         game.Id = ExecuteInsert("INSERT INTO Games (shotsFired) VALUES (?);", new object[]
    ///         {
    ///             game.ShotsFired
    ///         });
    ///     }
    /// 
    ///     public Game RetreiveGame(int id)
    ///     {
    ///         return ExecuteSelectOne("SELECT * FROM Games WHERE id = ?;", new object[]
    ///         {
    ///             id
    ///         });
    ///     }
    /// 
    ///     public IList<Game> RetreiveAllGames()
    ///     {
    ///         return ExecuteSelectAll("SELECT * FROM Games;", new object[] { });
    ///     }
    /// 
    ///     public void UpdateGame(Game game)
    ///     {
    ///         ExecuteUpdate("UPDATE Games SET shotsFired = ? WHERE id = ?;", new object[]
    ///         {
    ///             game.ShotsFired,
    ///             game.Id
    ///         });
    ///     }
    /// 
    ///     public void DeleteGame(Game game)
    ///     {
    ///         ExecuteDelete("DELETE FROM Games WHERE id = ?;", new object[]
    ///         {
    ///             game.Id
    ///         });
    ///     }
    /// }
    /// </code>
    /// </remarks>
    /// <seealso cref="IDbConnectionFactory"/>
    /// <seealso cref="IDbParameterFactory"/>
    /// <seealso cref="IDbDataMapper{T}"/>
    public abstract class DbRepository<T> where T : class
    {
        //#Dirty Fix : Unity does not load DLLs that depends on other DLLs
#if !UNITY_EDITOR
        static DbRepository()
        {
            string path = System.Environment.GetEnvironmentVariable("PATH", System.EnvironmentVariableTarget.Process);
            string pluginPath = UnityEngine.Application.dataPath + System.IO.Path.DirectorySeparatorChar + "Plugins";
            UnityEngine.Debug.Log("THE PATH IS : " + pluginPath);
            if (path.Contains(pluginPath) == false)
            {
                System.Environment.SetEnvironmentVariable("PATH", path + System.IO.Path.PathSeparator + pluginPath,
                                                          System.EnvironmentVariableTarget.Process);
            }
        }

#endif

        private readonly IDbConnectionFactory connectionFactory;
        private readonly IDbParameterFactory parameterFactory;
        private readonly IDbDataMapper<T> dataMapper;

        /// <summary>
        /// Constructeur de DbRepository.
        /// </summary>
        /// <param name="connectionFactory">
        /// Fabrique de connexion à la base de données. Utilisée pour créer une connexion avant chaque requête.
        /// </param>
        /// <param name="parameterFactory">
        /// Fabrique de DBParameter. Utilisée pour chaque paramêtre d'une requête préparée.
        /// </param>
        /// <param name="dataMapper">
        /// <see cref="IDbDataMapper{T}"/> à utiliser pour transformer les données provenant de la base de données en un objet
        /// ainsi que pour obtenir les clés primaires générées.
        /// </param>
        /// <seealso cref="IDbConnectionFactory"/>
        /// <seealso cref="IDbParameterFactory"/>
        /// <seealso cref="IDbDataMapper{T}"/>
        protected DbRepository([NotNull] IDbConnectionFactory connectionFactory,
                               [NotNull] IDbParameterFactory parameterFactory,
                               [NotNull] IDbDataMapper<T> dataMapper)
        {
            this.connectionFactory = connectionFactory;
            this.parameterFactory = parameterFactory;
            this.dataMapper = dataMapper;
        }

        /// <summary>
        /// Execute une requête Select dans le but d'obtenir une valeur entière en provenance de la base de 
        /// données. Ces requêtes ne doivent donc avoir qu'une seul colonne et ne donner qu'un seul enregistrement,
        /// tel qu'un compte, une somme ou une moyenne.
        /// </summary>
        /// <param name="commandSql">
        /// Requête SQL SELECT à exécuter. Pour des questions de sécurité, cela doit être une requête préparée. 
        /// </param>
        /// <param name="parametersValues">
        /// Valeurs à placer dans les paramêtres de la requête préparée. Ne devraient être que des types de base.
        /// </param>
        /// <returns>Valeur demandé dans la requête.</returns>
        protected virtual long ExecuteScallar([NotNull] string commandSql, [NotNull] object[] parametersValues)
        {
            return ExecuteQueryScallar(commandSql, parametersValues);
        }

        /// <summary>
        /// Execute une requête Select dans le but d'obtenir un seul objet de la base de données. Si plusieurs objets
        /// résultent de la requête, seul le premier sera retourné.
        /// </summary>
        /// <param name="commandSql">
        /// Requête SQL SELECT à exécuter. Pour des questions de sécurité, cela doit être une requête préparée. 
        /// </param>
        /// <param name="parametersValues">
        /// Valeurs à placer dans les paramêtres de la requête préparée. Ne devraient être que des types de base.
        /// </param>
        /// <returns>Objet demandé dans la requête, ou null s'il en existe aucun.</returns>
        [CanBeNull]
        protected virtual T ExecuteSelectOne([NotNull] string commandSql, [NotNull] object[] parametersValues)
        {
            return ExecuteQueryOne(commandSql, parametersValues);
        }

        /// <summary>
        /// Execute une requête Select et retourne tous les objets en résultant.
        /// </summary>
        /// <param name="commandSql">
        /// Requête SQL SELECT à exécuter. Pour des questions de sécurité, cela doit être une requête préparée. 
        /// </param>
        /// <param name="parametersValues">
        /// Valeurs à placer dans les paramêtres de la requête préparée. Ne devraient être que des types de base.
        /// </param>
        /// <returns>Liste des objets obtenus lors de la requête. Ne sera jamais <c>null</c>.</returns>
        [NotNull]
        protected virtual IList<T> ExecuteSelectAll([NotNull] string commandSql, [NotNull] object[] parametersValues)
        {
            return ExecuteQueryList(commandSql, parametersValues);
        }

        /// <summary>
        /// Execute une requête Insert.
        /// </summary>
        /// <param name="commandSql">
        /// Requête SQL INSERT à exécuter. Pour des questions de sécurité, cela doit être une requête préparée. 
        /// </param>
        /// <param name="parametersValues">
        /// Valeurs à placer dans les paramêtres de la requête préparée. Ne devraient être que des types de base.
        /// </param>
        /// <returns>Le <i>Id</i> de l'élément nouvellement créé dans la base de données.</returns>
        protected virtual long ExecuteInsert([NotNull] string commandSql, [NotNull] object[] parametersValues)
        {
            return ExecuteNonQueryWithId(commandSql, parametersValues);
        }

        /// <summary>
        /// Execute une requête Update.
        /// </summary>
        /// <param name="commandSql">
        /// Requête SQL UPDATE à exécuter. Pour des questions de sécurité, cela doit être une requête préparée. 
        /// </param>
        /// <param name="parametersValues">
        /// Valeurs à placer dans les paramêtres de la requête préparée. Ne devraient être que des types de base.
        /// </param>
        protected virtual void ExecuteUpdate([NotNull] string commandSql, [NotNull] object[] parametersValues)
        {
            ExecuteNonQuery(commandSql, parametersValues);
        }

        /// <summary>
        /// Execute une requête Delete.
        /// </summary>
        /// <param name="commandSql">
        /// Requête SQL DELETE à exécuter. Pour des questions de sécurité, cela doit être une requête préparée. 
        /// </param>
        /// <param name="parametersValues">
        /// Valeurs à placer dans les paramêtres de la requête préparée. Ne devraient être que des types de base.
        /// </param>
        protected virtual void ExecuteDelete([NotNull] string commandSql, [NotNull] object[] parametersValues)
        {
            ExecuteNonQuery(commandSql, parametersValues);
        }

        private long ExecuteQueryScallar(string commandSql, object[] parametersValues)
        {
            using (DbConnection connection = connectionFactory.GetConnection())
            {
                connection.Open();

                return (long) GetCommand(connection, commandSql, parametersValues).ExecuteScalar();
            }
        }

        private T ExecuteQueryOne(string commandSql, object[] parametersValues)
        {
            using (DbConnection connection = connectionFactory.GetConnection())
            {
                connection.Open();

                DbCommand command = GetCommand(connection, commandSql, parametersValues);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return dataMapper.GetObjectFromReader(reader);
                    }
                }
            }
            return null;
        }

        private IList<T> ExecuteQueryList(string commandSql, object[] parametersValues)
        {
            IList<T> dataObjects = new List<T>();
            using (DbConnection connection = connectionFactory.GetConnection())
            {
                connection.Open();

                DbCommand command = GetCommand(connection, commandSql, parametersValues);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataObjects.Add(dataMapper.GetObjectFromReader(reader));
                    }
                }
            }
            return dataObjects;
        }

        private void ExecuteNonQuery(string commandSql, object[] parametersValues)
        {
            using (DbConnection connection = connectionFactory.GetConnection())
            {
                connection.Open();

                DbCommand command = GetCommand(connection, commandSql, parametersValues);
                command.ExecuteNonQuery();
            }
        }

        private long ExecuteNonQueryWithId(string commandSql, object[] parametersValues)
        {
            using (DbConnection connection = connectionFactory.GetConnection())
            {
                connection.Open();

                DbCommand command = GetCommand(connection, commandSql, parametersValues);
                command.ExecuteNonQuery();

                return dataMapper.GetPrimaryKeyFromConnection(connection);
            }
        }

        private DbCommand GetCommand(DbConnection connection, string commandSql, object[] parametersValues)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = commandSql;
            foreach (object value in parametersValues)
            {
                command.Parameters.Add(parameterFactory.GetParameter(value));
            }
            return command;
        }
    }
}