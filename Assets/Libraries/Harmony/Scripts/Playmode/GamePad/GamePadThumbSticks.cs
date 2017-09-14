namespace Harmony
{
    public struct GamePadThumbSticks
    {
        public GamePadThumbSticks(StickValue left, StickValue right) : this()
        {
            Left = left;
            Right = right;
        }

        public static implicit operator GamePadThumbSticks(XInputDotNetPure.GamePadThumbSticks gamePadThumbSticks)
        {
            return new GamePadThumbSticks(gamePadThumbSticks.Left,
                                          gamePadThumbSticks.Right);
        }

        public static GamePadThumbSticks operator +(GamePadThumbSticks gamePadThumbSticks1, GamePadThumbSticks gamePadThumbSticks2)
        {
            return new GamePadThumbSticks(gamePadThumbSticks1.Left + gamePadThumbSticks2.Left,
                                          gamePadThumbSticks1.Right + gamePadThumbSticks2.Right);
        }

        public StickValue Left { get; private set; }
        public StickValue Right { get; private set; }

        public struct StickValue
        {
            public StickValue(float x, float y) : this()
            {
                X = x;
                Y = y;
            }

            public static implicit operator StickValue(XInputDotNetPure.GamePadThumbSticks.StickValue stickValue)
            {
                return new StickValue(stickValue.X,
                                      stickValue.Y);
            }

            public static StickValue operator +(StickValue stickValue1, StickValue stickValue2)
            {
                return new StickValue(stickValue1.X + stickValue2.X,
                                      stickValue1.Y + stickValue2.Y);
            }

            public float X { get; private set; }
            public float Y { get; private set; }
        }
    }
}