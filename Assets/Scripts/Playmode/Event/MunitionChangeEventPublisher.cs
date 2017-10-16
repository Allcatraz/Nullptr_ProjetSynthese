using Harmony;

namespace ProjetSynthese
{
    public class MunitionChangeEventPublisher : GameScript
    {
        private Weapon weapon;
        private MunitionChangeEventChannel munitionChangeEventChannel;

        private void InjectMunitionChangeEventPublisher([EntityScope] Weapon weapon,
                                                        [EventChannelScope] MunitionChangeEventChannel munitionChangeEventChannel)
        {
            this.munitionChangeEventChannel = munitionChangeEventChannel;
            this.weapon = weapon;
        }

        private void Awake()
        {
            InjectDependencies("InjectMunitionChangeEventPublisher");
        }

        private void OnEnable()
        {
            weapon.OnMunitionChanged += OnMunitionChanged;
        }

        private void OnDisable()
        {
            weapon.OnMunitionChanged -= OnMunitionChanged;
        }

        private void OnMunitionChanged()
        {
            munitionChangeEventChannel.Publish(new MunitionChangeEvent(weapon.MagazineAmount, weapon.MagazineMax));
        }
    }
}
