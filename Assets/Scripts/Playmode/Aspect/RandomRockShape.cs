using System.Collections.Generic;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/RandomRockShape")]
    public class RandomRockShape : GameScript
    {
        [SerializeField]
        [Range(1, 10)]
        private float radius = 1f;

        [SerializeField]
        [Range(0, 100)]
        private int roughness = 10;

        [SerializeField]
        [Range(0, 360)]
        private int minAngleBetweenSegments = 30;

        [SerializeField]
        [Range(0, 360)]
        private int maxAngleBetweenSegments = 50;

        private LineRenderer lineRenderer;
        private PolygonCollider2D colisionCollider2D;
        private PolygonCollider2D hitboxTrigger2D;

        public virtual float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                GenerateRockShape();
            }
        }

        private void InjectRandomRockShape([GameObjectScope] LineRenderer lineRenderer,
                                          [GameObjectScope] PolygonCollider2D colisionCollider2D,
                                          [Named(R.S.GameObject.Hitbox)] [ChildScope] PolygonCollider2D hitboxTrigger2D)
        {
            this.lineRenderer = lineRenderer;
            this.colisionCollider2D = colisionCollider2D;
            this.hitboxTrigger2D = hitboxTrigger2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectRandomRockShape");
        }

        private void Start()
        {
            lineRenderer.loop = true;

            GenerateRockShape();
        }

        private void GenerateRockShape()
        {
            //Rock generation is pretty basic. It is generated clockwise just like a circle.
            //
            //First, a random distance from the origin is generated. Then, we apply a random rotation angle
            //to it. We keep that angle.
            //
            //We then generate an other random distance from the origin. We add the previous rotation angle to it
            //and an other random angle.
            //
            //We keep like this until we reach 360 degrees. Then, the Rock is generated.

            IList<Vector3> rockPoints = new List<Vector3>();
            IList<Vector2> rockColisionPoints = new List<Vector2>();
            float currentAngleInDegrees = 0;
            while (currentAngleInDegrees < 360)
            {
                float randomDistance = RandomExtensions.GetRandomFloat(100 - roughness, 100) * radius / 100;

                Vector3 rockPoint = new Vector3(0, randomDistance, transform.position.z).RotateAround(Vector3.zero, z: currentAngleInDegrees);

                rockPoints.Add(rockPoint);
                rockColisionPoints.Add(rockPoint);

                currentAngleInDegrees += RandomExtensions.GetRandomFloat(minAngleBetweenSegments, maxAngleBetweenSegments);
            }
            SetPointsToLineRenderer(rockPoints);
            SetPointsOfColisionCollider(rockColisionPoints);
            SetPointsOfHitboxTrigger(rockColisionPoints);
        }

        private void SetPointsToLineRenderer(IList<Vector3> points)
        {
            lineRenderer.positionCount = points.Count;
            for (var i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }

        private void SetPointsOfColisionCollider(IList<Vector2> points)
        {
            Vector2[] pointsArray = new Vector2[points.Count];
            for (var i = 0; i < pointsArray.Length; i++)
            {
                pointsArray[i] = points[i];
            }
            colisionCollider2D.SetPath(0, pointsArray);
        }

        private void SetPointsOfHitboxTrigger(IList<Vector2> points)
        {
            Vector2[] pointsArray = new Vector2[points.Count];
            for (var i = 0; i < pointsArray.Length; i++)
            {
                pointsArray[i] = points[i];
            }
            hitboxTrigger2D.SetPath(0, pointsArray);
        }
    }
}