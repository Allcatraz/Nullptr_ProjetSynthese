using Harmony.Testing;
using XInputDotNetPure;

namespace Harmony.XInput
{
    /// <summary>
    /// Représente un GamePadInput XInput.
    /// </summary>
    /// <inheritdoc cref="IGamePadInput"/>
    [NotTested(Reason.Wrapper)]
    public class XInputGamepadInput : IGamePadInput
    {
        public GamePadState GetGamepadState()
        {
            return GetGamepadState(PlayerIndex.One) +
                   GetGamepadState(PlayerIndex.Two) +
                   GetGamepadState(PlayerIndex.Three) +
                   GetGamepadState(PlayerIndex.Four);
        }

        public GamePadState GetGamepadState(PlayerIndex playerIndex)
        {
            return GamePad.GetState(playerIndex);
        }
    }
}