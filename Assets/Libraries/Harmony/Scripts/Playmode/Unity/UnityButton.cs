using Harmony.Testing;
using UnityEngine.UI;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un Button Unity.
    /// </summary>
    /// <inheritdoc cref="IButton"/>
    [NotTested(Reason.Wrapper)]
    public class UnityButton : UnitySelectable, IButton
    {
        private readonly Button button;
        private UnityButtonEventWrapper eventWrapper;

        public event ButtonClickedEventHandler OnClicked
        {
            add
            {
                EventWrapper.OnClickedEvent += value;
            }
            remove
            {
                if (CanRemoveEventSafely()) EventWrapper.OnClickedEvent -= value;
            }
        }

        public UnityButton(Button button) : base(button)
        {
            this.button = button;
        }

        private UnityButtonEventWrapper EventWrapper
        {
            get
            {
                //Try getting it first, then create it if it doesnt exist.
                if (eventWrapper == null)
                {
                    eventWrapper = button.gameObject.GetComponent<UnityButtonEventWrapper>();
                }
                if (eventWrapper == null)
                {
                    eventWrapper = button.gameObject.AddComponent<UnityButtonEventWrapper>();
                    eventWrapper.SetButton(button);
                }
                return eventWrapper;
            }
        }

        private bool CanRemoveEventSafely()
        {
            return eventWrapper != null;
        }

        public class UnityButtonEventWrapper : UnityScript
        {
            public event ButtonClickedEventHandler OnClickedEvent;

            public void SetButton(Button button)
            {
                button.onClick.AddListener(OnClick);
            }

            private void OnClick()
            {
                if (OnClickedEvent != null) OnClickedEvent();
            }
        }
    }
}