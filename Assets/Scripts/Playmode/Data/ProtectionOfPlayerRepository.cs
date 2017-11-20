using System.Collections.Generic;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/ProtectionOfPlayerRepository")]
    public class ProtectionOfPlayerRepository : GameScript
    {
        private Repository repository;

        public void InjectProtectionOfPlayerRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new ProtectionOfPlayerMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectProtectionOfPlayerRepository");
        }

        public virtual void AddProtectionOfPlayer(ProtectionOfPlayer protectionOfPlayer)
        {
            repository.AddProtectionOfPlayer(protectionOfPlayer);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel3Entry()
        {
            return repository.GetAllLevel3Entry();
        }

        public virtual IList<ProtectionOfPlayer> GetAllEntryOfPlayer(long playerId)
        {
            return repository.GetAllEntryOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel3EntryOfPlayer(long playerId)
        {
            return repository.GetAllLevel3EntryOfPlayer(playerId);
        }

        public virtual long Count()
        {
            return repository.Count();
        }

        private class Repository : DbRepository<ProtectionOfPlayer>
        {
            private readonly ProtectionOfPlayerRepository protectionOfPlayerRepository;

            public Repository(ProtectionOfPlayerRepository protectionOfPlayerRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<ProtectionOfPlayer> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.protectionOfPlayerRepository = protectionOfPlayerRepository;
            }

            public void AddProtectionOfPlayer(ProtectionOfPlayer protectionOfPlayer)
            {
                protectionOfPlayer.Id = ExecuteInsert("INSERT INTO ProtectionOfPlayer (playerId, levelProtection, typeProtection) VALUES (?,?,?);", new object[]
                {
                    protectionOfPlayer.PlayerId,
                    protectionOfPlayer.LevelProtection,
                    protectionOfPlayer.TypeProtection
                });
            }

            public IList<ProtectionOfPlayer> GetAllLevel3Entry()
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE levelProtection = ?;", new object[] { 3 });
            }

            public IList<ProtectionOfPlayer> GetAllEntryOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ?;", new object[] { playerId });
            }

            public IList<ProtectionOfPlayer> GetAllLevel3EntryOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ?;", new object[] { playerId, 3 });
            }

            public long Count()
            {
                return ExecuteScallar("SElECT COUNT(*) FROM ProtectionOfPlayer;", new object[] { });
            }
        }
    }
}