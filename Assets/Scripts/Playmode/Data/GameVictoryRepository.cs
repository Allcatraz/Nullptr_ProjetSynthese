using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/GameVictoryRepository")]
    public class GameVictoryRepository : GameScript
    {

        private Repository repository;

        public void InjectGameVictoryRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new GameVictoryMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectGameVictoryRepository");
        }

        public virtual void AddGameVictory(long playerId)
        {
            repository.AddGameVictory(new GameVictory { PlayerId = playerId });
        }

        public virtual long GetCountFromPlayer(long playerId)
        {
            return repository.GetCountFromPlayer(playerId);
        }

        private class Repository : DbRepository<GameVictory>
        {
            private readonly GameVictoryRepository gameVictoryRepository;

            public Repository(GameVictoryRepository gameVictoryRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<GameVictory> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.gameVictoryRepository = gameVictoryRepository;
            }

            public void AddGameVictory(GameVictory playerWhoPlayed)
            {
                playerWhoPlayed.Id = ExecuteInsert("INSERT INTO GameVictory (playerId) VALUES (?);", new object[]
                {
                    playerWhoPlayed.PlayerId
                });
            }

            public long GetCountFromPlayer(long playerId)
            {
                return ExecuteScallar("SELECT COUNT(*) FROM GameVictory WHERE playerId = ?;", new object[] { playerId });
            }
        }
    }
}