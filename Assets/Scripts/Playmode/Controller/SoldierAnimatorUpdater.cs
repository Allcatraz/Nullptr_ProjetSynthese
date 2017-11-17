using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjetSynthese
{
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
        private InterpolationFactors shootingLayerInterpolationFactors;

        [SerializeField]
        private AnimationClip shootingAnimation;
        [SerializeField]
        private AnimationClip grenadeAnimation;


        private Grenade grenade;
        bool isThrowingGrenade;

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

            shootingLayerInterpolationFactors = new InterpolationFactors();
            shootingLayerInterpolationFactors.animatorValueName = "Shooting";
            shootingLayerInterpolationFactors.interpolatingPerFrameValue = 0.02f;


            AnimationEvent shootingEvent = new AnimationEvent();
            shootingEvent.time = shootingAnimation.length;
            shootingEvent.functionName = "EndShootingEvent";
            shootingAnimation.AddEvent(shootingEvent);


            AnimationEvent grenadeReleaseEvent = new AnimationEvent();
            grenadeReleaseEvent.time = grenadeAnimation.length * 0.42f;
            grenadeReleaseEvent.functionName = "ReleaseGrenadeEvent";
            grenadeAnimation.AddEvent(grenadeReleaseEvent);

            AnimationEvent grenadeThrowingEnd = new AnimationEvent();
            grenadeThrowingEnd.time = grenadeAnimation.length;
            grenadeThrowingEnd.functionName = "EndThrowingGrenadeAnimationEvent";
            grenadeAnimation.AddEvent(grenadeThrowingEnd);
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
                InterpolateLayerWeight(ref shootingLayerInterpolationFactors);
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
                if (factors.interpolant > 1 || factors.interpolant < 0)
                {
                    factors.isInterpolating = false;
                }
            }

            factors.lastFrame = factors.presentFrame;
        }

        private void InterpolateLayerWeight(ref InterpolationFactors factors)
        {
            if (factors.isInterpolating == true)
            {
                factors.actual = Mathf.Lerp(factors.begin, factors.end, factors.interpolant);
                animator.SetLayerWeight(animator.GetLayerIndex(factors.animatorValueName), factors.actual);
                factors.interpolant += factors.interpolatingPerFrameValue;
                if (factors.interpolant > 1 || factors.interpolant < 0)
                {
                    factors.isInterpolating = false;
                }
            }

            factors.lastFrame = factors.presentFrame;
        }

        public void Shoot()
        {
            shootingLayerInterpolationFactors.isInterpolating = false;
            animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 1);
            animator.Play("assault_combat_shoot", -1, 0f);
        }

        public void ThrowGrenade(Grenade grenade)
        {
            if (grenade != null && isThrowingGrenade == false)
            {
                animator.SetLayerWeight(animator.GetLayerIndex("GrenadeThrow"), 1);
                animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 0);
                animator.Play("assault_combat_throw_grenade", -1, 0f);
                isThrowingGrenade = true;
                this.grenade = grenade;
            }

        }

        private void EndShootingEvent()
        {
            shootingLayerInterpolationFactors.isInterpolating = true;
            shootingLayerInterpolationFactors.begin = 1;
            shootingLayerInterpolationFactors.end = 0;
            shootingLayerInterpolationFactors.interpolant = 0;
        }

        private void ReleaseGrenadeEvent()
        {
            grenade.Release();
            grenade = null;
        }

        private void EndThrowingGrenadeAnimationEvent()
        {
            animator.SetLayerWeight(animator.GetLayerIndex("GrenadeThrow"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 1);
            isThrowingGrenade = false;
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
}


