using Harmony.Testing;
using UnityEngine;
using UnityEngine.UI;

namespace Harmony.Unity
{
    /// <summary>
    /// Représente un MenuInput Unity.
    /// </summary>
    /// <inheritdoc cref="IMenuState"/>
    [NotTested(Reason.Wrapper)]
    public class UnityMenuState : IMenuState
    {
        public ISelectable CurrentSelected
        {
            get
            {
                UnityEngine.EventSystems.EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current;
                if (eventSystem != null)
                {
                    GameObject gameObject = eventSystem.currentSelectedGameObject;
                    if (gameObject != null)
                    {
                        Selectable selectable = gameObject.GetComponent<Selectable>();
                        if (selectable != null)
                        {
                            return new UnitySelectable(selectable);
                        }
                        return null;
                    }
                    return null;
                }
                return null;
            }
        }
    }
}