using System.Collections.Generic;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Signature des fonction utiles pour le filtrage d'éléments dans une liste. Ces fonctions indique si, oui ou non,
    /// l'item reçu en paramêtre doit être conservé.
    /// </summary>
    /// <typeparam name="T">Type des éléments à filtrer.</typeparam>
    /// <param name="item">Élément à considérer dans le filtrage.</param>
    /// <returns>Vrai si l'élément reçu en paramêtre doit être conservé, faux sinon.</returns>
    public delegate bool FilterFunction<in T>(T item);

    /// <summary>
    /// Contient nombre de méthodes d'extensions pour des <see cref="IList{T}"/> C#.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Considère une liste comme étant une file et retourne l'élément à la fin de la liste (autrement dit,
        /// à la fin de la file).
        /// </summary>
        /// <typeparam name="T">Type des éléments de la liste.</typeparam>
        /// <param name="list">Liste des éléments.</param>
        /// <returns>
        /// Item à la fin de la file. Lance une exception si la file est vide.
        /// </returns> 
        public static T Peek<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        /// <summary>
        /// Considère une liste comme étant une file et ajoute l'élément au début de la liste (autrement dit,
        /// au début de la file).
        /// </summary>
        /// <typeparam name="T">Type des éléments de la liste.</typeparam>
        /// <param name="list">Liste des éléments.</param>
        /// <param name="item">Item à ajouter au début de la liste, comme si elle était une file.</param>
        public static void Enqueue<T>(this IList<T> list, [NotNull] T item)
        {
            list.Insert(0, item);
        }

        /// <summary>
        /// Considère une liste comme étant une file et retire l'élément à la fin de la liste (autrement dit,
        /// à la fin de la file).
        /// </summary>
        /// <typeparam name="T">Type des éléments de la liste.</typeparam>
        /// <param name="list">Liste des éléments.</param>
        /// <returns>
        /// Item à la fin de la file. Lance une exception si la file est vide.
        /// </returns> 
        public static T Dequeue<T>(this IList<T> list)
        {
            T item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }

        /// <summary>
        /// Crée une nouvelle liste à partir de cette liste, en enlevant les éléments ne respectant pas un
        /// certain prédicat.
        /// </summary>
        /// <typeparam name="T">Type des éléments de la liste à filtrer.</typeparam>
        /// <param name="list">Liste des éléments à filtrer.</param>
        /// <param name="filter">Prédicat de filtrage. Voir <see cref="FilterFunction{T}"/></param>
        /// <returns>
        /// Une nouvelle liste filtrée. Notez qu'elle n'est pas nécessairement du même type que la liste 
        /// initiale.
        /// </returns>
        [NotNull]
        public static IList<T> Filter<T>(this IList<T> list, [NotNull] FilterFunction<T> filter)
        {
            IList<T> filteredList = new List<T>();
            foreach (T item in list)
            {
                if (filter(item))
                {
                    filteredList.Add(item);
                }
            }
            return filteredList;
        }

        /// <summary>
        /// Converti une liste de <i>X</i> en une liste de <i>Y</i>, si <i>X</i> hérite bel et bien de <i>Y</i>. 
        /// Souvent nécessaire, car le paramêtre <i>T</i> de <see cref="IList{T}"/> n'est pas covariant.
        /// </summary>
        /// <remarks>
        /// Cet utilitaire est nécessaire, car une <i>IList&lt;Pomme&gt;</i> n'hérite pas de <i>IList&lt;Fruit&gt;</i>, même si
        /// <i>Pomme</i> hérite de <i>Fruit</i>. Une conversion implicite (ou même explicite) est donc impossible et il faut donc 
        /// convertir manuellement la liste.
        /// </remarks>
        /// <typeparam name="From">Type des éléments de la liste à convertir.</typeparam>
        /// <typeparam name="To">Type des éléments de la nouvelle liste. <i>From</i> doit hériter de <i>To</i>.</typeparam>
        /// <param name="list">Liste à convertir.</param>
        /// <returns>Une nouvelle liste contenant les éléments de la liste reçu en paramêtre, mais du type demandé.</returns>
        [NotNull]
        public static IList<To> Convert<From, To>(this IList<From> list) where To : class where From : To
        {
            IList<To> convertedList = new List<To>();
            foreach (From item in list)
            {
                convertedList.Add(item);
            }
            return convertedList;
        }
    }
}