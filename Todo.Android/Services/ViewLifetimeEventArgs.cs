using System;

using Android.App;

namespace Todo.Services
{
    public class ViewLifetimeEventArgs : EventArgs
    {
        public ViewLifetimeEventArgs(Activity activity, LifecycleEventType lifecycleEvent)
        {
            LifecycleEvent = lifecycleEvent;
            Activity = activity;
        }

        public LifecycleEventType LifecycleEvent { get; private set; }

        public Activity Activity { get; set; }

    }
}