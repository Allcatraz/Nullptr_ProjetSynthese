using XInputDotNetPure;

namespace Harmony
{
    public struct GamePadButtons
    {
        public GamePadButtons(ButtonState start,
                              ButtonState back,
                              ButtonState leftStick,
                              ButtonState rightStick,
                              ButtonState leftShoulder,
                              ButtonState rightShoulder,
                              ButtonState guide,
                              ButtonState a,
                              ButtonState b,
                              ButtonState x,
                              ButtonState y) : this()
        {
            Start = start;
            Back = back;
            LeftStick = leftStick;
            RightStick = rightStick;
            LeftShoulder = leftShoulder;
            RightShoulder = rightShoulder;
            Guide = guide;
            A = a;
            B = b;
            X = x;
            Y = y;
        }

        public static implicit operator GamePadButtons(XInputDotNetPure.GamePadButtons gamePadButtons)
        {
            return new GamePadButtons(gamePadButtons.Start,
                                      gamePadButtons.Back,
                                      gamePadButtons.LeftStick,
                                      gamePadButtons.RightStick,
                                      gamePadButtons.LeftShoulder,
                                      gamePadButtons.RightShoulder,
                                      gamePadButtons.Guide,
                                      gamePadButtons.A,
                                      gamePadButtons.B,
                                      gamePadButtons.X,
                                      gamePadButtons.Y);
        }

        public static GamePadButtons operator +(GamePadButtons gamePadButtons1, GamePadButtons gamePadButtons2)
        {
            return
                new GamePadButtons(
                    gamePadButtons1.Start == ButtonState.Pressed || gamePadButtons2.Start == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.Back == ButtonState.Pressed || gamePadButtons2.Back == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.LeftStick == ButtonState.Pressed || gamePadButtons2.LeftStick == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.RightStick == ButtonState.Pressed || gamePadButtons2.RightStick == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.LeftShoulder == ButtonState.Pressed || gamePadButtons2.LeftShoulder == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.RightShoulder == ButtonState.Pressed || gamePadButtons2.RightShoulder == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.Guide == ButtonState.Pressed || gamePadButtons2.Guide == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.A == ButtonState.Pressed || gamePadButtons2.A == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.B == ButtonState.Pressed || gamePadButtons2.B == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.X == ButtonState.Pressed || gamePadButtons2.X == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadButtons1.Y == ButtonState.Pressed || gamePadButtons2.Y == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released);
        }

        public ButtonState Start { get; private set; }
        public ButtonState Back { get; private set; }
        public ButtonState LeftStick { get; private set; }
        public ButtonState RightStick { get; private set; }
        public ButtonState LeftShoulder { get; private set; }
        public ButtonState RightShoulder { get; private set; }
        public ButtonState Guide { get; private set; }
        public ButtonState A { get; private set; }
        public ButtonState B { get; private set; }
        public ButtonState X { get; private set; }
        public ButtonState Y { get; private set; }
    }
}