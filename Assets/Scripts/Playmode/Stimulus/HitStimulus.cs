using System;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void HitStimulusEventHandler(int hitPoints);

    [AddComponentMenu("Game/Stimulus/HitStimulus")]
    public class HitStimulus : GameScript
    {
        [SerializeField]
        private int hitPoints;

        private new Collider2D collider2D;

        public event HitStimulusEventHandler OnHit;

        private void InjectHitStimulus([GameObjectScope] Collider2D collider2D)
        {
            this.collider2D = collider2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectHitStimulus");

            int layer = LayerMask.NameToLayer(R.S.Layer.HitSensor);
            if (layer == -1)
            {
                throw new Exception("In order to use a HitStimulus, you must have a " + R.S.Layer.HitSensor + " layer.");
            }
            gameObject.layer = layer;
            collider2D.isTrigger = true;
        }

        private void OnEnable()
        {
            collider2D.Events().OnEnterTrigger += OnEnterTrigger;
        }

        private void OnDisable()
        {
            collider2D.Events().OnEnterTrigger -= OnEnterTrigger;
        }

        private void OnEnterTrigger(Collider2D other)
        {
            HitSensor hitSensor = other.GetComponent<HitSensor>();
            if (hitSensor != null)
            {
                hitSensor.Hit(hitPoints);

                if (OnHit != null) OnHit(hitPoints);
            }
        }
    }
}