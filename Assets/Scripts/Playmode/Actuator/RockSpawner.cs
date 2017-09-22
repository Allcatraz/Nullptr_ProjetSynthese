using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/RockSpawner")]
    public class RockSpawner : GameScript
    {
        private const float TriangleAngleSumInRad = 180 * Mathf.Deg2Rad;

        [SerializeField]
        private GameObject rockPrefab;

        [SerializeField]
        private float minRockRadius;

        [SerializeField]
        private float maxRockRadius;

        [SerializeField]
        private float initialForce;

        [SerializeField]
        private float explosionForce;

        private ScreenPositionProvider positionProvider;

        private void InjectRockSpawner([ApplicationScope] ScreenPositionProvider positionProvider)
        {
            this.positionProvider = positionProvider;
        }

        private void Awake()
        {
            InjectDependencies("InjectRockSpawner");
        }

        public void SpawnNormals(uint nbRocks)
        {
            //Bug : They sometinmes spawn over each other, because of the random positionning.
            for (int i = 0; i < nbRocks; i++)
            {
                //Allways spawn rocks at the same distance from the screen border, but give them a random position.
                Vector2 rockPosition = positionProvider.GetRandomOffScreenPosition(maxRockRadius);
                Vector2 rockDirection = positionProvider.GetRandomInScreenPosition() - rockPosition;
                float rockRadius = RandomExtensions.GetRandomFloat(minRockRadius, maxRockRadius);

                GameObject rock = Instantiate(rockPrefab,
                                              rockPosition,
                                              Quaternion.Euler(Vector3.zero));

                Configure(rock,
                          rockPosition,
                          rockRadius,
                          rockDirection,
                          initialForce);
            }
        }

        public void SpawnFragments(uint nbFragments, Vector3 parentRockPosition, float parentRockRadius)
        {
            if (parentRockRadius > minRockRadius)
            {
                //Rocks fragments are disposed in a circle around the parent rock

                //**************************
                //Fragment Radius
                //**************************
                //
                //We consider the parent rock as a square to simplify.
                //First, calculate his area (Height * Height), and then divide it by the number of fragments to get fragments area.
                //Then, compute fragments area squared root to get fragments height.
                //Divide it by 2 to get fragments radius.

                float parentRockDiameter = parentRockRadius * 2;
                float fragmentDiameter = Mathf.Sqrt(parentRockDiameter * parentRockDiameter / nbFragments);
                float fragmentRadius = fragmentDiameter / 2;

                //****************************************************
                //Distance between parent rock and fragments
                //****************************************************
                //
                //Simple trigonometry, using the Law of Sines
                //
                //             a
                //       B _________ C
                //         \       /
                //          \     /          a       b       c
                //         c \   / b       ----- = ----- = -----  where A,B,C are angles and a,b,c are lenghts
                //            \ /          Sin A   Sin B   Sin C
                //             V
                //             A
                //
                //We want fragments to be disposed in a circle around the parent. Thus, we can devide the circle equaly like this.
                //              a
                //          _________
                //         /\       /\
                //        /  \     /  \ 
                //       /  b \ A /    \
                //      /      \ /      \
                //      |------ * ------|
                //      \      / \      /
                //       \    /   \    /
                //        \  /     \  / 
                //         \/_______\/
                //
                // 
                //We want "b" : 
                //
                // A = The angle between each fragment
                // a = The fragment diameter
                // B = (180 - A) / 2
                // b = Sin B * a / Sin A

                float angleBetweenFragments = 360f / nbFragments;
                float angleBetweenFragmentsInRad = angleBetweenFragments * Mathf.Deg2Rad;
                float distanceBetweenParentAndFragment =
                    Mathf.Sin((TriangleAngleSumInRad - angleBetweenFragmentsInRad) / 2f) *
                    (fragmentDiameter / Mathf.Sin(angleBetweenFragmentsInRad));

                //******************************************************************************
                //With all that information, we can now create the rock fragments
                //******************************************************************************

                Vector3 baseFragmentPosition = parentRockPosition + Vector3.up * distanceBetweenParentAndFragment;
                for (int i = 0; i < nbFragments; i++)
                {
                    Vector3 fragmentPosition = baseFragmentPosition.RotateAround(parentRockPosition, z: angleBetweenFragments * i);
                    Vector3 fragmentDirection = fragmentPosition - parentRockPosition;

                    GameObject rock = Instantiate(rockPrefab,
                                                  fragmentPosition,
                                                  Quaternion.Euler(Vector3.zero));
                    Configure(rock,
                              fragmentPosition,
                              fragmentRadius,
                              fragmentDirection,
                              explosionForce);
                }
            }
        }

        private void Configure(GameObject rock, Vector3 position, float radius, Vector3 direction, float force)
        {
            rock.transform.position = position;
            rock.transform.rotation = Quaternion.Euler(Vector3.zero);

            RockController rockController = rock.GetComponentInChildren<RockController>();
            rockController.Configure(radius, direction, force);
        }
    }
}