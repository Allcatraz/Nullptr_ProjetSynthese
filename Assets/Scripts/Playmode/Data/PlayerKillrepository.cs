using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/PlayerKillRepository")]
    public class PlayerKillrepository : GameScript
    {
        private Repository repository;

        public void InjectPlayerKillRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new PlayerKillMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayerKillRepository");
        }

        public void AddPlayerKill(PlayerKill playerKillWithPlayerWhoDidTheDeed)
        {
            repository.AddPlayerKill(playerKillWithPlayerWhoDidTheDeed);
        }

        public long GetCountFromMurderer(long playerId)
        {
            return repository.GetCountFromMurderer(playerId);
        }

        private class Repository : DbRepository<PlayerKill>
        {
            private readonly PlayerKillrepository playerKillRepository;

            public Repository(PlayerKillrepository playerKillRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<PlayerKill> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.playerKillRepository = playerKillRepository;
            }

            public void AddPlayerKill(PlayerKill playerKillWithPlayerWhoDidTheDeed)
            {
                playerKillWithPlayerWhoDidTheDeed.Id = ExecuteInsert("INSERT INTO PlayerKill (playerId) VALUES (?);", new object[]
                {
                    playerKillWithPlayerWhoDidTheDeed.PlayerId
                });
            }

            public long GetCountFromMurderer(long playerId)
            {
                return ExecuteScallar("SELECT COUNT(*) FROM PlayerKill WHERE playerId = ?;", new object[] { playerId });
            }
        }
    }
}