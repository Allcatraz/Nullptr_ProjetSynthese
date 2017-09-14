using System.Collections.Generic;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un PolygonCollider2D Unity.
    /// </summary>
    public class UnityPolygonCollider2D : UnityCollider2D, IPolygonCollider2D
    {
        private readonly PolygonCollider2D polygonCollider2D;

        public UnityPolygonCollider2D(PolygonCollider2D polygonCollider2D) : base(polygonCollider2D)
        {
            this.polygonCollider2D = polygonCollider2D;
        }

        public IList<Vector2> GetPoints()
        {
            return polygonCollider2D.GetPath(0);
        }

        public void SetPoints(IList<Vector2> points)
        {
            Vector2[] pointsArray = new Vector2[points.Count];
            for (var i = 0; i < pointsArray.Length; i++)
            {
                pointsArray[i] = points[i];
            }
            polygonCollider2D.SetPath(0, pointsArray);
        }
    }
}