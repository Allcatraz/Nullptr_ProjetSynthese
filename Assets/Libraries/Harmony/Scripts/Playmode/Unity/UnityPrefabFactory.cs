using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un PrefabFactory Unity.
    /// </summary>
    /// <inheritdoc cref="IPrefabFactory" />
    [NotTested(Reason.Wrapper)]
    [AddComponentMenu("Game/Utils/UnityPrefabFactory")]
    public class UnityPrefabFactory : IPrefabFactory
    {
        public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(prefab, position, rotation);
        }

        public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, GameObject parent)
        {
            GameObject gameObject = Instantiate(prefab, position, rotation);
            gameObject.transform.SetParent(parent.transform);
            return gameObject;
        }
    }
}