using System.Data.Common;
using Harmony.Testing;
using NUnit.Framework;

namespace ProjetSynthese
{
    public class SqLiteParameterFactoryTest : UnitTestCase
    {
        private SqLiteParameterFactory parameterFactory;

        [SetUp]
        public void Before()
        {
            parameterFactory = CreateBehaviour<SqLiteParameterFactory>();
        }

        [Test]
        public void ReturnsNewDbParameterWithProvidedValue()
        {
            DbParameter parameter = parameterFactory.GetParameter(1337);

            Assert.AreEqual(1337, parameter.Value);
        }
    }
}