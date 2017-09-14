using Harmony.Testing;
using UnityEngine;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Collider2D Unity.
    /// </summary>
    /// <inheritdoc cref="ICollider2D"/>
    [NotTested(Reason.Wrapper)]
    public class UnityCollider2D : UnityDisableable, ICollider2D
    {
        private readonly Collider2D collider2D;
        private UnityCollider2DEventWrapper eventWrapper;

        public event Collider2DTriggerEventHandler OnTriggerEntered
        {
            add { EventWrapper.OnTriggerEnteredEvent += value; }
            remove { EventWrapper.OnTriggerEnteredEvent -= value; }
        }

        public event Collider2DTriggerEventHandler OnTriggerExited
        {
            add { EventWrapper.OnTriggerExitedEvent += value; }
            remove { EventWrapper.OnTriggerExitedEvent -= value; }
        }

        public bool IsTrigger
        {
            get { return collider2D.isTrigger; }
            set { collider2D.isTrigger = value; }
        }

        public UnityCollider2D(Collider2D collider2D) : base(collider2D)
        {
            this.collider2D = collider2D;
        }

        public T GetOtherComponent<T>() where T : class
        {
            return collider2D.GetComponent<T>();
        }

        private UnityCollider2DEventWrapper EventWrapper
        {
            get
            {
                //Try getting it first, then create it if it doesnt exist.
                if (eventWrapper == null)
                {
                    eventWrapper = collider2D.gameObject.GetComponent<UnityCollider2DEventWrapper>();
                }
                if (eventWrapper == null)
                {
                    eventWrapper = collider2D.gameObject.AddComponent<UnityCollider2DEventWrapper>();
                }
                return eventWrapper;
            }
        }

        public class UnityCollider2DEventWrapper : UnityScript
        {
            public event Collider2DTriggerEventHandler OnTriggerEnteredEvent;
            public event Collider2DTriggerEventHandler OnTriggerExitedEvent;

            public void OnTriggerEnter2D(Collider2D other)
            {
                if (OnTriggerEnteredEvent != null) OnTriggerEnteredEvent(new UnityCollider2D(other));
            }

            public void OnTriggerExit2D(Collider2D other)
            {
                if (OnTriggerExitedEvent != null) OnTriggerExitedEvent(new UnityCollider2D(other));
            }
        }
    }
}