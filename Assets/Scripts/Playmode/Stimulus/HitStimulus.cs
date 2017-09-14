using System;
using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void HitStimulusEventHandler(int hitPoints);

    [AddComponentMenu("Game/World/Object/Stimulus/HitStimulus")]
    public class HitStimulus : GameScript
    {
        [SerializeField]
        private int hitPoints;

        private new ICollider2D collider2D;

        public virtual event HitStimulusEventHandler OnHit;

        public void InjectHitStimulus(int hitPoints,
                                      [GameObjectScope] ICollider2D collider2D)
        {
            this.hitPoints = hitPoints;
            this.collider2D = collider2D;
        }

        public void Awake()
        {
            InjectDependencies("InjectHitStimulus", hitPoints);

            int layer = LayerMask.NameToLayer(R.S.Layer.HitSensor);
            if (layer == -1)
            {
                throw new Exception("In order to use a HitStimulus, you must have a " + R.S.Layer.HitSensor + " layer.");
            }
            gameObject.layer = layer;
            collider2D.IsTrigger = true;
            collider2D.OnTriggerEntered += OnTriggerEntered;
        }

        public void OnDestroy()
        {
            collider2D.OnTriggerEntered -= OnTriggerEntered;
        }

        private void OnTriggerEntered(ICollider2D other)
        {
            HitSensor hitSensor = other.GetOtherComponent<HitSensor>();
            if (hitSensor != null)
            {
                hitSensor.Hit(hitPoints);

                if (OnHit != null) OnHit(hitPoints);
            }
        }
    }
}