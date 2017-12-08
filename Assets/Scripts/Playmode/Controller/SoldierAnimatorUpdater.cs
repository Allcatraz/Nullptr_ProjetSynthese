using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    public class SoldierAnimatorUpdater : NetworkGameScript
    {
        #region Strings and layer index

        private const string ValueNameDirectionFactors = "DirectionFactors";
        private const string ValueNameAngle = "Angle";
        private const string ValueNameIsMoving = "IsMoving";
        private const string ValueNameSpeed = "Speed";

        private const string LayerNameShooting = "Shooting";
        private const string LayerNameHands = "Hands";
        private const string LayerNameGrenadeThrow = "GrenadeThrow";
        private const string LayerNameReload = "Reload";

        private const string FunctionNameEndShootingEvent = "EndShootingEvent";
        private const string FunctionNameReleaseGrenadeEvent = "ReleaseGrenadeEvent";
        private const string FunctionNameEndThrowingGrenadeAnimationEvent = "EndThrowingGrenadeAnimationEvent";
        private const string FunctionNameEndReloadingAnimationEvent = "EndReloadingAnimationEvent";

        private const string AnimationNameReload = "assault_combat_reload_generic";
        private const string AnimationNameShoot = "assault_combat_shoot";
        private const string AnimationNameThrowGrenade = "assault_combat_throw_grenade";

        #endregion


        [SerializeField]
        [Tooltip("L'animation de tir du player")]
        private AnimationClip shootingAnimation;
        [SerializeField]
        [Tooltip("L'animation de lancé de grenade du player")]
        private AnimationClip grenadeAnimation;
        [SerializeField]
        [Tooltip("L'animation de reload du player")]
        private AnimationClip reloadAnimation;
        [SerializeField]
        [Tooltip("The number of parameters to sync on the network")]
        private int numberOfParamsToSync;

        private Animator animator;
        private NetworkAnimator networkAnimator;
        public Vector3 MouvementDirection { get; set; }
        public Vector3 ViewDirection { get; set; }        

        public float NormalPlayerSpeed { get; set; }

        private InterpolationFactors directionInterpolationFactors;
        private InterpolationFactors angleInterpolationFactors;
        private InterpolationFactors shootingLayerInterpolationFactors;
        private InterpolationFactors handsLayerInterpolationFactors;

        private Grenade grenade;
        private bool isThrowingGrenade;

        private bool isMoving = false;
        private bool isShooting = false;

        private int layerIndexHands;
        private int layerIndexShooting;
        private int layerIndexGrenadeThrow;
        private int layerIndexReload;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            networkAnimator = GetComponent<NetworkAnimator>();

            for (int i = 0; i < numberOfParamsToSync; i++)
            {
                networkAnimator.SetParameterAutoSend(i, true);
            }

            InitializeInterpolationFactors();

            InitializeEvents();

            isMoving = false;
            SwitchMovingState(isMoving);

            InitialializeLayerIndex();
        }

        private void InitializeInterpolationFactors()
        {
            directionInterpolationFactors = new InterpolationFactors
            {
                AnimatorValueName = ValueNameDirectionFactors,
                DiffFactor = 0.5f,
                InterpolatingPerFrameValue = 0.02f
            };

            angleInterpolationFactors = new InterpolationFactors
            {
                AnimatorValueName = ValueNameAngle,
                DiffFactor = 2,
                InterpolatingPerFrameValue = 0.02f
            };

            shootingLayerInterpolationFactors = new InterpolationFactors
            {
                AnimatorValueName = LayerNameShooting,
                InterpolatingPerFrameValue = 0.02f
            };

            handsLayerInterpolationFactors = new InterpolationFactors
            {
                AnimatorValueName = LayerNameHands,
                InterpolatingPerFrameValue = 0.02f
            };
        }

        private void InitializeEvents()
        {
            AnimationEvent shootingEvent = new AnimationEvent
            {
                time = shootingAnimation.length,
                functionName = FunctionNameEndShootingEvent
            };
            shootingAnimation.AddEvent(shootingEvent);

            AnimationEvent grenadeReleaseEvent = new AnimationEvent
            {
                time = grenadeAnimation.length * 0.42f,
                functionName = FunctionNameReleaseGrenadeEvent
            };
            grenadeAnimation.AddEvent(grenadeReleaseEvent);

            AnimationEvent grenadeThrowingEnd = new AnimationEvent
            {
                time = grenadeAnimation.length,
                functionName = FunctionNameEndThrowingGrenadeAnimationEvent
            };
            grenadeAnimation.AddEvent(grenadeThrowingEnd);

            AnimationEvent reloadOverEvent = new AnimationEvent
            {
                time = reloadAnimation.length,
                functionName = FunctionNameEndReloadingAnimationEvent
            };
            reloadAnimation.AddEvent(reloadOverEvent);
        }

        private void InitialializeLayerIndex()
        {
            layerIndexHands = animator.GetLayerIndex(LayerNameHands);
            layerIndexShooting = animator.GetLayerIndex(LayerNameShooting);
            layerIndexGrenadeThrow = animator.GetLayerIndex(LayerNameGrenadeThrow);
            layerIndexReload = animator.GetLayerIndex(LayerNameReload);
        }

        public void UpdateAnimator()
        {
            float viewMagn = ViewDirection.magnitude;
            float mouvMagn = MouvementDirection.magnitude;
            if (viewMagn != 0 && mouvMagn != 0 && isMoving == true)
            {
                if (animator.GetBool(ValueNameIsMoving) == false)
                {
                    SwitchMovingState(isMoving);
                    BeginInterpolation(1, ref handsLayerInterpolationFactors);
                }

                double cosTheta = Vector3.Dot(MouvementDirection, ViewDirection) / (mouvMagn * viewMagn);

                cosTheta = cosTheta < -1 ? -1 : cosTheta;
                cosTheta = cosTheta > 1 ? 1 : cosTheta;
                double angle = Math.Acos(cosTheta);

                angleInterpolationFactors.PresentFrame = Mathf.Rad2Deg * (float)angle;

                directionInterpolationFactors.PresentFrame = Vector3.Dot(new Vector3(MouvementDirection.z * -1, 0, MouvementDirection.x).normalized, ViewDirection.normalized);

                InterpolateAnimation(ref angleInterpolationFactors);
                InterpolateAnimation(ref directionInterpolationFactors);
            }
            else
            {
                if (animator.GetBool(ValueNameIsMoving) == true)
                {
                    SwitchMovingState(isMoving);
                    BeginInterpolation(0, ref handsLayerInterpolationFactors);
                }
            }
            InterpolateLayerWeight(ref shootingLayerInterpolationFactors);
            InterpolateLayerWeight(ref handsLayerInterpolationFactors);
        }

        private void InterpolateAnimation(ref InterpolationFactors factors)
        {
            if (Mathf.Abs(factors.LastFrame - factors.PresentFrame) > factors.DiffFactor)
            {
                BeginInterpolation(factors.PresentFrame, ref factors);
            }

            if (factors.IsInterpolating == true)
            {
                factors.Actual = Mathf.Lerp(factors.Begin, factors.End, factors.Interpolant);
                animator.SetFloat(factors.AnimatorValueName, factors.Actual);
                factors.Interpolant += factors.InterpolatingPerFrameValue;
                if (factors.Interpolant > 1 || factors.Interpolant < 0)
                {
                    factors.IsInterpolating = false;
                }
            }

            factors.LastFrame = factors.PresentFrame;
        }

        private void InterpolateLayerWeight(ref InterpolationFactors factors)
        {
            if (factors.IsInterpolating == true)
            {
                factors.Actual = Mathf.Lerp(factors.Begin, factors.End, factors.Interpolant);
                animator.SetLayerWeight(animator.GetLayerIndex(factors.AnimatorValueName), factors.Actual);
                factors.Interpolant += factors.InterpolatingPerFrameValue;
                if (factors.Interpolant > 1 || factors.Interpolant < 0)
                {
                    factors.IsInterpolating = false;
                }
            }

            factors.LastFrame = factors.PresentFrame;
        }

        private void BeginInterpolation(float end, ref InterpolationFactors factors)
        {
            factors.Begin = factors.Actual;
            factors.End = end;
            factors.Interpolant = 0;
            factors.IsInterpolating = true;
        }

        public void Shoot()
        {
            shootingLayerInterpolationFactors.IsInterpolating = false;
            CmdShoot();
        }

        public void OnSpeedChange(float newSpeed)
        {
            float animationSpeed = newSpeed / NormalPlayerSpeed;
            animator.SetFloat(ValueNameSpeed, animationSpeed);
        }

        public void OnBeginMoving()
        {
            isMoving = true;
            SwitchMovingState(isMoving);

            BeginInterpolation(1, ref handsLayerInterpolationFactors);
        }

        public void OnStopMoving()
        {
            isMoving = false;
            SwitchMovingState(isMoving);

            BeginInterpolation(0, ref handsLayerInterpolationFactors);
        }

        private void SwitchMovingState(bool isMoving)
        {
            animator.SetBool(ValueNameIsMoving, isMoving);
            animator.SetLayerWeight(layerIndexHands, Convert.ToSingle(isMoving));
        }

        public void Reload()
        {
            CmdReload();
        }

        [Command]
        private void CmdReload()
        {
            RpcReload();
        }

        [ClientRpc]
        private void RpcReload()
        {
            animator.SetLayerWeight(layerIndexReload, 1);
            animator.Play(AnimationNameReload, -1, 0f);
        }

        private void EndReloadingAnimationEvent()
        {
            animator.SetLayerWeight(layerIndexReload, 0);
        }

        [Command]
        private void CmdShoot()
        {
            RpcShoot();
        }

        [ClientRpc]
        private void RpcShoot()
        {
            animator.SetLayerWeight(layerIndexShooting, 1);
            animator.Play(AnimationNameShoot, -1, 0f);
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
                animator.SetLayerWeight(layerIndexGrenadeThrow, 1);
                animator.SetLayerWeight(layerIndexHands, 0);
                animator.Play(AnimationNameThrowGrenade, -1, 0f);
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
            shootingLayerInterpolationFactors.IsInterpolating = true;
            shootingLayerInterpolationFactors.Begin = 1;
            shootingLayerInterpolationFactors.End = 0;
            shootingLayerInterpolationFactors.Interpolant = 0;
        }

        private void ReleaseGrenadeEvent()
        {
            if (isLocalPlayer && grenade != null)
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
            animator.SetLayerWeight(layerIndexGrenadeThrow, 0);
            animator.SetLayerWeight(layerIndexHands, 1);
            isThrowingGrenade = false;
        }



        private struct InterpolationFactors
        {
            public float Begin;
            public float End;
            public float Actual;
            public float Interpolant;
            public bool IsInterpolating;
            public float LastFrame;
            public float PresentFrame;
            public float DiffFactor;
            public string AnimatorValueName;
            public float InterpolatingPerFrameValue;
        }
    }
}


