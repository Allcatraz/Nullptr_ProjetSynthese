using UnityEngine;

namespace ProjetSynthese
{
    public static class Maths
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle <= -360F)
                angle += 360F;
            if (angle >= 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
