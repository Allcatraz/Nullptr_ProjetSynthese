using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;
using Prototype.NetworkLobby;

namespace ProjetSynthese
{
    //BEN_CORRECTION : Ceci est un parfait exemple d'Aspect.
    //
    //                 Si le joueur meurt ==> Alors le Lobby doit être affiché.
    //
    //                 Donc, encore une fois, ce n'est pas un contrôleur...en dehors du nom, cette classe 
    //                 est très bien ainsi. En fait, évitez de tout nommer "Controlleur". Comme ça, vous 
    //                 aurez moins tendance à faire des "God Class".
    //                 
    //                 Ceci pourrait s'appeler "ShowLobbyOnPlayerDeath". Simple non ?
   
    
    public class EndGameUIController : GameScript
    {
        //BEN_CORRECTION : private...
        PlayerDeathEventChannel playerDeathChannel;

        private void InjectEndGameUIController([EventChannelScope] PlayerDeathEventChannel playerDeathChannel)
        {
            this.playerDeathChannel = playerDeathChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectEndGameUIController");
            playerDeathChannel.OnEventPublished += OnEnd;
        }

        private void OnEnd(PlayerDeathEvent playerDeathEvent)
        {
            //BEN_CORRECTION : LobbyTopPanel devrait être obtenu à l'injection.
            LobbyTopPanel topPanel = GameObject.FindGameObjectWithTag(R.S.Tag.TopPanel).GetComponent<LobbyTopPanel>();
            topPanel.ToggleVisibility(true);
        }

        private void OnDestroy()
        {
            playerDeathChannel.OnEventPublished -= OnEnd;
        }
    }
}