using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Aspect/CreateAccountOnClick")]
    public class CreateAccountOnClick : GameScript
    {
        [Tooltip("Bouton qui crée l'account du joueur lorsque clické.")]
        [SerializeField]
        private Button button;

        [Tooltip("InputField contenant les informations du nom du compte a créer.")]
        [SerializeField]
        private InputField nameInput;

        [Tooltip("InputField contenant les informations du mot de passe du compte a créer.")]
        [SerializeField]
        private InputField passwordInput;

        private AchivementController achivementController;

        public void InjectCreateAccountButton([ApplicationScope] AchivementController achivementController)
        {
            this.achivementController = achivementController;
        }

        public void Awake()
        {
            InjectDependencies("InjectCreateAccountButton");
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
            Player playerToAdd = ExtractPlayerFromInputField();
            bool addSuccesful = achivementController.AddPlayerToDatabase(playerToAdd);
            if (addSuccesful)
            {
                FeedbackSuccesfulCreation();
            }
            else
            {
                FeedbackWrongName();
            }

        }

        private void FeedbackSuccesfulCreation()
        {
            //Feedback temporaire : barre d'erreur?
            nameInput.text = "User succesfuly created";
        }

        private void FeedbackWrongName()
        {
            //Feedback temporaire : barre d'erreur?
            nameInput.text = "User name invalid";
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
