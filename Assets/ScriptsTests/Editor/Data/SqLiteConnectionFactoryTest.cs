using System.Data.Common;
using Harmony;
using Harmony.Testing;
using NSubstitute;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class SqLiteConnectionFactoryTest : UnitTestCase
    {
        private IApplication application;
        private SqLiteConnectionFactory connectionFactory;

        [SetUp]
        public void Before()
        {
            application = CreateSubstitute<IApplication>();
            connectionFactory = CreateBehaviour<SqLiteConnectionFactory>();

            application.ApplicationDataPath.Returns("App\\Data");
        }

        [Test]
        public void ReturnsNewDbDbConnectionToProvidedFile()
        {
            Initialize();

            DbConnection connection = connectionFactory.GetConnection();

            //This test will only work on Windows, because URL concatenation is platform dependant.
            Assert.AreEqual("URI=file:App\\Data\\Database\\File.db", connection.ConnectionString);
        }

        private void Initialize()
        {
            connectionFactory.InjectSqLiteConnectionFactory("Database\\File.db", application);
            connectionFactory.Awake();
        }
    }
}