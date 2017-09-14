namespace Harmony
{
    public struct GamePadTriggers
    {
        public GamePadTriggers(float left, float right) : this()
        {
            Left = left;
            Right = right;
        }

        public static implicit operator GamePadTriggers(XInputDotNetPure.GamePadTriggers gamePadTriggers)
        {
            return new GamePadTriggers(gamePadTriggers.Left,
                                       gamePadTriggers.Right);
        }

        public static GamePadTriggers operator +(GamePadTriggers gamePadTriggers1, GamePadTriggers gamePadTriggers2)
        {
            return new GamePadTriggers(gamePadTriggers1.Left + gamePadTriggers2.Left,
                                       gamePadTriggers1.Right + gamePadTriggers2.Right);
        }

        public float Left { get; private set; }
        public float Right { get; private set; }
    }
}