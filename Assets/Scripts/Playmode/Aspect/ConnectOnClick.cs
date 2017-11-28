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

        private void InjectConnectButton([ApplicationScope] AchivementController achivementController)
        {
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
            int feedback = achivementController.CheckPlayerExistAndAddToDatabse(playerToConnect);
            if (feedback == 1)
            {
                FeedbackNoUserWithTheName();
            }
            else if (feedback == 2)
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
