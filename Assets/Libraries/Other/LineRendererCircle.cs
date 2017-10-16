//Author: MarkPixel
//https://forum.unity.com/threads/linerenderer-to-create-an-ellipse.74028/

using UnityEngine;

namespace ProjetSynthese
{
    public class LineRendererCircle : GameScript
    {
        [SerializeField] private CircleInfo circleInfo;

        public float Radius { get; set; }

        public void Create()
        {
            float x;
            float y;
            float z = 0f;

            float angle = circleInfo.Angle;
            int segments = circleInfo.Segment;

            LineRenderer line = gameObject.GetComponent<LineRenderer>();

            line.positionCount = segments + 1;
            line.useWorldSpace = false;

            for (int i = 0; i < (segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * Radius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * Radius;

                line.SetPosition(i, new Vector3(x, y, z));

                angle += (360f / segments);
            }
        }
    }
}
