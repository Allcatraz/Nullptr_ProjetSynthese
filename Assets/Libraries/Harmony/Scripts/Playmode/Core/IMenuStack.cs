namespace Harmony
{
    /// <summary>
    /// Représente une pile d'<see cref="IMenu"/>. Permet de démarer/arrêter/redémarer des menus.
    /// </summary>
    public interface IMenuStack
    {
        /// <summary>
        /// Active le menu donné et l'ajoute sur le dessus de la pile de menus.
        /// </summary>
        /// <param name="menu">Menu à activer.</param>
        /// <param name="parameters">Paramêtres à envoyer au menu, si nécessaire.</param>
        /// <remarks>
        /// <para>
        /// Seul le menu sur le dessus de la pile est actif. Tous les menus sur la pile sont affichés en même temps.
        /// </para>
        /// <para>
        /// Pour qu'un menu puisse être démarré, ce dernier doit être chargé. Assurez-vous donc les menus 
        /// utilisés par l'activités soient chargés avant de les utiliser.
        /// </para>
        /// <para>
        /// Au démarrage d'un menu, IMenuStack cherche pour un <see cref="IMenuController"/> dans la scène
        /// du menu. S'il en trouve un, il appelle les méthodes <see cref="IMenuController.OnCreate"/> et 
        /// <see cref="IMenuController.OnResume"/> dessus.
        /// </para>
        /// <para>
        /// S'il y avait déjà un menu d'actif, IMenuStack appellera la méthode <see cref="IMenuController.OnPause"/> sur
        /// le contrôleur de l'ancien menu avant d'activer le nouveau menu.
        /// </para>
        /// </remarks>
        void StartMenu(IMenu menu, params object[] parameters);

        /// <summary>
        /// Désactive le menu sur le dessus de la pile et la retire de la pile de menus.
        /// Le nouveau menu sur le dessus de la pile se retrouve alors activé, s'il y en a un. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rien ne se produit s'il n'y a aucun menu actif.
        /// </para>
        /// <para>
        /// Lors de l'arrêt d'un menu, IMenuStack cherche pour un <see cref="IMenuController"/> dans la scène
        /// du menu. S'il en trouve un, il appelle les méthodes <see cref="IMenuController.OnPause"/> et 
        /// <see cref="IMenuController.OnStop"/> dessus.
        /// </para>
        /// <para>
        /// Si suivant le retrait du menu courant de la pile il existe toujours un menu dans cette pile, IMenuStack 
        /// appellera la méthode <see cref="IMenuController.OnResume"/> sur le contrôleur de ce menu.
        /// </para>
        /// </remarks>
        void StopCurrentMenu();

        /// <summary>
        /// Indique si, oui ou non, il y a un menu en cours.
        /// </summary>
        /// <returns>True s'il y a un menu en cours, false sinon.</returns>
        bool HasMenuRunning();
    }
}