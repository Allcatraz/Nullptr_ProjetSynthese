using Harmony;

namespace ProjetSynthese
{
    public class PlayerMoveEvent : IEvent
    {
        public PlayerMover PlayerMover { get; private set; }

        public PlayerMoveEvent(PlayerMover playerMover)
        {
            PlayerMover = playerMover;
        }
    }
}
