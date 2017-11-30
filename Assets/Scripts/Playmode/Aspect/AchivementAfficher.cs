using Harmony;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class AchivementAfficher : GameScript
    {
        [Tooltip("L'emplacement ou il faut mettre les objets à créer.")]
        [SerializeField] private RectTransform grid;
        [Tooltip("Le prefab représentant les achivements du joueur.")]
        [SerializeField] private GameObject achivementViewPrefab;
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
            SpawnAchivement();
        }

        public void OnDisable()
        {
            ClearGrid();
        }

        private void ClearGrid()
        {
            foreach (Transform child in grid)
            {
                Destroy(child.gameObject);
            }
        }

        private void SpawnAchivement()
        {
            Player player = achivementController.GetPlayer();
            if (player != null)
            {
                IList<Achivement> allAchivement = achivementRepository.GetAchivementFromPlayerId(player);
                foreach (Achivement achiv in allAchivement)
                {
                    GameObject gameObject = Instantiate(achivementViewPrefab);
                    gameObject.transform.SetParent(grid, false);
                    gameObject.GetComponentInChildren<Text>().text = achiv.Name;
                    Vector2 size = grid.sizeDelta;
                    size.y += 172.7f;
                    grid.sizeDelta = size;
                }
            }
        }
    }
}