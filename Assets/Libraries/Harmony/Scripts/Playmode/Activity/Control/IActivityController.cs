namespace Harmony
{
    /// <summary>
    /// Représente un contrôleur d'activité. 
    /// </summary>
    /// <seealso cref="Activity"/>
    public interface IActivityController
    {
        /// <summary>
        /// Évènement appellé lorsque l'activité est chargée (c'est-à-dire que tous les fragments et tous les menus sont 
        /// chargés et initialisés).
        /// </summary>
        /// <remarks>
        /// Sous Unity, cette méthode est toujours appellée après <i>Awake</i>, mais elle peut parfois être appellée avant 
        /// ou après <i>Start</i>. Cela est dû au fait que les fragments sont chargés de manière asynchrone : un fragment peut
        /// être chargé avant un autre, et donc, <i>Awake</i> ainsi que <i>Start</i> seront appellée sur ce fragment.
        /// </remarks>
        void OnCreate();

        /// <summary>
        /// Évènement appellé lorsque l'activité est déchargée, que ce soit parce qu'une autre activité est chargée ou parce
        /// que l'activité est arrêtée.
        /// </summary>
        void OnStop();
    }
}