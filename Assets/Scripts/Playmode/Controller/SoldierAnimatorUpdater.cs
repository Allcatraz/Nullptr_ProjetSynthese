using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimatorUpdater : MonoBehaviour
{
    [SerializeField]
    [Tooltip("L'animator utilisé pour faire l'animation du joueur.")]
    private Animator animator;
    public Vector3 MouvementDirection { get; set; }
    public Vector3 ViewDirection { get; set; }
    private int leftHandLayer;

    public void UpdateAnimator()
    {
        float angle = 0;
        float direction = Vector3.Dot(Vector3.Normalize(MouvementDirection), Vector3.Normalize(ViewDirection));

        if (direction > 0)
        {
            direction = 1;
            angle = Vector3.SignedAngle(MouvementDirection, ViewDirection, Vector3.up);
            //animator.SetLayerWeight(animator.GetLayerIndex("LeftHand"), 0);
        }
        else if (direction < 0)
        {
            direction = -1;
            angle = Vector3.SignedAngle(MouvementDirection * -1, ViewDirection, Vector3.up);
            //animator.SetLayerWeight(animator.GetLayerIndex("LeftHand"), 1);
        }
        else
        {
            direction = 0;
        }
        angle /= 90;
        animator.SetInteger("Direction", (int)direction);
        animator.SetFloat("DirectionFactor", angle);

    }
}
