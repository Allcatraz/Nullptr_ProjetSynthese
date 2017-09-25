using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/ExecuteEffectOnCollect")]
    public class ExecuteEffectOnCollect : GameScript
    {
        private GameObject topParentGameObject;
        private ItemSensor itemSensor;

        private void InjectExecuteEffectOnCollect([TopParentScope] GameObject topParentGameObject,
                                                  [EntityScope] ItemSensor itemSensor)
        {
            this.topParentGameObject = topParentGameObject;
            this.itemSensor = itemSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectExecuteEffectOnCollect");
        }

        private void OnEnable()
        {
            itemSensor.OnCollectItem += OnCollectItem;
        }

        private void OnDisable()
        {
            itemSensor.OnCollectItem -= OnCollectItem;
        }

        private void OnCollectItem(Effect effect)
        {
            effect.ApplyOn(this, topParentGameObject);
        }
    }
}