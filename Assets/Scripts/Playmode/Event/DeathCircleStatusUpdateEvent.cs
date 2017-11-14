using Harmony;
namespace ProjetSynthese
{
    public class DeathCircleStatusUpdateEvent : IEvent
    {
        public DeathCircleController DeathCircleController { get; private set; }

        public DeathCircleStatusUpdateEvent(DeathCircleController deathCircleController)
        {
            DeathCircleController = deathCircleController;
        }
    }
}