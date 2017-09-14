using Harmony;
using Harmony.Injection;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/World/Ui/View/HealthBarView")]
    public class HealthBarView : GameScript
    {
        private new ITransform transform;

        public void InjectHealthBar([GameObjectScope] ITransform transform)
        {
            this.transform = transform;
        }

        public void Awake()
        {
            InjectDependencies("InjectHealthBar");
        }

        public virtual void SetHealthPercentage(float percentage)
        {
            transform.LocalScale = new Vector3(percentage, transform.LocalScale.y, transform.LocalScale.z);
        }
    }
}