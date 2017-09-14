using XInputDotNetPure;

namespace Harmony
{
    /// <summary>
    /// Représente une entrée de type manette.
    /// </summary>
    public interface IGamePadInput
    {
        /// <summary>
        /// Retourne l'état courant des manettes branchées.
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
        GamePadState GetGamepadState();

        /// <summary>
        /// Retourne l'état courant d'une manette.
        /// </summary>
        /// <param name="playerIndex">Index de la manette à obtenir l'état.</param>
        /// <returns>État de la manette à obtenir.</returns>
        /// <remarks>
        /// Lors du "mocking" de cette méthode, ne retournez pas un "mock" de <see cref="GamePadState"/>. Retournez
        /// un objet complet. De toute façon, il n'est pas possible de créer un "mock" d'une "struct".
        /// </remarks>
        GamePadState GetGamepadState(PlayerIndex playerIndex);
    }
}