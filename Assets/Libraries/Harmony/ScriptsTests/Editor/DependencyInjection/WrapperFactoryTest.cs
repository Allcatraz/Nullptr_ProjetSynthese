using System;
using Harmony.Testing;
using NUnit.Framework;

namespace Harmony.Injection
{
    public class WrapperFactoryTest : UnitTestCase
    {
        [Test]
        public void TellThatCantUseWrapperTypeThatIsNotAnInterface()
        {
            Assert.Throws<ArgumentException>(delegate
            {
                new WrapperFactory(typeof(IWrapped), typeof(Wrapper), dependency => new Wrapper());
            });
        }

        [Test]
        public void TellThatCanWrapWrappedType()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.True(wrapperFactory.CanWrap(typeof(IWrapped)));
        }

        [Test]
        public void TellThatCanWrapWrappedTypeImplementation()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.True(wrapperFactory.CanWrap(typeof(Wrapped)));
        }

        [Test]
        public void TellThatCanWrapWrappedTypeSubClass()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.True(wrapperFactory.CanWrap(typeof(WrappedSubClass)));
        }

        [Test]
        public void CanTellThatTypeIsNotAWrapperTypeImplementation()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.False(wrapperFactory.IsWrapperTypeImplementation(typeof(IWrapper)));
        }

        [Test]
        public void CanTellThatTypeIsAWrapperTypeImplementation()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.True(wrapperFactory.IsWrapperTypeImplementation(typeof(Wrapper)));
        }

        [Test]
        public void CanTellThatTypeIsAWrapperTypeSubType()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.True(wrapperFactory.IsWrapperTypeImplementation(typeof(WrapperSubClass)));
        }

        [Test]
        public void TellThatCanWrapIntoWrapperExactType()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.True(wrapperFactory.CanWrapInto(typeof(IWrapper)));
        }

        [Test]
        public void TellThatCantWrapIntoWrapperTypeImplementation()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.False(wrapperFactory.CanWrapInto(typeof(Wrapper)));
        }

        [Test]
        public void TellThatCantWrapIntoWrapperTypeSubtype()
        {
            WrapperFactory wrapperFactory = new WrapperFactory(typeof(IWrapped), typeof(IWrapper), dependency => new Wrapper());

            Assert.False(wrapperFactory.CanWrapInto(typeof(WrapperSubClass)));
        }

        private interface IWrapped
        {
        }

        private interface IWrappedSubClass : IWrapped
        {
        }

        private class Wrapped : IWrapped
        {
        }

        private class WrappedSubClass : Wrapped, IWrappedSubClass
        {
        }

        private interface IWrapper
        {
        }

        private interface IWrapperSubClass : IWrapper
        {
        }

        private class Wrapper : IWrapper
        {
        }

        private class WrapperSubClass : Wrapper, IWrapperSubClass
        {
        }
    }
}