using Harmony;
using ProjetSynthese;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : GameScript
    {
        private AchivementController achivementController;

        public LobbyManager lobbyManager;

        public RectTransform lobbyPanel;

        public InputField ipInput;

        public void Awake()
        {
            InjectDependencies("InjectLobbyMainMenu");
        }

        private void InjectLobbyMainMenu([ApplicationScope] AchivementController achivementController)
        {
            this.achivementController = achivementController;
        }

        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);
        }

        public void OnClickHost()
        {
            if (achivementController.GetPlayer() != null)
            {
                lobbyManager.StartHost();
            }           
        }

        public void OnClickJoin()
        {
            if (achivementController.GetPlayer() != null)
            {
                lobbyManager.ChangeTo(lobbyPanel);

                lobbyManager.networkAddress = ipInput.text;
                lobbyManager.StartClient();

                lobbyManager.backDelegate = lobbyManager.StopClientClbk;
                lobbyManager.DisplayIsConnecting();

                lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
            }
        }

        public void OnClickDedicated()
        {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();

            lobbyManager.backDelegate = lobbyManager.StopServerClbk;

            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        void onEndEditIP(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }
    }
}
