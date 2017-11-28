using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class SoldierAnimatorUpdater : NetworkGameScript
    {
        private Animator animator;
        private NetworkAnimator networkAnimator;
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
        private bool isThrowingGrenade;

        [SerializeField]
        [Tooltip("The number of parameters to sync on the network")]
        private int numberOfParamsToSync;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            networkAnimator = GetComponent<NetworkAnimator>();


            for (int i = 0; i < numberOfParamsToSync; i++)
            {
                networkAnimator.SetParameterAutoSend(i, true);
            }


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
            CmdShoot();
        }

        [Command]
        private void CmdShoot()
        {
            RpcShoot();
        }

        [ClientRpc]
        private void RpcShoot()
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Shooting"), 1);
            animator.Play("assault_combat_shoot", -1, 0f);
        }

        public void ThrowGrenade(Grenade grenade)
        {
            CmdThrowGrenade(grenade.gameObject.GetComponent<NetworkIdentity>());
        }

        [Command]
        private void CmdThrowGrenade(NetworkIdentity grenade)
        {
            RpcThrowGrenade(grenade);
        }

        [ClientRpc]
        private void RpcThrowGrenade(NetworkIdentity grenade)
        {
            if (grenade != null && isThrowingGrenade == false)
            {
                animator.SetLayerWeight(animator.GetLayerIndex("GrenadeThrow"), 1);
                animator.SetLayerWeight(animator.GetLayerIndex("Hands"), 0);
                animator.Play("assault_combat_throw_grenade", -1, 0f);
                isThrowingGrenade = true;
                this.grenade = grenade.gameObject.GetComponent<Grenade>();
            }
        }

        private void EndShootingEvent()
        {
            CmdEndShootingEvent();
        }

        [Command]
        private void CmdEndShootingEvent()
        {
            RpcEndShootingEvent();
        }

        [ClientRpc]
        private void RpcEndShootingEvent()
        {
            shootingLayerInterpolationFactors.isInterpolating = true;
            shootingLayerInterpolationFactors.begin = 1;
            shootingLayerInterpolationFactors.end = 0;
            shootingLayerInterpolationFactors.interpolant = 0;
        }

        private void ReleaseGrenadeEvent()
        {
            if (isLocalPlayer)
            {
                grenade.Release();
                grenade = null;
            }
        }

        [Command]
        private void CmdReleaseGrenadeEvent(NetworkIdentity grenade)
        {
            RpcReleaseGrenadeEvent(grenade);
        }

        [ClientRpc]
        private void RpcReleaseGrenadeEvent(NetworkIdentity grenade)
        {
            if (isLocalPlayer)
            {
                grenade.GetComponent<Grenade>().Release();
            }

        }

        private void EndThrowingGrenadeAnimationEvent()
        {
            CmdEndThrowingGrenadeAnimationEvent();
        }

        [Command]
        private void CmdEndThrowingGrenadeAnimationEvent()
        {
            RpcEndThrowingGrenadeAnimationEvent();
        }

        [ClientRpc]
        private void RpcEndThrowingGrenadeAnimationEvent()
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


