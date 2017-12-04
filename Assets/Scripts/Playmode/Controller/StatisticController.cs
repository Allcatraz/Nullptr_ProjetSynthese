using System;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class StatisticController : GameScript
    {
        [Tooltip("")]
        [SerializeField]
        private Text playerKills;

        [Tooltip("")]
        [SerializeField]
        private Text aiKills;

        [Tooltip("")]
        [SerializeField]
        private Text totalKills;

        [Tooltip("")]
        [SerializeField]
        private Text numberVictory;

        [Tooltip("")]
        [SerializeField]
        private Text numberDefeat;

        [Tooltip("")]
        [SerializeField]
        private Text totalGame;

        [Tooltip("")]
        [SerializeField]
        private Text killDeathsAverage;

        [Tooltip("")]
        [SerializeField]
        private Text killDeathsRatio;

        [Tooltip("")]
        [SerializeField]
        private Text militaryHelmet;

        [Tooltip("")]
        [SerializeField]
        private Text militaryVest;

        [Tooltip("")]
        [SerializeField]
        private Text rucksac;

        [Tooltip("")]
        [SerializeField]
        private Text poliveHelmet;

        [Tooltip("")]
        [SerializeField]
        private Text swatVest;

        [Tooltip("")]
        [SerializeField]
        private Text schoolBag;

        [Tooltip("")]
        [SerializeField]
        private Text motoHelmet;

        [Tooltip("")]
        [SerializeField]
        private Text poliveVest;

        [Tooltip("")]
        [SerializeField]
        private Text pouchBag;

        [Tooltip("")]
        [SerializeField]
        private Text totalLevel3;

        [Tooltip("")]
        [SerializeField]
        private Text totalLevel2;

        [Tooltip("")]
        [SerializeField]
        private Text totalLevel1;

        private PlayerKillrepository playerKillrepository;
        private AiKillRepository aiKillRepository;
        private ProtectionOfPlayerRepository protectionOfPlayerRepository;
        private AchivementController achivementController;

        private void InjectStatisticController([ApplicationScope] PlayerKillrepository playerKillrepository,
                                               [ApplicationScope] AiKillRepository aiKillRepository,
                                               [ApplicationScope] ProtectionOfPlayerRepository protectionOfPlayerRepository,
                                               [ApplicationScope] AchivementController achivementController)
        {
            this.playerKillrepository = playerKillrepository;
            this.aiKillRepository = aiKillRepository;
            this.protectionOfPlayerRepository = protectionOfPlayerRepository;
            this.achivementController = achivementController;
        }

        private void Awake()
        {
            InjectDependencies("InjectStatisticController");
        }

        private void OnEnable()
        {
            Player player = achivementController.GetPlayer();
            if (player != null)
            {
                long playerId = Convert.ToInt64(player.Id);
                int killsPlayer = Convert.ToInt32(playerKillrepository.GetCountFromMurderer(playerId));
                int killsAi = Convert.ToInt32(aiKillRepository.GetAiKillCountFromMurderer(playerId));
                int killTotal = killsAi + killsPlayer;

                playerKills.text = killsPlayer.ToString();
                aiKills.text = killsAi.ToString();
                totalKills.text = killTotal.ToString();

                int victory = Convert.ToInt32(achivementController.GetGameWonFromPlayer());
                int defeat = Convert.ToInt32(achivementController.GetGamePlayedFromPlayer()) - victory;
                int totalGames = Convert.ToInt32(achivementController.GetGamePlayedFromPlayer());

                numberVictory.text = victory.ToString();
                numberDefeat.text = defeat.ToString();
                totalGame.text = totalGames.ToString();

                float killDeathRatio = defeat != 0 ? (float)killTotal / defeat : killTotal;
                float killDeathAverage = totalGames != 0 ? (float)killTotal / totalGames : killTotal;

                killDeathsRatio.text = killDeathRatio.ToString();
                killDeathsAverage.text = killDeathAverage.ToString();

                int level3Helmet = protectionOfPlayerRepository.GetAllLevel3HelmetOfPlayer(playerId).Count;
                int level3Vest = protectionOfPlayerRepository.GetAllLevel3VestOfPlayer(playerId).Count;
                int level3Bag = protectionOfPlayerRepository.GetAllLevel3BagOfPlayer(playerId).Count;

                militaryHelmet.text = level3Helmet.ToString();
                militaryVest.text = level3Vest.ToString();
                rucksac.text = level3Bag.ToString();

                int level2Helmet = protectionOfPlayerRepository.GetAllLevel2HelmetOfPlayer(playerId).Count;
                int level2Vest = protectionOfPlayerRepository.GetAllLevel2VestOfPlayer(playerId).Count;
                int level2Bag = protectionOfPlayerRepository.GetAllLevel2BagOfPlayer(playerId).Count;

                poliveHelmet.text = level2Helmet.ToString();
                swatVest.text = level2Vest.ToString();
                schoolBag.text = level2Bag.ToString();

                int level1Helmet = protectionOfPlayerRepository.GetAllLevel1HelmetOfPlayer(playerId).Count;
                int level1Vest = protectionOfPlayerRepository.GetAllLevel1VestOfPlayer(playerId).Count;
                int level1Bag = protectionOfPlayerRepository.GetAllLevel1BagOfPlayer(playerId).Count;

                motoHelmet.text = level1Helmet.ToString();
                poliveVest.text = level1Vest.ToString();
                pouchBag.text = level1Bag.ToString();

                int allLevel3 = protectionOfPlayerRepository.GetAllLevel3EntryOfPlayer(playerId).Count;
                int allLevel2 = protectionOfPlayerRepository.GetAllLevel2EntryOfPlayer(playerId).Count;
                int allLevel1 = protectionOfPlayerRepository.GetAllLevel1EntryOfPlayer(playerId).Count;

                totalLevel3.text = allLevel3.ToString();
                totalLevel2.text = allLevel2.ToString();
                totalLevel1.text = allLevel1.ToString();
            }
        }
    }
}