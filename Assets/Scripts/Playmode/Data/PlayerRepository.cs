using System.Collections.Generic;
using Harmony;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Data/PlayerRepository")]
    public class PlayerRepository : GameScript
    {
        private Repository repository;

        public void InjectPlayerRepository([ApplicationScope] IDbConnectionFactory connectionFactory,
                                              [ApplicationScope] IDbParameterFactory parameterFactory)
        {
            repository = new Repository(this, connectionFactory, parameterFactory, new PlayerMapper());
        }

        public void Awake()
        {
            InjectDependencies("InjectPlayerRepository");
        }

        public virtual void AddPlayer(Player player)
        {
            repository.AddPlayer(player);
        }

        public virtual Player GetPlayerFromName(Player player)
        {
            return repository.GetPlayerFromName(player.Name);
        }

        public virtual Player GetPlayerFromNameAndPassword(Player player)
        {
            return repository.GetPlayerFromNameAndPassword(player.Name, player.Password);
        }

        public virtual Player GetPlayerFromId(Player player)
        {
            return repository.GetPlayerFromId(player.Id);
        }

        public virtual void DeletePlayerAtId(Player player)
        {
            repository.DeletePlayerAtId(player.Id);
        }

        public virtual long Count()
        {
            return repository.Count();
        }

        private class Repository : DbRepository<Player>
        {
            private readonly PlayerRepository playerRepository;

            public Repository(PlayerRepository playerRepository,
                              [NotNull] IDbConnectionFactory connectionFactory,
                              [NotNull] IDbParameterFactory parameterFactory,
                              [NotNull] IDbDataMapper<Player> dataMapper)
                : base(connectionFactory, parameterFactory, dataMapper)
            {
                this.playerRepository = playerRepository;
            }

            public void AddPlayer(Player player)
            {
                player.Id = ExecuteInsert("INSERT INTO Player (name, password) VALUES (?,?);", new object[]
                {
                    player.Name,
                    player.Password
                });
            }

            public Player GetPlayerFromName(string name)
            {
                return ExecuteSelectOne("SELECT * FROM Player WHERE name = ?;", new object[] {name});
            }

            public Player GetPlayerFromId(long Id)
            {
                return ExecuteSelectOne("SELECT * FROM Player WHERE id = ?;", new object[] { Id });
            }

            public Player GetPlayerFromNameAndPassword(string name, string password)
            {
                return ExecuteSelectOne("SELECT * FROM Player WHERE name = ? AND password = ?;", new object[] {name, password});
            }

            public void DeletePlayerAtId(long Id)
            {
                ExecuteDelete("DELETE FROM Player WHERE id = ?",
                              new object[] { Id });
            }

            public long Count()
            {
                return ExecuteScallar("SElECT COUNT(*) FROM Player;", new object[] { });
            }
        }
    }
}