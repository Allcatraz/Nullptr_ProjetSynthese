using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class HealthBarController : GameScript
    {
        [SerializeField]
        private Image image;

        private Health health;

        private float fillAmount;

        private void Start()
        {
            health = StaticHealthPass.health;
        }

        private void Update()
        {
            UpdateHelth();
            SetFillAmountFromHealth();
            UpdateBar();
        }

        private void UpdateHelth()
        {
            health = StaticHealthPass.health;
        }

        private void SetFillAmountFromHealth()
        {
            if (health != null)
            {
                fillAmount = health.HealthPoints / health.MaxHealthPoints;
            }
        }

        private void UpdateBar()
        {
            image.fillAmount = fillAmount;
        }
    }
}

