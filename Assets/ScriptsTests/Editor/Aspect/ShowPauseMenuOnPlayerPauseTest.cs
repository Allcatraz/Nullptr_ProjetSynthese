using Harmony;
using Harmony.EventSystem;
using Harmony.Testing;
using Harmony.Unity;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;

namespace ProjetSynthese
{
    public class ShowPauseMenuOnPlayerPauseTest : UnitTestCase
    {
        private UnityMenu pauseMenu;
        private PlayerInputSensor playerInputSensor;
        private ITime time;
        private IMenuStack menuStack;
        private IInputDevice playersInputDevice;
        private ShowPauseMenuOnPlayerPause showPauseMenuOnPlayerPause;

        [SetUp]
        public void Before()
        {
            pauseMenu = CreateSubstitute<UnityMenu>();
            playerInputSensor = CreateSubstitute<PlayerInputSensor>();
            time = CreateSubstitute<ITime>();
            menuStack = CreateSubstitute<IMenuStack>();
            playersInputDevice = CreateSubstitute<IInputDevice>();
            showPauseMenuOnPlayerPause = CreateBehaviour<ShowPauseMenuOnPlayerPause>();

            playerInputSensor.Players.Returns(playersInputDevice);
        }

        [Test]
        public void WhenEnabledRegistersToEvents()
        {
            Initialize();

            playersInputDevice.Received().OnTogglePause += Arg.Any<TogglePauseEventHandler>();
        }

        [Test]
        public void WhenDisabledUnRegistersToEvents()
        {
            Initialize();

            Disable();

            playersInputDevice.Received().OnTogglePause -= Arg.Any<TogglePauseEventHandler>();
        }

        [Test]
        public void OnTogglePauseIfTimeIsNotPausedShowPauseMenu()
        {
            Initialize();

            MakeTimeNotPaused();
            TogglePause();

            CheckPauseMenuShown();
        }

        [Test]
        public void OnTogglePauseIfTimeIsPausedDoNotShowPauseMenu()
        {
            Initialize();

            MakeTimePaused();
            TogglePause();

            CheckPauseMenuNotShown();
        }

        private void Initialize()
        {
            showPauseMenuOnPlayerPause.InjectShowPauseMenuOnPlayerPause(pauseMenu,
                                                                        playerInputSensor,
                                                                        time,
                                                                        menuStack);
            showPauseMenuOnPlayerPause.Awake();
            showPauseMenuOnPlayerPause.OnEnable();
        }

        private void Disable()
        {
            showPauseMenuOnPlayerPause.OnDisable();
        }

        private void MakeTimePaused()
        {
            time.IsPaused().Returns(true);
        }

        private void MakeTimeNotPaused()
        {
            time.IsPaused().Returns(false);
        }

        private void TogglePause()
        {
            playersInputDevice.OnTogglePause += Raise.Event<TogglePauseEventHandler>();
        }

        private void CheckPauseMenuShown()
        {
            menuStack.Received().StartMenu(pauseMenu);
        }

        private void CheckPauseMenuNotShown()
        {
            menuStack.Received(0).StartMenu(pauseMenu);
        }
    }
}