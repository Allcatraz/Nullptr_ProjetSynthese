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

        public virtual IList<ProtectionOfPlayer> GetAllLevel2Entry()
        {
            return repository.GetAllLevel2Entry();
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel1Entry()
        {
            return repository.GetAllLevel1Entry();
        }

        public virtual IList<ProtectionOfPlayer> GetAllEntryOfPlayer(long playerId)
        {
            return repository.GetAllEntryOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel3EntryOfPlayer(long playerId)
        {
            return repository.GetAllLevel3EntryOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel2EntryOfPlayer(long playerId)
        {
            return repository.GetAllLevel2EntryOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel1EntryOfPlayer(long playerId)
        {
            return repository.GetAllLevel1EntryOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel1HelmetOfPlayer(long playerId)
        {
            return repository.GetAllLevel1HelmetOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel2HelmetOfPlayer(long playerId)
        {
            return repository.GetAllLevel2HelmetOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel3HelmetOfPlayer(long playerId)
        {
            return repository.GetAllLevel3HelmetOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel1VestOfPlayer(long playerId)
        {
            return repository.GetAllLevel1VestOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel2VestOfPlayer(long playerId)
        {
            return repository.GetAllLevel2VestOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel3VestOfPlayer(long playerId)
        {
            return repository.GetAllLevel3VestOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel1BagOfPlayer(long playerId)
        {
            return repository.GetAllLevel1BagOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel2BagOfPlayer(long playerId)
        {
            return repository.GetAllLevel2BagOfPlayer(playerId);
        }

        public virtual IList<ProtectionOfPlayer> GetAllLevel3BagOfPlayer(long playerId)
        {
            return repository.GetAllLevel3BagOfPlayer(playerId);
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

            public IList<ProtectionOfPlayer> GetAllLevel2Entry()
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE levelProtection = ?;", new object[] { 2 });
            }

            public IList<ProtectionOfPlayer> GetAllLevel1Entry()
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE levelProtection = ?;", new object[] { 1 });
            }

            public IList<ProtectionOfPlayer> GetAllEntryOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ?;", new object[] { playerId });
            }

            public IList<ProtectionOfPlayer> GetAllLevel3EntryOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ?;", new object[] { playerId, 3 });
            }

            public IList<ProtectionOfPlayer> GetAllLevel2EntryOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ?;", new object[] { playerId, 2 });
            }

            public IList<ProtectionOfPlayer> GetAllLevel1EntryOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ?;", new object[] { playerId, 1 });
            }

            public IList<ProtectionOfPlayer> GetAllLevel1HelmetOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] {playerId, 1, ItemType.Helmet.ToString()});
            }

            public IList<ProtectionOfPlayer> GetAllLevel2HelmetOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 2, ItemType.Helmet.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel3HelmetOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 3, ItemType.Helmet.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel1VestOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 1, ItemType.Vest.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel2VestOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 2, ItemType.Vest.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel3VestOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 3, ItemType.Vest.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel1BagOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 1, ItemType.Bag.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel2BagOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 2, ItemType.Bag.ToString() });
            }

            public IList<ProtectionOfPlayer> GetAllLevel3BagOfPlayer(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM ProtectionOfPlayer WHERE playerid = ? AND levelProtection = ? AND typeProtection = ?;", new object[] { playerId, 3, ItemType.Bag.ToString() });
            }

            public long Count()
            {
                return ExecuteScallar("SElECT COUNT(*) FROM ProtectionOfPlayer;", new object[] { });
            }
        }
    }
}