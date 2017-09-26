using UnityEngine;

namespace Harmony
{
    [AddComponentMenu("Game/Input/Mouse")]
    public class Mouse : Script
    {
        public bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }

        public bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }
    }
}
