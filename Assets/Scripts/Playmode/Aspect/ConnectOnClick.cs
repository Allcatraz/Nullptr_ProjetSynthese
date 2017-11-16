using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/ConnectOnClick")]
    public class ConnectOnClick : GameScript
    {
        [Tooltip("Button connect.")]
        [SerializeField] private Button button;

        private PlayerRepository playerRepository;

        private void InjectConnectButton([ApplicationScope] PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public void Awake()
        {
            InjectDependencies("InjectConnectButton");
        }

        private void OnEnable()
        {
            button.Events().OnClick += OnClick;
        }

        private void OnDisable()
        {
            button.Events().OnClick -= OnClick;
        }

        private void OnClick()
        {
            
        }
    }
}
