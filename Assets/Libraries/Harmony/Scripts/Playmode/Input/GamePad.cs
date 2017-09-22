using UnityEngine;
using XInputDotNetPure;

namespace Harmony
{
    /// <summary>
    /// Représente une entrée de type manette.
    /// </summary>
    [AddComponentMenu("Game/Input/GamePad")]
    public class GamePad : Script
    {
        /// <summary>
        /// Retourne l'état courant des manettes branchées. Sans DeadZone.
        /// </summary>
        /// <returns>État des manettes.</returns>
        /// <remarks>
        /// <para>
        /// </para>
        /// <para>
        /// Lors du "mocking" de cette méthode, ne retournez pas un "mock" de <see cref="GamePadState"/>. Retournez
        /// un objet complet. De toute façon, il n'est pas possible de créer un "mock" d'une "struct".
        /// </para>
        /// </remarks>
        public GamePadState GetGamepadState()
        {
            return GetGamepadState(GamePadDeadZone.None);
        }

        /// <summary>
        /// Retourne l'état courant des manettes branchées.
        /// </summary>
        /// <param name="deadZoneStyle">Type de DeadZone à appliquer.</param>
        /// <returns>État des manettes.</returns>
        /// <remarks>
        /// <para>
        /// </para>
        /// <para>
        /// Lors du "mocking" de cette méthode, ne retournez pas un "mock" de <see cref="GamePadState"/>. Retournez
        /// un objet complet. De toute façon, il n'est pas possible de créer un "mock" d'une "struct".
        /// </para>
        /// </remarks>
        public GamePadState GetGamepadState(GamePadDeadZone deadZoneStyle)
        {
            return GetGamepadState(PlayerIndex.One, deadZoneStyle) +
                   GetGamepadState(PlayerIndex.Two, deadZoneStyle) +
                   GetGamepadState(PlayerIndex.Three, deadZoneStyle) +
                   GetGamepadState(PlayerIndex.Four, deadZoneStyle);
        }

        /// <summary>
        /// Retourne l'état courant d'une manette. Sans DeadZone.
        /// </summary>
        /// <param name="playerIndex">Index de la manette à obtenir l'état.</param>
        /// <returns>État de la manette à obtenir.</returns>
        /// <remarks>
        /// Lors du "mocking" de cette méthode, ne retournez pas un "mock" de <see cref="GamePadState"/>. Retournez
        /// un objet complet. De toute façon, il n'est pas possible de créer un "mock" d'une "struct".
        /// </remarks>
        public GamePadState GetGamepadState(PlayerIndex playerIndex)
        {
            return GetGamepadState(playerIndex, GamePadDeadZone.None);
        }

        /// <summary>
        /// Retourne l'état courant d'une manette.
        /// </summary>
        /// <param name="playerIndex">Index de la manette à obtenir l'état.</param>
        /// <param name="deadZoneStyle">Type de DeadZone à appliquer.</param>
        /// <returns>État de la manette à obtenir.</returns>
        /// <remarks>
        /// Lors du "mocking" de cette méthode, ne retournez pas un "mock" de <see cref="GamePadState"/>. Retournez
        /// un objet complet. De toute façon, il n'est pas possible de créer un "mock" d'une "struct".
        /// </remarks>
        public GamePadState GetGamepadState(PlayerIndex playerIndex, GamePadDeadZone deadZoneStyle)
        {
            return XInputDotNetPure.GamePad.GetState(playerIndex, deadZoneStyle);
        }
    }
}