using System;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void HitSensorEventHandler(int hitPoints);

    [AddComponentMenu("Game/Sensor/HitSensor")]
    public class HitSensor : GameScript
    {
        private new Collider2D collider2D;

        public event HitSensorEventHandler OnHit;

        private void InjectHitSensor([GameObjectScope] Collider2D collider2D)
        {
            this.collider2D = collider2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectHitSensor");

            int layer = LayerMask.NameToLayer(R.S.Layer.HitSensor);
            if (layer == -1)
            {
                throw new Exception("In order to use a HitSensor, you must have a " + R.S.Layer.HitSensor + " layer.");
            }
            gameObject.layer = layer;
            collider2D.isTrigger = true;
        }

        public void Hit(int hitPoints)
        {
            if (OnHit != null) OnHit(hitPoints);
        }
    }
}