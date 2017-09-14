using JetBrains.Annotations;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Fabrique d'objet préfabriqués.
    /// </summary>
    public interface IPrefabFactory
    {
        /// <summary>
        /// Crée un nouvel objet à partir d'un Prefab.
        /// </summary>
        /// <param name="prefab">Prefab à instancier.</param>
        /// <param name="position">Position du nouvel objet.</param>
        /// <param name="rotation">Rotation du nouvel objet.</param>
        /// <returns>Le nouvel objet construit à partir du moule.</returns>
        [NotNull]
        GameObject Instantiate([NotNull] GameObject prefab, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Crée un nouvel objet à partir d'un Prefab et le place comme enfant du GameObject spécifié.
        /// </summary>
        /// <param name="prefab">Prefab à instancier.</param>
        /// <param name="position">Position du nouvel objet.</param>
        /// <param name="rotation">Rotation du nouvel objet.</param>
        /// <param name="parent">Parent du nouvel objet.</param>
        /// <returns>Le nouvel objet construit à partir du moule.</returns>
        /// <remarks>
        /// Le GameObject est d'abbord créé sans parent, ce qui veut dire qu'il n'a pas de parent durant sa construction
        /// et son initialisation (sous Unity, l'appel à Awake). Lorsque l'initialisation est terminée, le parent est attribué.
        /// Il est donc isolé de tout autre GameObject durant sa création, ce qui est assez pratique.
        /// </remarks>
        [NotNull]
        GameObject Instantiate([NotNull] GameObject prefab, Vector3 position, Quaternion rotation, [NotNull] GameObject parent);
    }
}