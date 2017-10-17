﻿using System;
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

        public List<GameObject> GetAllItems(Transform transformPlayer)
        {
            List<Collider> itemsRaycast = Physics.OverlapSphere(transformPlayer.position, 10, 1 << LayerMask.NameToLayer(R.S.Layer.Item)).ToList();
            return itemsRaycast.ConvertAll(item => item.transform.gameObject);
        }

        public List<GameObject> GetAllItems()
        {
            List<Collider> itemsRaycast = Physics.OverlapSphere(sensor.transform.position, 10, 1 << LayerMask.NameToLayer(R.S.Layer.Item)).ToList();
            return itemsRaycast.ConvertAll(item => item.transform.gameObject);
        }

        public GameObject GetItemNearest()
        {
            List<GameObject> items = GetAllItems();
			items.Sort ((x, y) => Vector3.Distance(transform.position, x.transform.position).CompareTo(Vector3.Distance(transform.position, y.transform.position)));
            if (items.Count >= 1)
                return items[0];
            return null;
        }
    }
}
