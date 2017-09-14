namespace Harmony
{
    /// <summary>
    /// Signature de toute fonction désirant être notifié d'un chargement d'activité.
    /// </summary>
    public delegate void ActivityLoadingEventHandler();

    /// <summary>
    /// Représente une pile d'<see cref="IActivity"/>. Permet de démarer/arrêter/redémarer des activités.
    /// </summary>
    public interface IActivityStack
    {
        /// <summary>
        /// Événement déclanché lorsque le chargement d'une activité débute.
        /// </summary>
        event ActivityLoadingEventHandler OnActivityLoadingStarted;
        
        /// <summary>
        /// Événement déclanché lorsque le chargement des activités est terminé. Autrement dit, il n'y
        /// aucune activité en cours de chargement.
        /// </summary>
        event ActivityLoadingEventHandler OnActivityLoadingEnded;

        /// <summary>
        /// Charge une activité et l'ajoute sur le dessus de la pile d'activités.
        /// </summary>
        /// <param name="activity">Activité à charger.</param>
        /// <remarks>
        /// <para>
        /// Seule l'activité sur le dessus de la pile est conservée. Autrement dit, s'il y a une activité en cours lors
        /// de l'appel à cette méthode, cette dernière est déchargée avant que la nouvelle soit chargée.
        /// </para>
        /// <para>
        /// Le chargement des Activités est asynchrone. Utilisez les évènements <see cref="OnActivityLoadingStarted"/>
        /// et <see cref="OnActivityLoadingEnded"/> pour être notifié du début et de la fin du chargement d'une activité.
        /// </para>
        /// <para>
        /// Au démarrage, IActivityStack cherche pour un <see cref="IActivityController"/> dans le GameObject supposé contenir
        /// le contrôleur de l'activité. S'il en trouve un, il appelle la méthode <see cref="IActivityController.OnCreate"/> dessus.
        /// </para>
        /// </remarks>
        void StartActivity(IActivity activity);

        /// <summary>
        /// Redémarre l'activité courante, c'est-à-dire celle sur le dessus de la pile d'activités. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rien ne se produit s'il n'y a aucune activité en cours.
        /// </para>
        /// <para>
        /// Le rechargement des Activités est asynchrone. Utilisez les évènements <see cref="OnActivityLoadingStarted"/> et 
        /// <see cref="OnActivityLoadingEnded"/> pour être notifié du début et de la fin du chargement asynchrone d'une activité.
        /// </para>
        /// <para>
        /// Au redémarrage, IActivityStack cherche pour un <see cref="IActivityController"/> dans le GameObject supposé contenir
        /// le contrôleur de l'activité. S'il en trouve un, il appelle la méthode <see cref="IActivityController.OnCreate"/> dessus.
        /// </para>
        /// </remarks>
        void RestartCurrentActivity();

        /// <summary>
        /// Décharge l'activité sur le dessus de la pile et la retire de la pile d'activités.
        /// La nouvelle activité sur le dessus de la pile se retrouve alors rechargée. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Si aucune activité est chargée, l'application est tout simplement arrêtée.
        /// </para>
        /// <para>
        /// Le déchargement des Activités est asynchrone. Utilisez les évènements <see cref="OnActivityLoadingStarted"/>
        /// et <see cref="OnActivityLoadingEnded"/> pour être notifié du début et de la fin du chargement d'une activité.
        /// </para>
        /// </remarks>
        void StopCurrentActivity();

        /// <summary>
        /// Indique si, oui ou non, il y a une activité en cours.
        /// </summary>
        /// <returns>True s'il y a une activité en cours, false sinon.</returns>
        bool HasActivityRunning();
    }
}