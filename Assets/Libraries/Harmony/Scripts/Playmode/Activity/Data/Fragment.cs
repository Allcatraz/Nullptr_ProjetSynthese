using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un fragment faisant partie d'une activité. Un fragment est une partie indépendante du jeu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Plusieurs fragments peuvent coexister en même temps. Les fragments ne communiquent jamais directement entre eux.
    /// Par contre, il peuvent se servir des <see cref="EventChannel{T}"/> pour publier des évènements ou être notifiés
    /// d'évènements.
    /// </para>
    /// <para>
    /// Les fragments sont chargés en même temps que l'activité. Ils peuvent posséder un contrôleur de fragment, leur permettant
    /// ainsi d'être notifié d'évènements importants reliés au cycle de vie de l'activité.
    /// </para>
    /// </remarks>
    /// <seealso cref="Activity"/>
    [CreateAssetMenu(fileName = "New Fragment", menuName = "Game/Activities/Fragment")]
    public class Fragment : ScriptableObject
    {
        [SerializeField]
        private R.E.Scene scene = R.E.Scene.None;

        [SerializeField]
        private R.E.GameObject controller = R.E.GameObject.None;

        /// <summary>
        /// Scène du Realm. (Obligatoire)
        /// </summary>
        public R.E.Scene Scene
        {
            get { return scene; }
        }

        /// <summary>
        /// Identifiant du GameObject contenant le controleur du Realm. (Facultatif)
        /// </summary>
        public R.E.GameObject Controller
        {
            get { return controller; }
        }
    }
}