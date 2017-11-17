using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class InteractableSensor : GameScript
    {
        [Tooltip("Distance que le joueur pourra détecter les objects intéractible autour de lui.")]
        [SerializeField]
        private float radius;

        private Transform sensor;

        private void InjectInteractableSensor([GameObjectScope] Transform sensor)
        {
            this.sensor = sensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectInteractableSensor");
        }

        public List<GameObject> GetAllInteractible()
        {
            List<Collider> interactibleColliders = Physics.OverlapSphere(sensor.position, radius, (1 << LayerMask.NameToLayer(R.S.Layer.Item)) | 
                                                                                                  (1 << LayerMask.NameToLayer(R.S.Layer.Interactible))).ToList();
            return interactibleColliders.ConvertAll(item => item.gameObject);
        }

        public GameObject GetNearestInteractible()
        {
            List<GameObject> interactibles = GetAllInteractible();
            interactibles.Sort((x, y) => Vector3.Distance(transform.position, x.transform.position)
                                                .CompareTo(Vector3.Distance(transform.position, y.transform.position)));

            if (interactibles.Count >= 1)
                return interactibles[0];

            return null;
        }
    }
}