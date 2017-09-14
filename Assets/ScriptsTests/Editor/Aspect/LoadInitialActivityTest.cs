using Harmony;
using Harmony.Testing;
using Harmony.Unity;
using NUnit.Framework;
using NSubstitute;

namespace ProjetSynthese
{
    public class LoadInitialActivityTest : UnitTestCase
    {
        private IActivityStack activityStack;
        private LoadInitialActivity loadInitialActivity;

        [SetUp]
        public void Before()
        {
            activityStack = CreateSubstitute<IActivityStack>();
            loadInitialActivity = CreateBehaviour<LoadInitialActivity>();
        }

        [Test]
        public void StartActivityAtStart()
        {
            UnityActivity activity = CreateSubstitute<UnityActivity>();

            LoadActivity(activity);

            HasStartedActivity(activity);
        }

        private void LoadActivity(UnityActivity activity)
        {
            loadInitialActivity.InjectLoadInitialActivity(activity, activityStack);
            loadInitialActivity.Awake();
            loadInitialActivity.Start();
        }

        private void HasStartedActivity(UnityActivity activity)
        {
            activityStack.Received().StartActivity(activity);
        }
    }
}