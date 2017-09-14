using Harmony.Testing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente une Selectable Unity.
    /// </summary>
    /// <inheritdoc cref="ISelectable"/>
    [NotTested(Reason.Wrapper)]
    public class UnitySelectable : UnityDisableable, ISelectable
    {
        private readonly Selectable selectable;
        private UnitySelectableEventWrapper eventWrapper;

        public event SelectedEventHandler OnSelected
        {
            add { EventWrapper.OnSelectedEvent += value; }
            remove
            {
                if (CanRemoveEventSafely()) EventWrapper.OnSelectedEvent -= value;
            }
        }

        public UnitySelectable(Selectable selectable) : base(selectable)
        {
            this.selectable = selectable;
        }

        public void Click()
        {
            ExecuteEvents.Execute(selectable.gameObject,
                                  new BaseEventData(UnityEngine.EventSystems.EventSystem.current),
                                  ExecuteEvents.submitHandler);
        }

        public void Select()
        {
            Deselect(); //#Dirty Hack : Sometimes, Select method does not work. We need to deselect it to make it work.
            selectable.Select();
        }

        public void Deselect()
        {
            if (IsSelected())
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private bool IsSelected()
        {
            return UnityEngine.EventSystems.EventSystem.current != null &&
                   UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == selectable.gameObject;
        }

        public ISelectable SelectNext()
        {
            Selectable nextSelectable = selectable.navigation.selectOnDown;
            if (nextSelectable != null)
            {
                nextSelectable.Select();
                return new UnitySelectable(nextSelectable);
            }
            return null;
        }

        public ISelectable SelectPrevious()
        {
            Selectable previousSelectable = selectable.navigation.selectOnUp;
            if (previousSelectable != null)
            {
                previousSelectable.Select();
                return new UnitySelectable(previousSelectable);
            }
            return null;
        }

        private UnitySelectableEventWrapper EventWrapper
        {
            get
            {
                //Try getting it first, then create it if it doesnt exist.
                if (eventWrapper == null)
                {
                    eventWrapper = selectable.gameObject.GetComponent<UnitySelectableEventWrapper>();
                }
                if (eventWrapper == null)
                {
                    eventWrapper = selectable.gameObject.AddComponent<UnitySelectableEventWrapper>();
                }
                return eventWrapper;
            }
        }

        private bool CanRemoveEventSafely()
        {
            return eventWrapper != null;
        }

        public static bool operator ==(UnitySelectable a, UnitySelectable b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            if (ReferenceEquals(a.selectable, b.selectable))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(UnitySelectable a, UnitySelectable b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UnitySelectable))
            {
                return false;
            }
            return Equals((UnitySelectable) obj);
        }

        private bool Equals(UnitySelectable other)
        {
            return selectable.Equals(other.selectable);
        }

        public override int GetHashCode()
        {
            return selectable != null ? selectable.GetHashCode() : 0;
        }

        public class UnitySelectableEventWrapper : UnityScript, ISelectHandler
        {
            public event SelectedEventHandler OnSelectedEvent;

            public void OnSelect(BaseEventData eventData)
            {
                if (OnSelectedEvent != null) OnSelectedEvent();
            }
        }
    }
}