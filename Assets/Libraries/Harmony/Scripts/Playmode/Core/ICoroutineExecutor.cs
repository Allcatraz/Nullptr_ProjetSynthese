using System.Collections;
using JetBrains.Annotations;

namespace Harmony
{
    /// <summary>
    /// Représente un exécuteur de Coroutines. Ce dernier s'occupe d'exécuter les Coroutines jusqu'à la fin.
    /// </summary>
    public interface ICoroutineExecutor
    {
        /// <summary>
        /// Démarre une nouvelle Coroutine, attachée au UnityScript fourni.
        /// </summary>
        /// <param name="target">
        /// Script auquel doit être attachée la Coroutine. Si ce Script en vient à être
        /// détruit, alors la Coroutine sera elle aussi détruite.
        /// </param>
        /// <param name="coroutineMethod">
        /// Méthode de la Coroutine.
        /// </param>
        /// <returns>
        /// Objet représentant la Couroutine. Sert, entre autre, à l'arrêter.
        /// </returns>
        [NotNull]
        ICoroutine StartCoroutine([NotNull] IScript target, [NotNull] IEnumerator coroutineMethod);
    }
}