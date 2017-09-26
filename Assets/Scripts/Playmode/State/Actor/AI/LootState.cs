
using ProjetSynthese;

namespace NullSurvival
{
    public class LootState : StateMachine
    {

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        public override void Execute(Actor actor)
        {
            actor.ActorController.Move();
        }
    }
}