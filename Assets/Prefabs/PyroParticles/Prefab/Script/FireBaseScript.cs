using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using ProjetSynthese;
using UnityEngine.Networking;

namespace DigitalRuby.PyroParticles
{
    [System.Serializable]
    public struct RangeOfIntegers
    {
        public int Minimum;
        public int Maximum;
    }

    [System.Serializable]
    public struct RangeOfFloats
    {
        public float Minimum;
        public float Maximum;
    }

    public class FireBaseScript : MonoBehaviour
    {
        [Tooltip("Optional audio source to play once when the script starts.")]
        public AudioSource AudioSource;

        [Tooltip("How long the script takes to fully start. This is used to fade in animations and sounds, etc.")]
        public float StartTime = 1.0f;

        [Tooltip("How long the script takes to fully stop. This is used to fade out animations and sounds, etc.")]
        public float StopTime = 3.0f;

        [Tooltip("How long the effect lasts. Once the duration ends, the script lives for StopTime and then the object is destroyed.")]
        public float Duration = 2.0f;

        [Tooltip("How much force to create at the center (explosion), 0 for none.")]
        public float ForceAmount;

        [Tooltip("The radius of the force, 0 for none.")]
        public float ForceRadius;

        [Tooltip("A hint to users of the script that your object is a projectile and is meant to be shot out from a person or trap, etc.")]
        public bool IsProjectile;

        [SerializeField]
        public List<string> excludedLayers = new List<string>();

        [Tooltip("Particle systems that must be manually started and will not be played on start.")]
        public ParticleSystem[] ManualParticleSystems;



        private float startTimeMultiplier;
        private float startTimeIncrement;

        private float stopTimeMultiplier;
        private float stopTimeIncrement;

        private IEnumerator CleanupEverythingCoRoutine()
        {
            // 2 extra seconds just to make sure animation and graphics have finished ending
            yield return new WaitForSeconds(StopTime + 2.0f);

            Destroy(gameObject);
        }

        private void StartParticleSystems()
        {
            foreach (ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                if (ManualParticleSystems == null || ManualParticleSystems.Length == 0 ||
                    System.Array.IndexOf(ManualParticleSystems, p) < 0)
                {
                    if (p.startDelay == 0.0f)
                    {
                        // wait until next frame because the transform may change
                        p.startDelay = 0.01f;
                    }
                    p.Play();
                }
            }
        }

        protected virtual void Awake()
        {
            Starting = true;
            //int fireLayer = UnityEngine.LayerMask.NameToLayer("FireLayer");
            //UnityEngine.Physics.IgnoreLayerCollision(fireLayer, fireLayer);
        }

        protected virtual void Start()
        {
            if (AudioSource != null)
            {
                AudioSource.Play();
            }

            // precalculate so we can multiply instead of divide every frame
            stopTimeMultiplier = 1.0f / StopTime;
            startTimeMultiplier = 1.0f / StartTime;

            // if this effect has an explosion force, apply that now
            CreateExplosion(gameObject.transform.position, ForceRadius, ForceAmount, excludedLayers);

            // start any particle system that is not in the list of manual start particle systems
            StartParticleSystems();

            // If we implement the ICollisionHandler interface, see if any of the children are forwarding
            // collision events. If they are, hook into them.
            ICollisionHandler handler = (this as ICollisionHandler);
            if (handler != null)
            {
                FireCollisionForwardScript collisionForwarder = GetComponentInChildren<FireCollisionForwardScript>();
                if (collisionForwarder != null)
                {
                    collisionForwarder.CollisionHandler = handler;
                }
            }
        }

        protected virtual void Update()
        {
            // reduce the duration
            Duration -= Time.deltaTime;
            if (Stopping)
            {
                // increase the stop time
                stopTimeIncrement += Time.deltaTime;
                if (stopTimeIncrement < StopTime)
                {
                    StopPercent = stopTimeIncrement * stopTimeMultiplier;
                }
            }
            else if (Starting)
            {
                // increase the start time
                startTimeIncrement += Time.deltaTime;
                if (startTimeIncrement < StartTime)
                {
                    StartPercent = startTimeIncrement * startTimeMultiplier;
                }
                else
                {
                    Starting = false;
                }
            }
            else if (Duration <= 0.0f)
            {
                // time to stop, no duration left
                Stop();
            }
        }

        public static void CreateExplosion(Vector3 pos, float radius, float force, IList<string> excludedLayers)
        {
            if (force <= 0.0f || radius <= 0.0f)
            {
                return;
            }

            LayerMask layerMask;
            if (excludedLayers.Count > 0)
            {
                layerMask = (1 << LayerMask.NameToLayer(excludedLayers[0]));
                for (int i = 1; i < excludedLayers.Count; i++)
                {
                    layerMask = (1 << LayerMask.NameToLayer(excludedLayers[i])) | layerMask;
                }
            }
            else
            {
                layerMask = new LayerMask();
            }
            CreateExplosionOverlapSphere(pos, radius, force, layerMask);
        }

        private static void CreateExplosionOverlapSphere(Vector3 pos, float radius, float force, int layerMask)
        {
            // find all colliders and add explosive force
            Collider[] objects = UnityEngine.Physics.OverlapSphere(pos, radius, layerMask);
            foreach (Collider collider in objects)
            {
                Rigidbody r = collider.GetComponent<Rigidbody>();
                if (r != null)
                {
                    bool isAI = false;

                    r.AddExplosionForce(force, pos, radius);
                    if (collider.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Player) || collider.gameObject.layer == LayerMask.NameToLayer(R.S.Layer.Ai))
                    {
                        Health health = collider.gameObject.GetComponentInChildren<Health>();

                        IInventory inventoryController = collider.gameObject.GetComponentInChildren<ProjetSynthese.PlayerController>();
                        if (inventoryController == null)
                        {
                            inventoryController = collider.gameObject.GetComponentInChildren<ActorAI>();
                            isAI = true;
                        }
                        if (health != null && collider.gameObject.GetComponent<NetworkBehaviour>().isLocalPlayer)
                        {
                            Item[] protectionItems = inventoryController.GetProtections();
                            float helmetProtection = protectionItems[0] == null ? 0 : ((Helmet)protectionItems[0]).ProtectionValue;
                            float vestProtection = protectionItems[1] == null ? 0 : ((Vest)protectionItems[1]).ProtectionValue;
                            float dist = 95 - Vector3.Distance(collider.transform.position, pos);
                            health.Hit(dist - (dist * ((helmetProtection + vestProtection) / 100)), isAI); 
                        }
                    }
                }
            }
        }

        public virtual void Stop()
        {
            if (Stopping)
            {
                return;
            }
            Stopping = true;

            // cleanup particle systems
            foreach (ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }

            StartCoroutine(CleanupEverythingCoRoutine());
        }

        public bool Starting
        {
            get;
            private set;
        }

        public float StartPercent
        {
            get;
            private set;
        }

        public bool Stopping
        {
            get;
            private set;
        }

        public float StopPercent
        {
            get;
            private set;
        }
    }
}