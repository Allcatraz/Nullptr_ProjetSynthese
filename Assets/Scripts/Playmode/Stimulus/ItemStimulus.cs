using System;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    public delegate void ItemStimulusEventHandler(Effect effect);

    [AddComponentMenu("Game/Stimulus/ItemStimulus")]
    public class ItemStimulus : GameScript
    {
        [SerializeField]
        private Effect effect;

        private new Collider2D collider2D;

        public event ItemStimulusEventHandler OnItemCollected;

        private void InjectItemStimulus([GameObjectScope] Collider2D collider2D)
        {
            this.collider2D = collider2D;
        }

        private void Awake()
        {
            InjectDependencies("InjectItemStimulus");

            int layer = LayerMask.NameToLayer(R.S.Layer.ItemSensor);
            if (layer == -1)
            {
                throw new Exception("In order to use a ItemStimulus, you must have a " + R.S.Layer.ItemSensor + " layer.");
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
            ItemSensor itemSensor = other.GetComponent<ItemSensor>();
            if (itemSensor != null)
            {
                itemSensor.CollectItem(effect);

                if (OnItemCollected != null) OnItemCollected(effect);
            }
        }
    }
}