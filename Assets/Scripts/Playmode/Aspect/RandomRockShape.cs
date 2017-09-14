using System.Collections.Generic;
using Harmony;
using Harmony.Util;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Object/Aspect/RandomRockShape")]
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

        private ILineRenderer lineRenderer;
        private IPolygonCollider2D colisionCollider2D;
        private IPolygonCollider2D hitboxTrigger2D;
        private IRandom random;

        public void InjectRandomRockShape(float radius,
                                          int roughness,
                                          int minAngleBetweenSegments,
                                          int maxAngleBetweenSegments,
                                          [GameObjectScope] ILineRenderer lineRenderer,
                                          [GameObjectScope] IPolygonCollider2D colisionCollider2D,
                                          [Named(R.S.GameObject.Hitbox)] [ChildScope] IPolygonCollider2D hitboxTrigger2D,
                                          [ApplicationScope] IRandom random)
        {
            this.radius = radius;
            this.roughness = roughness;
            this.minAngleBetweenSegments = minAngleBetweenSegments;
            this.maxAngleBetweenSegments = maxAngleBetweenSegments;
            this.lineRenderer = lineRenderer;
            this.colisionCollider2D = colisionCollider2D;
            this.hitboxTrigger2D = hitboxTrigger2D;
            this.random = random;
        }

        public void Awake()
        {
            InjectDependencies("InjectRandomRockShape",
                               radius,
                               roughness,
                               minAngleBetweenSegments,
                               maxAngleBetweenSegments);
        }

        public void Start()
        {
            lineRenderer.Loop = true;

            GenerateRockShape();
        }

        public virtual float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                GenerateRockShape();
            }
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
                float randomDistance = random.GetRandomFloat(100 - roughness, 100) * radius / 100;

                Vector3 rockPoint = new Vector3(0, randomDistance, transform.position.z).RotateAround(Vector3.zero, z: currentAngleInDegrees);

                rockPoints.Add(rockPoint);
                rockColisionPoints.Add(rockPoint);

                currentAngleInDegrees += random.GetRandomFloat(minAngleBetweenSegments, maxAngleBetweenSegments);
            }
            lineRenderer.SetPoints(rockPoints);
            colisionCollider2D.SetPoints(rockColisionPoints);
            hitboxTrigger2D.SetPoints(rockColisionPoints);
        }
    }
}