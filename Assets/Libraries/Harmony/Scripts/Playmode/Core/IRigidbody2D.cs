using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente un objet soumis au règles de la physique en deux dimensions.
    /// </summary>
    public interface IRigidbody2D : IComponent
    {
        /// <summary>
        /// Force de frotement à l'air lorsqu'il y a rotation de l'objet.
        /// </summary>
        float AngularDrag { get; set; }

        /// <summary>
        /// Force de frotement à l'air lorsqu'il y a movement de l'objet (à ne pas confondre avec Rotation, voir <see cref="AngularDrag"/>).
        /// </summary>
        float Drag { get; set; }

        /// <summary>
        /// Applique une force sur l'objet. La force est appliquée au centre de l'objet et ne cause pas sa rotation.
        /// </summary>
        /// <param name="force">Force à appliquer sur l'objet. Plus le vecteur est grand, plus la force est grande.</param>
        void AddForce(Vector2 force);

        /// <summary>
        /// Applique une force de rotation sur sur l'objet. Étant en 2 dimensions, la rotation s'effectue sur l'axe Z. La règle de la main
        /// gauche s'applique, ce qui veut dire qu'une valeur positive implique une rotation dans le sens horaire tandis qu'une force négative 
        /// implique une rotation dans le sans anti-horaire.
        /// </summary>
        /// <param name="torque">Force de rotation à appliquer. Plus la valeur est grande, plus la force est grande.</param>
        void AddTorque(float torque);

        /// <summary>
        /// Déplace l'objet dans l'espace via une translation. L'objet s'arrêtera sur tout objet immuable durant son movement, 
        /// mais considérez sa force comme étant infinie, ce qui veut dire que tout objet non immuable sera déplacé sans qu'il puisse
        /// résister.
        /// </summary>
        /// <param name="translation">Vecteur représentant le movement à effectuer.</param>
        void Translate(Vector2 translation);
    }
}