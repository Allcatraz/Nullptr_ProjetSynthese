namespace Harmony
{
    /// <summary>
    /// Signature de toute fonction désirant être notifié qu'un évènement avec un <see cref="ICollider2D"/>.
    /// </summary>
    public delegate void Collider2DTriggerEventHandler(ICollider2D otherCollider2D);

    /// <summary>
    /// Représente un objet physique en 2D pouvant colisionner avec un autre.
    /// </summary>
    public interface ICollider2D : IDisableable
    {
        /// <summary>
        /// Évènement déclanché lorsqu'un objet entre dans le Collider.
        /// </summary>
        event Collider2DTriggerEventHandler OnTriggerEntered;

        /// <summary>
        /// Évènement déclanché lorsqu'un objet quitte le Collider.
        /// </summary>
        event Collider2DTriggerEventHandler OnTriggerExited;

        /// <summary>
        /// Indique si ce Collider est utilisé pour de la simulation physique ou pour détecter
        /// si un objet entre à l'intérieur.
        /// </summary>
        bool IsTrigger { get; set; }

        /// <summary>
        /// Retourne le component demandé sur l'autre GameObject.
        /// </summary>
        /// <typeparam name="T">Type du Component à obtenir.</typeparam>
        /// <returns>Un Component du type demandé, ou null s'il en existe aucun.</returns>
        T GetOtherComponent<T>() where T : class;
    }
}