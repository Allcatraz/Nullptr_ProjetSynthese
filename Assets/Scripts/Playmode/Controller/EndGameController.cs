using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class EndGameController : GameScript
    {
        [Tooltip("Texte représentant le rank du joueur à la fin de la partie.")]
        [SerializeField]
        private Text rankPlayer;

        [Tooltip("Texte représentant le nombre de joueur restant dans la partie après la mort du joueur.")]
        [SerializeField]
        private Text numberPlayerLeft;

        [Tooltip("Texte représentant le nombre de joueur ou d'ai tué par le joueur mort.")]
        [SerializeField]
        private Text playerKill;

        private void OnEnable()
        {
            int total = CountAlive();
            int kill = GetKillLocalPlayer();

            rankPlayer.text = total > 9 ? (total + 1).ToString() : "0" + (total + 1);
            numberPlayerLeft.text = total > 9 ? total.ToString() : "0" + total;
            playerKill.text = kill > 9 ? kill.ToString() : "0" + kill;
        }

        private int CountAlive()
        {
            return FindObjectsOfType<ActorAI>().Length + FindObjectsOfType<PlayerController>().Length - 1;
        }

        private int GetKillLocalPlayer()
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            foreach (PlayerController player in players)
            {
                if (player.isLocalPlayer)
                {
                    return player.GetKill();
                }
            }

            return 0;
        }
    }
}