using UnityEngine;

namespace ProjetSynthese
{
    public static class CrateFactory
    {
        public static GameObject Prefab { get; set; }

        public static GameObject Create(Vector3 position)
        {
            GameObject crate = Object.Instantiate(Prefab);
            crate.transform.position = position;
            return crate;
        }
    }
}