using System.Collections.Generic;

namespace Harmony
{
    /// <summary>
    /// Représente une Activité. Une activité est un lot de <see cref="IFragment"/> et de <see cref="IMenu"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Une seule activité est affichée à la fois. Si une autre activité est démarrée, l'activité courante est déchargée 
    /// au profit de la nouvelle activité. La nouvelle activité est ensuite mise sur le dessus de la pile d'activités en cours. 
    /// </para>
    /// <para>
    /// Lorsque l'activité courante est arrêtée, cette dernière est déchargée, retirée de la pile, et c'est la nouvelle 
    /// activité sur le dessus de la pile qui est chargée.
    /// </para>
    /// <para>
    /// S'il n'y a plus aucune activité sur la pile, mais que l'on demande tout de même d'arrêter l'activité courante,
    /// alors c'est l'application au complet qui est arrêtée.
    /// </para>
    /// <para>
    /// Pour charger des Activités, utilisez un IActivityStack.
    /// </para>
    /// </remarks>
    /// <seealso cref="IFragment"/>
    /// <seealso cref="IMenu"/>
    /// <seealso cref="IActivityStack"/>
    public interface IActivity : IData
    {
        /// <summary>
        /// Scène de l'activité. (Facultatif)
        /// </summary>
        R.E.Scene Scene { get; }

        /// <summary>
        /// Identifiant du GameObject contenant le controleur de l'activité. (Facultatif)
        /// </summary>
        R.E.GameObject Controller { get; }

        /// <summary>
        /// Lot de fragments de l'activité. (Facultatif) 
        /// </summary>
        IList<IFragment> Fragments { get; }

        /// <summary>
        /// Lot de menus de l'activité. (Facultatif)
        /// </summary>
        IList<IMenu> Menus { get; }

        /// <summary>
        /// Fragment à marquer comme actif lors du chargement de l'activité (Facultatif).
        /// </summary>
        IFragment ActiveFragmentOnLoad { get; }
    }
}