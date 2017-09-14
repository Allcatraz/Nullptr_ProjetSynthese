using System.Collections.Generic;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un LineRenderer Unity.
    /// </summary>
    public class UnityLineRenderer : UnityRenderer, ILineRenderer
    {
        private readonly LineRenderer lineRenderer;

        public UnityLineRenderer(LineRenderer lineRenderer) : base(lineRenderer)
        {
            this.lineRenderer = lineRenderer;
        }

        public bool Loop
        {
            get { return lineRenderer.loop; }
            set { lineRenderer.loop = value; }
        }

        public IList<Vector3> GetPoints()
        {
            Vector3[] pointsArray = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(pointsArray);
            return pointsArray;
        }

        public void SetPoints(IList<Vector3> points)
        {
            lineRenderer.positionCount = points.Count;
            for (var i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }
}