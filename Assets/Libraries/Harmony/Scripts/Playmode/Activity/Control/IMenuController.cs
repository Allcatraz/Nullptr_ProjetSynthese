namespace Harmony
{
    /// <summary>
    /// Représente un contrôleur de menu. 
    /// </summary>
    /// <seealso cref="Menu"/>
    public interface IMenuController
    {
        /// <summary>
        /// Évènement appellé lorsque le menu est ouvert.
        /// </summary>
        /// <param name="parameters">Paramêtres envoyés lors de la demande d'ouverture du menu.</param>
        /// <remarks>
        /// <para>
        /// Les menus sont toujours préchargés. Cette méthode n'est pas appellée dès que le menu est chargé, mais
        /// plutôt lorsque le menu doit être affiché pour la première fois.
        /// </para>
        /// <para>
        /// Les menus sont affichés les uns par dessus les autres. Dès qu'un menu est affiché, <see cref="OnCreate"/>
        /// est appellé. Si un autre menu est affiché par dessus, <see cref="OnPause"/> est appellé. Dès que ce menu
        /// est fermé (et donc caché), <see cref="OnResume"/> est appellé, mais pas <see cref="OnCreate"/>. Enfin,
        /// lorsqu'un menu est fermé, <see cref="OnStop"/> est appellé.
        /// </para>
        /// </remarks>
        void OnCreate(params object[] parameters);

        /// <summary>
        /// Évènement appellé lorsque le menu est visible.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Les menus sont affichés les uns par dessus les autres. Dès qu'un menu est affiché, <see cref="OnCreate"/>
        /// est appellé. Si un autre menu est affiché par dessus, <see cref="OnPause"/> est appellé. Dès que ce menu
        /// est fermé (et donc caché), <see cref="OnResume"/> est appellé, mais pas <see cref="OnCreate"/>. Enfin,
        /// lorsqu'un menu est fermé, <see cref="OnStop"/> est appellé.
        /// </para>
        /// </remarks>
        void OnResume();

        /// <summary>
        /// Évènement appellé lorsque le menu est caché par un autre.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Les menus sont affichés les uns par dessus les autres. Dès qu'un menu est affiché, <see cref="OnCreate"/>
        /// est appellé. Si un autre menu est affiché par dessus, <see cref="OnPause"/> est appellé. Dès que ce menu
        /// est fermé (et donc caché), <see cref="OnResume"/> est appellé, mais pas <see cref="OnCreate"/>. Enfin,
        /// lorsqu'un menu est fermé, <see cref="OnStop"/> est appellé.
        /// </para>
        /// </remarks>
        void OnPause();

        /// <summary>
        /// Évènement appellé lorsque le menu est fermé.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Les menus sont affichés les uns par dessus les autres. Dès qu'un menu est affiché, <see cref="OnCreate"/>
        /// est appellé. Si un autre menu est affiché par dessus, <see cref="OnPause"/> est appellé. Dès que ce menu
        /// est fermé (et donc caché), <see cref="OnResume"/> est appellé, mais pas <see cref="OnCreate"/>. Enfin,
        /// lorsqu'un menu est fermé, <see cref="OnStop"/> est appellé.
        /// </para>
        /// </remarks>
        void OnStop();
    }
}