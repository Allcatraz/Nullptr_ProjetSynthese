using System;
using Harmony.Testing;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Random Unity.
    /// </summary>
    /// <inheritdoc cref="IRandom"/>
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityRandom")]
    public class UnityRandom : IRandom
    {
        public Vector2 GetRandomPosition(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(GetRandomFloat(minX, maxX), GetRandomFloat(minY, maxY));
        }

        public Vector2 GetRandomPositionOnRectangleEdge(Vector2 center, float height, float width)
        {
            if (height < 0)
            {
                throw new ArgumentException("Rectangle Height cannot be negative.");
            }
            if (width < 0)
            {
                throw new ArgumentException("Rectangle Width cannot be negative.");
            }

            /*
             * Imagine a rectangle, like this :
             * 
             *    |---------------1---------------|
             *    |                               |
             *    |                               |
             *    |                               |
             *    4                               3
             *    |                               |
             *    |                               |
             *    |                               |
             *    |---------------2---------------|
             *    
             * We can "unfold" it in a line, like this :
             * 
             * |--------1--------|--------2--------|--------3--------|--------4--------|
             * 
             * By picking a random position on that line, we pick a random position on the rectangle edges.
             */

            int linePart = Mathf.RoundToInt(GetRandomFloat(0, 1) * 4);

            float randomPosition = GetRandomFloat(-1, 1);
            float x = linePart <= 2 ? randomPosition : (linePart == 3 ? 1 : -1);
            float y = linePart >= 3 ? randomPosition : (linePart == 1 ? 1 : -1);

            return new Vector2(x * (width / 2), y * (height / 2)) + center;
        }

        public Vector2 GetRandomDirection()
        {
            return GetRandomPosition(-1, 1, -1, 1).normalized;
        }

        public int GetOneOrMinusOneAtRandom()
        {
            return Random.value > 0.5f ? 1 : -1;
        }

        public uint GetRandomUInt(uint min, uint max)
        {
            return (uint) GetRandomFloat(min, max);
        }

        public int GetRandomInt(int min, int max)
        {
            return (int) GetRandomFloat(min, max);
        }

        public float GetRandomFloat(float min, float max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum value must be smaller or equal to maximum value.");
            }
            return Random.value * (max - min) + min;
        }
    }
}