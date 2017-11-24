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

        [Tooltip("InputField contenant les informations du nom du compte a créer.")]
        [SerializeField]
        private InputField nameInput;

        [Tooltip("InputField contenant les informations du mot de passe du compte a créer.")]
        [SerializeField]
        private InputField passwordInput;

        private AchivementController achivementController;
        private PlayerRepository playerRepository;

        private void InjectConnectButton([ApplicationScope] PlayerRepository playerRepository,
                                            [ApplicationScope] AchivementController achivementController)
        {
            this.playerRepository = playerRepository;
            this.achivementController = achivementController;
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
            Player playerToConnect = ExtractPlayerFromInputField();
            Player playerWithNameInDatabase = playerRepository.GetPlayerFromName(playerToConnect);
            if (playerWithNameInDatabase == null)
            {
                FeedbackNoUserWithTheName();
            }
            else if (playerToConnect.Password == playerWithNameInDatabase.Password)
            {
                FeedbackSuccesfulConnection(playerToConnect);
            }
            else
            {
                FeedbackWrongPassword();
            }
        }

        private void FeedbackSuccesfulConnection(Player player)
        {
            //Feedback temporaire : barre d'erreur?
            nameInput.text = "User succesfuly connected";
            achivementController.SetPlayer(player);
        }

        private void FeedbackNoUserWithTheName()
        {
            //Feedback temporaire : barre d'erreur?
            nameInput.text = "No user with this name detected";
        }

        private void FeedbackWrongPassword()
        {
            //Feedback temporaire : barre d'erreur?
            nameInput.text = "Wrong Password";
        }

        private Player ExtractPlayerFromInputField()
        {
            Player player = new Player
            {
                Name = nameInput.text,
                Password = passwordInput.text
            };
            return player;
        }
    }
}
