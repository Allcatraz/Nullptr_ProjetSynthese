using XInputDotNetPure;

namespace Harmony
{
    /// <summary>
    /// Structure représentant l'état des boutons d'un GamePad, capturé à un instant précis.
    /// </summary>
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

        /// <summary>
        /// Indique si le bouton Start est appuyé ou non.
        /// </summary>
        public ButtonState Start { get; private set; }

        /// <summary>
        /// Indique si le bouton Back est appuyé ou non.
        /// </summary>
        public ButtonState Back { get; private set; }

        /// <summary>
        /// Indique si le bouton LeftStick est appuyé ou non.
        /// </summary>
        public ButtonState LeftStick { get; private set; }

        /// <summary>
        /// Indique si le bouton RightStick est appuyé ou non.
        /// </summary>
        public ButtonState RightStick { get; private set; }

        /// <summary>
        /// Indique si le bouton LeftShoulder est appuyé ou non.
        /// </summary>
        public ButtonState LeftShoulder { get; private set; }

        /// <summary>
        /// Indique si le bouton RightShoulder est appuyé ou non.
        /// </summary>
        public ButtonState RightShoulder { get; private set; }

        /// <summary>
        /// Indique si le bouton Guide est appuyé ou non.
        /// </summary>
        public ButtonState Guide { get; private set; }

        /// <summary>
        /// Indique si le bouton A est appuyé ou non.
        /// </summary>
        public ButtonState A { get; private set; }

        /// <summary>
        /// Indique si le bouton B est appuyé ou non.
        /// </summary>
        public ButtonState B { get; private set; }

        /// <summary>
        /// Indique si le bouton X est appuyé ou non.
        /// </summary>
        public ButtonState X { get; private set; }

        /// <summary>
        /// Indique si le bouton Y est appuyé ou non.
        /// </summary>
        public ButtonState Y { get; private set; }
    }
}