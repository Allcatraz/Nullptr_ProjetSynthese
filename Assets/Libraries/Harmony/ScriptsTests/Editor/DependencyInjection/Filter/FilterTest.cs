using Harmony.Testing;
using NUnit.Framework;

namespace Harmony.Injection
{
    public class FilterTest : UnitTestCase
    {
        private FilterForTest filter;

        [SetUp]
        public void Before()
        {
            filter = new FilterForTest();
        }

        [Test]
        public void CanFilterAccordingToFromChildClasses()
        {
            filter.valueToReturn = true;
            Assert.AreEqual(1, filter.FilterDependencies(new[] {new object()}).Count);

            filter.valueToReturn = false;
            Assert.AreEqual(0, filter.FilterDependencies(new[] {new object()}).Count);
        }

        private class FilterForTest : Filter
        {
            public bool valueToReturn;

            protected override bool FilterDependency(object dependency)
            {
                return valueToReturn;
            }
        }
    }
}