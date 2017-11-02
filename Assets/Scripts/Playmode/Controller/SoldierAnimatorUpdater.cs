using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoldierAnimatorUpdater : MonoBehaviour
{
    [SerializeField]
    [Tooltip("L'animator utilisé pour faire l'animation du joueur.")]
    private Animator animator;
    public Vector3 MouvementDirection { get; set; }
    public Vector3 ViewDirection { get; set; }
    private int leftHandLayer;
    private bool isShooting;
    private float iterpolatingDirectionBegin;
    private float interpolatingDirectionEnd;
    private float actualDirection;
    private float interpolant;
    private bool isInterpoling;

    private float lastFrameDirection;

    public void UpdateAnimator()
    {
        float viewMagn = ViewDirection.magnitude;
        float mouvMagn = MouvementDirection.magnitude;
        if (viewMagn != 0 && mouvMagn != 0)
        {
            double cosTheta = Vector3.Dot(MouvementDirection, ViewDirection) / (mouvMagn * viewMagn);

            double angle = Math.Acos(cosTheta);
            float angleDegree = Mathf.Rad2Deg * (float)angle;

            float presentFrameDirection = Vector3.Dot(new Vector3(MouvementDirection.z * -1, 0, MouvementDirection.x).normalized, ViewDirection.normalized);

            if (Mathf.Abs(lastFrameDirection - presentFrameDirection) > 0.5f)
            {
                isInterpoling = true;
                iterpolatingDirectionBegin = actualDirection;
                interpolatingDirectionEnd = presentFrameDirection;
                interpolant = 0;
            }


            if (isInterpoling == true)
            {
                actualDirection = Mathf.Lerp(iterpolatingDirectionBegin, interpolatingDirectionEnd, interpolant);
                animator.SetFloat("DirectionFactor", actualDirection);
                interpolant += 0.02f;
                if (interpolant > 1)
                {
                    isInterpoling = false;
                }
            }

            animator.SetFloat("Angle", angleDegree);


            lastFrameDirection = presentFrameDirection;
        }
        else
        {

        }

        animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1);

    }

    public void Shoot()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 0);
        isShooting = true;
    }
}
