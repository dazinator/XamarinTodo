
using Android.App;

namespace Todo.Services
{
    public class AndroidCurrentTopActivity : IAndroidCurrentTopActivity
    {
        private IDroidViewLifecycleManager _lifecycleManager;

        public AndroidCurrentTopActivity(IDroidViewLifecycleManager lifecycleManager)
        {
            _lifecycleManager = lifecycleManager;
        }

        public Activity Activity
        {
            get
            {
                return _lifecycleManager.GetCurrentActivity();
            }
        }

        public IDroidViewLifecycleManager LifecycleManager { get { return _lifecycleManager; } }
    }
}