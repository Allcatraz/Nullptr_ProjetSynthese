using System.Collections.Generic;
using Harmony.Testing;
using NUnit.Framework;
using UnityEngine;

namespace Harmony.Util
{
    public class VectorExtensionsTest : UnitTestCase
    {
        private Vector3TestComparer vector3TestComparer;

        [SetUp]
        public void Before()
        {
            vector3TestComparer = new Vector3TestComparer(0.01f);
        }

        [Test]
        public void CanRotatePointAroundPivot()
        {
            //Around 0,0,0
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(0, 0, 0), z: 0), Is.EqualTo(new Vector3(1, 2, 3)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 0, 0).RotateAround(new Vector3(0, 0, 0), z: 90), Is.EqualTo(new Vector3(0, 1, 0)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 0, 0).RotateAround(new Vector3(0, 0, 0), z: 180), Is.EqualTo(new Vector3(-1, 0, 0)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 0, 0).RotateAround(new Vector3(0, 0, 0), z: 270), Is.EqualTo(new Vector3(0, -1, 0)).Using(vector3TestComparer));

            //Around some pivot (X)
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), 0), Is.EqualTo(new Vector3(1, 2, 3)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), x: 90), Is.EqualTo(new Vector3(1, -1, 2)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), x: 180), Is.EqualTo(new Vector3(1, 0, -1)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), x: 270), Is.EqualTo(new Vector3(1, 3, 0)).Using(vector3TestComparer));

            //Around some pivot (Y)
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), 0), Is.EqualTo(new Vector3(1, 2, 3)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), y: 90), Is.EqualTo(new Vector3(3, 2, 1)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), y: 180), Is.EqualTo(new Vector3(1, 2, -1)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), y: 270), Is.EqualTo(new Vector3(-1, 2, 1)).Using(vector3TestComparer));

            //Around some pivot (Z)
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), 0), Is.EqualTo(new Vector3(1, 2, 3)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), z: 90), Is.EqualTo(new Vector3(0, 1, 3)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), z: 180), Is.EqualTo(new Vector3(1, 0, 3)).Using(vector3TestComparer));
            Assert.That(new Vector3(1, 2, 3).RotateAround(new Vector3(1, 1, 1), z: 270), Is.EqualTo(new Vector3(2, 1, 3)).Using(vector3TestComparer));


            Assert.That(new Vector3(0, 0, 0).RotateAround(new Vector3(1, 1, 1), 0, 0, 0), Is.EqualTo(new Vector3(0, 0, 0)).Using(vector3TestComparer));
        }

        private class Vector3TestComparer : IEqualityComparer<Vector3>
        {
            private readonly float range;

            public Vector3TestComparer(float range)
            {
                this.range = range;
            }

            public bool Equals(Vector3 left, Vector3 right)
            {
                return Mathf.Abs(left.x - right.x) < range && Mathf.Abs(left.y - right.y) < range && Mathf.Abs(left.z - right.z) < range;
            }

            public int GetHashCode(Vector3 obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}