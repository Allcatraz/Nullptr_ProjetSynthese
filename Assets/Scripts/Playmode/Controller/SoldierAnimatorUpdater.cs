using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimatorUpdater : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public Vector3 MouvementDirection { get; set; }
    public Vector3 ViewDirection { get; set; }

    public void UpdateAnimator()
    {
    

        float angle = 0;
        float direction = Vector3.Dot(Vector3.Normalize(MouvementDirection), Vector3.Normalize(ViewDirection));

        if (direction > 0)
        {
            direction = 1;
            angle = Vector3.SignedAngle(MouvementDirection, ViewDirection, Vector3.up);
        }
        else if (direction < 0)
        {
            direction = -1;
            angle = Vector3.SignedAngle(MouvementDirection * -1, ViewDirection, Vector3.up);
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
