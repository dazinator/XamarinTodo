using System;

using Android.App;

namespace Todo.Services
{
    public interface IDroidViewLifecycleManager : IViewLifecycleManager, Application.IActivityLifecycleCallbacks
    {
        event EventHandler<ViewLifetimeEventArgs> LifetimeChanged;
        Activity GetCurrentActivity();
        int ActivityCount { get; }
    }
}