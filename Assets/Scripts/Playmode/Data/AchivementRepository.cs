using System.Collections.Generic;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/AchivementRepository")]
    public class AchivementRepository : GameScript
    {
        private Repository repository;

        public void InjectAchivementRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new AchivementMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectAchivementRepository");
        }

        public virtual Achivement GetAchivementFromName(Achivement achivement)
        {
            return repository.GetAchivementFromName(achivement.Name);
        }

        public virtual Achivement GetAchivementFromId(Achivement achivement)
        {
            return repository.GetAchivementFromId(achivement.Id);
        }

        public virtual IList<Achivement> GetAchivementFromPlayerId(Player player)
        {
            return repository.GetAllAchivementFromPlayerId(player.Id);
        }

        public virtual void DeleteAllAchivementAtPlayerId(Player player)
        {
            repository.DeleteAllAchivementAtPlayerId(player.Id);
        }

        public virtual void DeleteAllAchivementWithName(Achivement achivement)
        {
            repository.DeleteAllAchivementWithName(achivement.Name);
        }

        public virtual long Count()
        {
            return repository.Count();
        }

        private class Repository : DbRepository<Achivement>
        {
            private readonly AchivementRepository achivementRepository;

            public Repository(AchivementRepository achivementRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<Achivement> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.achivementRepository = achivementRepository;
            }

            public void AddAchivement(Achivement achivement)
            {
                achivement.Id = ExecuteInsert("INSERT INTO Achivement (name, playerId) VALUES (?,?);", new object[]
                {
                    achivement.Name,
                    achivement.PlayerId
                });
            }

            public Achivement GetAchivementFromName(string name)
            {
                return ExecuteSelectOne("SELECT * FROM Achivement WHERE name = ?;", new object[] { name });
            }

            public Achivement GetAchivementFromId(long Id)
            {
                return ExecuteSelectOne("SELECT * FROM Achivement WHERE id = ?;", new object[] { Id });
            }

            public IList<Achivement> GetAllAchivementFromPlayerId(long playerId)
            {
                return ExecuteSelectAll("SELECT * FROM Achivement WHERE playerId = ?;", new object[] { playerId });
            }

            public void DeleteAllAchivementAtPlayerId(long playerId)
            {
                ExecuteDelete("DELETE FROM Achivement WHERE playerId = ?",
                              new object[] { playerId });
            }

            public void DeleteAllAchivementWithName(string name)
            {
                ExecuteDelete("DELETE FROM Achivement WHERE name = ?",
                              new object[] { name });
            }

            public long Count()
            {
                return ExecuteScallar("SElECT COUNT(*) FROM Achivement;", new object[] { });
            }
        }
    }
}