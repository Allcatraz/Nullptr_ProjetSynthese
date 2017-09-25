using System.Collections;
using UnityEngine;

namespace ProjetSynthese
{
    [CreateAssetMenu(fileName = "New Heal Effect", menuName = "Game/Effect/Heal")]
    public class Heal : Effect
    {
        [SerializeField]
        private int durationInSeconds;

        [SerializeField]
        private int healPointsPerSecond;

        public override void ApplyOn(GameScript effectExecutor, GameObject topParent)
        {
            Health health = topParent.GetComponentInChildren<Health>();
            effectExecutor.StartCoroutine(ApplyOnRoutine(health));
        }

        private IEnumerator ApplyOnRoutine(Health health)
        {
            float healEndTime = Time.time + durationInSeconds;
            while (Time.time < healEndTime)
            {
                health.Heal(healPointsPerSecond);
                yield return new WaitForSeconds(1);
            }
        }
    }
}