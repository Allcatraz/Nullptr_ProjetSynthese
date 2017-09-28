using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public class ItemSensor : GameScript
    {
        private Transform sensor;

        private void InjectItemSensor([GameObjectScope] Transform sensor)
        {
            this.sensor = sensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectItemSensor");
        }

        public List<GameObject> GetAllItems()
        {
            List<RaycastHit> itemsRaycast = Physics.SphereCastAll(sensor.transform.position, 10, Vector3.down).ToList();
            itemsRaycast.RemoveAll(item => !item.transform.gameObject.GetComponent<Item>());
            return itemsRaycast.ConvertAll(item => item.transform.gameObject);
        }

        public GameObject GetItemNearest()
        {
            List<GameObject> items = GetAllItems();
            if (items.Count >= 1)
                return GetAllItems()[0];
            return null;
        }
    }
}
