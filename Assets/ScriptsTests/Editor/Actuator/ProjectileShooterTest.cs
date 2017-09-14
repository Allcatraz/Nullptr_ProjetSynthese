using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace ProjetSynthese
{
    public class ProjectileShooterTest : UnitTestCase
    {
        private static readonly Vector3 SpawnPointsPosition = new Vector3(10, 15, 20);
        private static readonly Quaternion SpawnPointsRotation = new Quaternion(25, 30, 35,40);

        private GameObject bulletPrefab;
        private ITransform spawnPointTransform;
        private IPrefabFactory prefabFactory;
        private ProjectileShooter projectileShooter;

        [SetUp]
        public void Before()
        {
            bulletPrefab = CreateGameObject();
            prefabFactory = CreateSubstitute<IPrefabFactory>();
            projectileShooter = CreateBehaviour<ProjectileShooter>();

            spawnPointTransform = CreateSubstitute<ITransform>();
            spawnPointTransform.Position.Returns(SpawnPointsPosition);
            spawnPointTransform.Rotation.Returns(SpawnPointsRotation);
        }

        [Test]
        public void WhenFireringCreateNewBulletAtSpawnPoint()
        {
            Initialize();

            projectileShooter.Fire();

            CheckBulletCreatedAtSpawnPoint();
        }

        private void Initialize()
        {
            projectileShooter.InjectShipVehiculeFire(bulletPrefab, spawnPointTransform, prefabFactory);
            projectileShooter.Awake();
        }

        private void CheckBulletCreatedAtSpawnPoint()
        {
            prefabFactory.Received().Instantiate(bulletPrefab, SpawnPointsPosition, SpawnPointsRotation);
        }
    }
}