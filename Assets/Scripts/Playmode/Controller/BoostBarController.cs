using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class BoostBarController : GameScript
    {
        [SerializeField]
        private Image image;
        private BoostStats boostStats;
        private float fillAmount;

        private void Start()
        {
            boostStats = StaticBoostPass.boostStats;
        }

        private void Update()
        {
            SetFillAmountFromBoost();
            UpdateBar();
        }

        private void SetFillAmountFromBoost()
        {
            if (boostStats != null)
            {
                fillAmount = boostStats.BoostPoints / boostStats.MaxBoostPoints;
            }
        }

        private void UpdateBar()
        {
            image.fillAmount = fillAmount;
        }
    }
}


