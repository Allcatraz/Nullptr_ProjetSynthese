using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    class LastPlayerController : GameScript
    {
        private int nbCheck = 0;

        private void FixedUpdate()
        {
            if (NetworkManager.singleton.numPlayers <= 1 && LobbyManager.s_Singleton.AliveNumber <= 0 && LobbyManager.s_Singleton.topPanel.isInGame && nbCheck == 0)
            {               
                PlayerController lastPlayer = FindObjectOfType<PlayerController>();
                lastPlayer.WinGame();
                lastPlayer.OnDeath(new PlayerDeathEvent());
                nbCheck++; 
            }       
        }
    }
}
