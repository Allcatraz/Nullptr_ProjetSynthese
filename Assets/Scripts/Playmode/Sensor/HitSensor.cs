using System;
using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void HitSensorEventHandler(int hitPoints);

    [AddComponentMenu("Game/World/Object/Sensor/HitSensor")]
    public class HitSensor : GameScript
    {
        private new ICollider2D collider2D;

        public virtual event HitSensorEventHandler OnHit;

        public void InjectHitSensor([GameObjectScope] ICollider2D collider2D)
        {
            this.collider2D = collider2D;
        }

        public void Awake()
        {
            InjectDependencies("InjectHitSensor");

            int layer = LayerMask.NameToLayer(R.S.Layer.HitSensor);
            if (layer == -1)
            {
                throw new Exception("In order to use a HitSensor, you must have a " + R.S.Layer.HitSensor + " layer.");
            }
            gameObject.layer = layer;
            collider2D.IsTrigger = true;
        }

        public virtual void Hit(int hitPoints)
        {
            if (OnHit != null) OnHit(hitPoints);
        }
    }
}