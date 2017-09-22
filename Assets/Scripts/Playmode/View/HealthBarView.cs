using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/View/HealthBarView")]
    public class HealthBarView : GameScript
    {
        public void SetHealthPercentage(float percentage)
        {
            transform.localScale = new Vector3(percentage, transform.localScale.y, transform.localScale.z);
        }
    }
}