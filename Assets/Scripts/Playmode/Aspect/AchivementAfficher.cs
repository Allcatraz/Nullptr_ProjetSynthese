using Harmony;
using UnityEngine;
using System.Collections.Generic;

namespace ProjetSynthese
{
    public class AchivementAfficher : GameScript
    {
        [SerializeField] private Transform grid;
        private AchivementController achivementController;
        private AchivementRepository achivementRepository;

        private void InjectAchivementAfficher([ApplicationScope] AchivementController achivementController,
                                              [ApplicationScope] AchivementRepository achivementRepository)
        {
            this.achivementController = achivementController;
            this.achivementRepository = achivementRepository;
        }

        public void Awake()
        {
            InjectDependencies("InjectAchivementAfficher");
        }

        public void OnEnable()
        {
            
        }

        public void OnDisable()
        {
            
        }

        private void ClearGrid()
        {
            
        }
    }
}