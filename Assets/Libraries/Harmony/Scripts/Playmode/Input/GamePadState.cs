namespace Harmony
{
    /// <summary>
    /// Structure représentant l'état d'un GamePad, capturé à un instant précis.
    /// </summary>
    public struct GamePadState
    {
        public GamePadState(bool isConnected,
                            GamePadButtons buttons,
                            GamePadDPad dPad,
                            GamePadTriggers triggers,
                            GamePadThumbSticks thumbSticks) : this()
        {
            IsConnected = isConnected;
            Buttons = buttons;
            DPad = dPad;
            Triggers = triggers;
            ThumbSticks = thumbSticks;
        }

        public static implicit operator GamePadState(XInputDotNetPure.GamePadState gamePadState)
        {
            return new GamePadState(gamePadState.IsConnected,
                                    gamePadState.Buttons,
                                    gamePadState.DPad,
                                    gamePadState.Triggers,
                                    gamePadState.ThumbSticks);
        }

        public static GamePadState operator +(GamePadState gamePadState1, GamePadState gamePadState2)
        {
            return new GamePadState(gamePadState1.IsConnected && gamePadState2.IsConnected,
                                    gamePadState1.Buttons + gamePadState2.Buttons,
                                    gamePadState1.DPad + gamePadState1.DPad,
                                    gamePadState1.Triggers + gamePadState2.Triggers,
                                    gamePadState1.ThumbSticks + gamePadState2.ThumbSticks);
        }

        /// <summary>
        /// Indique si le GamePad est actuellement connecté.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// État des boutons.
        /// </summary>
        public GamePadButtons Buttons { get; private set; }

        /// <summary>
        /// État du DPad.
        /// </summary>
        public GamePadDPad DPad { get; private set; }

        /// <summary>
        /// État des gachettes.
        /// </summary>
        public GamePadTriggers Triggers { get; private set; }

        /// <summary>
        /// États des joysticks analogiques.
        /// </summary>
        public GamePadThumbSticks ThumbSticks { get; private set; }
    }
}