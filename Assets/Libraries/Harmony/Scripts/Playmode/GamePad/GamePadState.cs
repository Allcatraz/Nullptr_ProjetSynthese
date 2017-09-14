using UnityEngine;
using System.Collections;

namespace Harmony
{
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

        public bool IsConnected { get; private set; }
        public GamePadButtons Buttons { get; private set; }
        public GamePadDPad DPad { get; private set; }
        public GamePadTriggers Triggers { get; private set; }
        public GamePadThumbSticks ThumbSticks { get; private set; }
    }
}