using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class AchivementController : GameScript
    {
        [SerializeField] private string[] achivementName;
        [SerializeField] private GameObject feedBackPrefab;
        private Player player;
        private ProtectionOfPlayerRepository protectionOfPlayerRepository;
        private AchivementRepository achivementRepository;
        private AiKillRepository aiKillRepository;
        private PlayerKillrepository playerKillrepository;
        private PlayerRepository playerRepository;

        public void InjectAchivementController([ApplicationScope] PlayerRepository playerRepository,
                                               [ApplicationScope] ProtectionOfPlayerRepository protectionOfPlayerRepository,
                                               [ApplicationScope] PlayerKillrepository playerKillrepository,
                                               [ApplicationScope] AiKillRepository aiKillRepository,
                                               [ApplicationScope] AchivementRepository achivementRepository)
        {
            this.playerRepository = playerRepository;
            this.playerKillrepository = playerKillrepository;
            this.protectionOfPlayerRepository = protectionOfPlayerRepository;
            this.aiKillRepository = aiKillRepository;
            this.achivementRepository = achivementRepository;
        }

        public void Awake()
        {
            InjectDependencies("InjectAchivementController");
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public void TriggerFirstWeapon()
        {
            if (player != null)
            {
                Achivement achivement = new Achivement
                {
                    Name = achivementName[0],
                    PlayerId = player.Id
                };
                if (!CheckIfAchivementExist(achivement))
                {
                    achivementRepository.AddAchivement(achivement);
                    TriggerFeedBackAchivement(achivement);
                }
            }
        }

        public void TriggerFirstVictory()
        {
            if (player != null)
            {
                Achivement achivement = new Achivement
                {
                    Name = achivementName[4],
                    PlayerId = player.Id
                };
                if (!CheckIfAchivementExist(achivement))
                {
                    achivementRepository.AddAchivement(achivement);
                    TriggerFeedBackAchivement(achivement);
                }
            }  
        }

        public void CheckAchivements()
        {
            if (player != null)
            {
                CheckPlayerKillAchivement();
                CheckAiKillAchivement();
                CheckProtectionOfPlayerAchivement();
            }
        }

        private void CheckPlayerKillAchivement()
        {
            Achivement achivement = new Achivement
            {
                Name = achivementName[3],
                PlayerId = player.Id
            };
            if (!CheckIfAchivementExist(achivement))
            {
                long countPlayerKill = playerKillrepository.GetCountFromMurderer(player.Id);
                if (countPlayerKill >= 70)
                {
                    achivementRepository.AddAchivement(achivement);
                    TriggerFeedBackAchivement(achivement);
                }
            }
        }

        private void CheckAiKillAchivement()
        {
            Achivement achivement = new Achivement
            {
                Name = achivementName[2],
                PlayerId = player.Id
            };
            if (!CheckIfAchivementExist(achivement))
            {
                long countAiKill = aiKillRepository.GetAiKillCountFromMurderer(player.Id);
                if (countAiKill >= 50)
                {
                    achivementRepository.AddAchivement(achivement);
                    TriggerFeedBackAchivement(achivement);

                }
            }
        }

        private void CheckProtectionOfPlayerAchivement()
        {
            Achivement achivement = new Achivement
            {
                Name = achivementName[1],
                PlayerId = player.Id
            };
            if (!CheckIfAchivementExist(achivement))
            {
                IList<ProtectionOfPlayer> listAllProtection = protectionOfPlayerRepository.GetAllLevel3EntryOfPlayer(player.Id);
                if (listAllProtection != null)
                {
                    bool helmet = false;
                    bool vest = false;
                    bool bag = false;
                    foreach (ProtectionOfPlayer item in listAllProtection)
                    {
                        if (item.TypeProtection == ItemType.Bag.ToString())
                        {
                            bag = true;
                        }
                        else if (item.TypeProtection == ItemType.Helmet.ToString())
                        {
                            helmet = true;
                        }
                        else if (item.TypeProtection == ItemType.Vest.ToString())
                        {
                            vest = true;
                        }
                    }
                    if (helmet && bag && vest)
                    {
                        achivementRepository.AddAchivement(achivement);
                        TriggerFeedBackAchivement(achivement);
                    }
                }
            }      
        }

        private bool CheckIfAchivementExist(Achivement achivement)
        {
            IList<Achivement> achivementToCheck = achivementRepository.GetAchivementFromPlayerId(player);
            if (achivementToCheck != null)
            {
                foreach (Achivement item in achivementToCheck)
                {
                    if (item.Name == achivement.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void TriggerFeedBackAchivement(Achivement achivement)
        {
            GameObject gameObject = Instantiate(feedBackPrefab);
            gameObject.GetComponentInChildren<Text>().text = achivement.Name;
            gameObject.transform.SetParent(transform, false);
            Destroy(gameObject, 2.0f);
        }

        public void AddAiKill()
        {
            if (player != null)
            {
                aiKillRepository.AddAiKill(player.Id);
            }
        }

        public void AddPlayerKill()
        {
            if (player != null)
            {
                PlayerKill playerKill = new PlayerKill
                {
                    PlayerId = player.Id
                };
                playerKillrepository.AddPlayerKill(playerKill);
            }
        }
    }
}