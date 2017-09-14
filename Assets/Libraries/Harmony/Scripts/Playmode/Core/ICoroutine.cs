namespace Harmony
{
    /// <summary>
    /// Représente une Coroutine. Une Coroutine est une fonction étant en mesure de mettre en pause son exécution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Une Coroutine est comparable à un Thread, à la différence que cela en est pas un. Une Coroutine est en fait
    /// une fonction qui est en mesure de mettre elle même fin à son exécution, tout en conservant son état afin
    /// de reprendre exactement où elle s'était arrêtée.
    /// </para>
    /// <para>
    /// L'appel à une Coroutine est synchrone, ce qui veut dire qu'elle bloque jusqù'à ce que son exécution soit
    /// terminée ou qu'elle décide de se mettre en pause. C'est toute la différence avec un Thread, qui lui, est 
    /// asynchrone et nécessite donc un mécanisme de synchronisation.
    /// </para>
    /// <para>
    /// Il est de la responsabilité de l'utilisateur d'une Coroutine de l'appeller tant et aussi longtemps quelle
    /// le nécessite. Cette dernière retourne une valeur indiquant si, oui ou non, elle doit encore être appellée.
    /// Certaines Coroutine vont même plus loin en indiquant <i>quand</i> elle devront être appellées à nouveau.
    /// </para>
    /// </remarks>
    /// <seealso cref="ICoroutineExecutor"/>
    public interface ICoroutine
    {
        /// <summary>
        /// Arrête l'exécution de cette Coroutine.
        /// </summary>
        /// <remarks>
        /// <para>
        /// L'arrêt de l'exécution d'une Coroutine n'est pas immédiat. En vérité, l'appel à cette
        /// méthode empêche plutôt tout nouvel appel à la Coroutine, cette dernière indiquant alors
        /// à son appellant qu'elle a terminé.
        /// </para>
        /// <para>
        /// Autrement dit, si cette Coroutine est exécutée sur un autre thread, l'appel à cette méthode
        /// n'arrêtera pas son exécution sur ce thread, mais empêchera plutôt toute nouvelle exécution.
        /// </para>
        /// </remarks>
        void Stop();
    }
}