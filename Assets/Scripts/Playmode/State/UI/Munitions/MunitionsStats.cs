using Harmony;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class MunitionsStats : GameScript
    {
        private Text text;
        private MunitionChangeEventChannel munitionChangeEventChannel;

        private void InjectMinitionsStats([GameObjectScope] Text text,
            [EventChannelScope] MunitionChangeEventChannel munitionChangeEventChannel)
        {
            this.text = text;
            this.munitionChangeEventChannel = munitionChangeEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectMinitionsStats");
            munitionChangeEventChannel.OnEventPublished += OnMunitionChanged;
        }

        private void OnMunitionChanged(MunitionChangeEvent munitionChangeEvent)
        {
            text.text = munitionChangeEvent.Munitions + " / " + munitionChangeEvent.MunitionsMax;
        }
    }
}