using System.Collections.Generic;
using NUnit.Framework;

namespace Harmony.Util
{
    public class ListExtensionsTest
    {
        [Test]
        public void CanFilterList()
        {
            FilteredType obj1 = new FilteredType {Value = 1};
            FilteredType obj2 = new FilteredType {Value = 2};
            IList<FilteredType> listToFilter = new List<FilteredType>(new[] {obj1, obj2});

            IList<FilteredType> filteredList = listToFilter.Filter(item => item.Value > 1);

            Assert.AreSame(obj2, filteredList[0]);
            Assert.AreEqual(1, filteredList.Count);
        }

        [Test]
        public void CanConvertListIfItsTypeInheritsFromTheTargetType()
        {
            InheritedType obj1 = new InheritedType();
            InheritedType obj2 = new InheritedType();
            IList<InheritedType> listToConvert = new List<InheritedType>(new[] {obj1, obj2});

            IList<ParentType> convertedList = listToConvert.Convert<InheritedType, ParentType>();

            Assert.AreSame(obj1, convertedList[0]);
            Assert.AreSame(obj2, convertedList[1]);
        }

        private class FilteredType
        {
            public int Value { get; set; }
        }

        private class ParentType
        {
        }

        private class InheritedType : ParentType
        {
        }
    }
}