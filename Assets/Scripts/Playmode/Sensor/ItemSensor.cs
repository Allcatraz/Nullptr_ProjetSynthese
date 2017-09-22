using System;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void ItemSensorEventHandler(Effect effect);

    [AddComponentMenu("Game/Sensor/ItemSensor")]
    public class ItemSensor : GameScript
    {
        private new Collider2D collider2D;

        public event ItemSensorEventHandler OnCollectItem;

        private void InjectItemSensor([GameObjectScope] Collider2D collider2D)
        {
            this.collider2D = collider2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectItemSensor");

            int layer = LayerMask.NameToLayer(R.S.Layer.ItemSensor);
            if (layer == -1)
            {
                throw new Exception("In order to use a ItemSensor, you must have a " + R.S.Layer.ItemSensor + " layer.");
            }
            gameObject.layer = layer;
            collider2D.isTrigger = true;
        }

        public void CollectItem(Effect effect)
        {
            if (OnCollectItem != null) OnCollectItem(effect);
        }
    }
}