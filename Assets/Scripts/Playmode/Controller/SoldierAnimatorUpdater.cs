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

    private bool isShooting;

    private InterpolationFactors directionInterpolationFactors;
    private InterpolationFactors angleInterpolationFactors;

    [SerializeField]
    private AnimationClip shootingAnimation;
    [SerializeField]
    private AnimationClip grenadeAnimation;



    private void Awake()
    {
        directionInterpolationFactors = new InterpolationFactors();
        directionInterpolationFactors.animatorValueName = "DirectionFactor";
        directionInterpolationFactors.diffFactor = 0.5f;
        directionInterpolationFactors.interpolatingPerFrameValue = 0.02f;

        angleInterpolationFactors = new InterpolationFactors();
        angleInterpolationFactors.animatorValueName = "Angle";
        angleInterpolationFactors.diffFactor = 2;
        angleInterpolationFactors.interpolatingPerFrameValue = 0.02f;

        AnimationEvent shootingEvent = new AnimationEvent();
        shootingEvent.time = shootingAnimation.length;
        shootingEvent.functionName = "EndShooting";
        shootingAnimation.AddEvent(shootingEvent);
        
    }

    public void UpdateAnimator()
    {
        float viewMagn = ViewDirection.magnitude;
        float mouvMagn = MouvementDirection.magnitude;
        if (viewMagn != 0 && mouvMagn != 0)
        {
            double cosTheta = Vector3.Dot(MouvementDirection, ViewDirection) / (mouvMagn * viewMagn);

            cosTheta = cosTheta < -1 ? -1 : cosTheta;
            cosTheta = cosTheta > 1 ? 1 : cosTheta;
            double angle = Math.Acos(cosTheta);

            angleInterpolationFactors.presentFrame = Mathf.Rad2Deg * (float)angle;

            directionInterpolationFactors.presentFrame = Vector3.Dot(new Vector3(MouvementDirection.z * -1, 0, MouvementDirection.x).normalized, ViewDirection.normalized);

            InterpolateAnimation(ref angleInterpolationFactors);
            InterpolateAnimation(ref directionInterpolationFactors);
        }
        else
        {
            // TODO : Gèrer le idle
        }
    }

    private void InterpolateAnimation(ref InterpolationFactors factors)
    {
        if (Mathf.Abs(factors.lastFrame - factors.presentFrame) > factors.diffFactor)
        {
            factors.isInterpolating = true;
            factors.begin = factors.actual;
            factors.end = factors.presentFrame;
            factors.interpolant = 0;
        }

        if (factors.isInterpolating == true)
        {
            factors.actual = Mathf.Lerp(factors.begin, factors.end, factors.interpolant);
            animator.SetFloat(factors.animatorValueName, factors.actual);
            factors.interpolant += factors.interpolatingPerFrameValue;
            if (factors.interpolant > 1)
            {
                factors.isInterpolating = false;
            }
        }

        factors.lastFrame = factors.presentFrame;
    }

    public void Shoot()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 1);
        animator.Play("assault_combat_shoot", -1, 0f);
        Debug.Log("Begin the shooting animation");
    }

    public void EndShooting()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 0);
        Debug.Log("End the shooting animation");
    }

    private struct InterpolationFactors
    {
        public float begin;
        public float end;
        public float actual;
        public float interpolant;
        public bool isInterpolating;
        public float lastFrame;
        public float presentFrame;
        public float diffFactor;
        public string animatorValueName;
        public float interpolatingPerFrameValue;
    }
}
