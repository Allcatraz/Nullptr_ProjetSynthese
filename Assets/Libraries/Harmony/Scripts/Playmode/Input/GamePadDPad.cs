using XInputDotNetPure;

namespace Harmony
{
    /// <summary>
    /// Structure représentant l'état du DPad d'un GamePad, capturé à un instant précis.
    /// </summary>
    public struct GamePadDPad
    {
        public GamePadDPad(ButtonState up,
                           ButtonState down,
                           ButtonState left,
                           ButtonState right) : this()
        {
            Up = up;
            Down = down;
            Left = left;
            Right = right;
        }

        public static implicit operator GamePadDPad(XInputDotNetPure.GamePadDPad gamePadDPad)
        {
            return new GamePadDPad(gamePadDPad.Up,
                                   gamePadDPad.Down,
                                   gamePadDPad.Left,
                                   gamePadDPad.Right);
        }

        public static GamePadDPad operator +(GamePadDPad gamePadDPad1, GamePadDPad gamePadDPad2)
        {
            return
                new GamePadDPad(
                    gamePadDPad1.Up == ButtonState.Pressed || gamePadDPad2.Up == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadDPad1.Down == ButtonState.Pressed || gamePadDPad2.Down == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadDPad1.Left == ButtonState.Pressed || gamePadDPad2.Left == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released,
                    gamePadDPad1.Right == ButtonState.Pressed || gamePadDPad2.Right == ButtonState.Pressed
                        ? ButtonState.Pressed
                        : ButtonState.Released);
        }

        /// <summary>
        /// Indique si DPad Up est appuyé ou non.
        /// </summary>
        public ButtonState Up { get; private set; }

        /// <summary>
        /// Indique si DPad Down est appuyé ou non.
        /// </summary>
        public ButtonState Down { get; private set; }

        /// <summary>
        /// Indique si DPad Left est appuyé ou non.
        /// </summary>
        public ButtonState Left { get; private set; }

        /// <summary>
        /// Indique si DPad Right est appuyé ou non.
        /// </summary>
        public ButtonState Right { get; private set; }
    }
}