using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/PlayedGameRepository")]
    public class PlayedGameRepository : GameScript
    {

        private Repository repository;

        public void InjectPlayedGameRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new PlayedGameMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayedGameRepository");
        }

        public virtual void AddPlayedGame(long playerId)
        {
            repository.AddPlayedGame(new PlayedGame { PlayerId = playerId });
        }

        public virtual long GetCountFromPlayer(long playerId)
        {
            return repository.GetCountFromPlayer(playerId);
        }

        private class Repository : DbRepository<PlayedGame>
        {
            private readonly PlayedGameRepository playedGameRepository;

            public Repository(PlayedGameRepository playedGameRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<PlayedGame> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.playedGameRepository = playedGameRepository;
            }

            public void AddPlayedGame(PlayedGame playerWhoPlayed)
            {
                playerWhoPlayed.Id = ExecuteInsert("INSERT INTO PlayedGame (playerId) VALUES (?);", new object[]
                {
                    playerWhoPlayed.PlayerId
                });
            }

            public long GetCountFromPlayer(long playerId)
            {
                return ExecuteScallar("SELECT COUNT(*) FROM PlayedGame WHERE playerId = ?;", new object[] { playerId });
            }
        }
    }
}