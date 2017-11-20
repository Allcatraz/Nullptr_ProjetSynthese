using System.Collections.Generic;
using Harmony;
using UnityEngine;
namespace ProjetSynthese
{
    public class AchivementController : GameScript
    {
        [SerializeField] private string[] achivementName;
        [SerializeField] private Player player;
        private ProtectionOfPlayerRepository protectionOfPlayerRepository;
        private AchivementRepository achivementRepository;
        private AiKillRepository aiKillRepository;
        private PlayerKillrepository playerKillrepository;
        private PlayerRepository playerRepository;
        private bool firstVictory;
        private bool firstWeapon;

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

        public void TriggerFirstWeapon()
        {
            if (firstWeapon != true)
            {
                Achivement achivement = new Achivement
                {
                    Name = achivementName[0],
                    PlayerId = player.Id
                };
                achivementRepository.AddAchivement(achivement);
                TriggerFeedBackAchivement(achivement);
                firstWeapon = true;
            }
        }

        public void TriggerFirstVictory()
        {
            if (firstVictory != true)
            {
                Achivement achivement = new Achivement
                {
                    Name = achivementName[4],
                    PlayerId = player.Id
                };
                achivementRepository.AddAchivement(achivement);
                TriggerFeedBackAchivement(achivement);
                firstVictory = true;
                
            }
        }

        private void CheckAchivements()
        {
            CheckPlayerKillAchivement();
            CheckAiKillAchivement();
            CheckProtectionOfPlayerAchivement();
        }

        private void CheckPlayerKillAchivement()
        {
            Achivement achivement = achivementRepository.GetAchivementFromName(new Achivement
            {
                Name = achivementName[3],
                PlayerId = player.Id
            });
            if (achivement == null)
            {
                long countPlayerKill = playerKillrepository.GetCountFromMurderer(player.Id);
                if (countPlayerKill <= 70)
                {
                    Achivement achivementToAdd = new Achivement
                    {
                        Name = achivementName[3],
                        PlayerId = player.Id
                    };
                    achivementRepository.AddAchivement(achivementToAdd);
                    TriggerFeedBackAchivement(achivementToAdd);
                }
            }
        }

        private void CheckAiKillAchivement()
        {
            Achivement achivement = achivementRepository.GetAchivementFromName(new Achivement
                                                                                {
                                                                                    Name = achivementName[2],
                                                                                    PlayerId = player.Id
                                                                                });
            if (achivement == null)
            {
                long countAiKill = aiKillRepository.GetAiKillCountFromMurderer(player.Id);
                if (countAiKill <= 50)
                {
                    Achivement achivementToAdd = new Achivement
                                                    {
                                                        Name = achivementName[2],
                                                        PlayerId = player.Id
                                                    };
                    achivementRepository.AddAchivement(achivementToAdd);
                    TriggerFeedBackAchivement(achivementToAdd);

                }
            }
        }

        private void CheckProtectionOfPlayerAchivement()
        {
            Achivement achivement = achivementRepository.GetAchivementFromName(new Achivement
            {
                Name = achivementName[1],
                PlayerId = player.Id
            });
            if (achivement == null)
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
                        Achivement achivementToAdd = new Achivement
                        {
                            Name = achivementName[1],
                            PlayerId = player.Id
                        };
                        achivementRepository.AddAchivement(achivementToAdd);
                        TriggerFeedBackAchivement(achivementToAdd);
                    }
                }
            }      
        }

        private void TriggerFeedBackAchivement(Achivement achivement)
        {

        }
    }
}