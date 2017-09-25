using UnityEditor;

namespace ProjetSynthese
{
    [CustomEditor(typeof(SqLiteConnectionFactory), true)]
    public class SqLiteConnectionFactoryInspector : GameScriptInspector
    {
        private SqLiteConnectionFactory sqLiteConnectionFactory;

        private void Awake()
        {
            sqLiteConnectionFactory = target as SqLiteConnectionFactory;
        }

        private void OnDestroy()
        {
            sqLiteConnectionFactory = null;
        }

        protected override void OnDraw()
        {
            base.OnDraw();

            DrawButton("Reset Database", sqLiteConnectionFactory.ResetDatabase);
        }
    }
}