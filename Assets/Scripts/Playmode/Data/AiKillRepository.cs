using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/AiKillRepository")]
    public class AiKillRepository : GameScript
    {
        private Repository repository;

        public void InjectAiKillRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new AiKillMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectAiKillRepository");
        }

        public virtual void AddAiKill(long playerId)
        {
            repository.AddAiKill(new AiKill { PlayerId = playerId });
        }

        public virtual long GetAiKillCountFromMurderer(long playerId)
        {
            return repository.GetCountFromMurderer(playerId);
        }

        private class Repository : DbRepository<AiKill>
        {
            private readonly AiKillRepository aiKillRepository;

            public Repository(AiKillRepository aiKillRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<AiKill> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.aiKillRepository = aiKillRepository;
            }
            
            public void AddAiKill(AiKill aiKillWithPlayerWhoDidTheDeed)
            {
                aiKillWithPlayerWhoDidTheDeed.Id = ExecuteInsert("INSERT INTO AiKill (playerId) VALUES (?);", new object[]
                {
                    aiKillWithPlayerWhoDidTheDeed.PlayerId
                });
            }

            public long GetCountFromMurderer(long playerId)
            {
                return ExecuteScallar("SELECT COUNT(*) FROM AiKill WHERE playerId = ?;", new object[] { playerId });
            }
        }
    }
}