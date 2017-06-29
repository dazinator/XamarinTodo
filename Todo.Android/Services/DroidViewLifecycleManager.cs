using System;

using Android.App;
using System.Collections.Concurrent;

namespace Todo.Services
{

    /// <summary>
    /// This class implements IActivityLifecycleCallbacks and is registered on application startup to 
    /// listen for activity lifecycle events - which it then uses to track the Activities, including which Activity is Current.
    /// </summary>
    public class DroidViewLifecycleManager : Java.Lang.Object, IDroidViewLifecycleManager
    {

        public event EventHandler<ViewLifetimeEventArgs> LifetimeChanged;

        public DroidViewLifecycleManager()
        {

        }

        private ConcurrentDictionary<string, ActivityInfo> _Activities = new ConcurrentDictionary<string, ActivityInfo>();

        protected string GetActivityName(Activity activity)
        {
            return activity.Class.SimpleName;
        }

        public Activity GetCurrentActivity()
        {
            if (_Activities.Count > 0)
            {
                var e = _Activities.GetEnumerator();
                while (e.MoveNext())
                {
                    var current = e.Current;
                    if (current.Value.IsCurrent)
                    {
                        return current.Value.Activity;
                    }
                }

                // var key = _Activities..;
            }

            return null;
        }

        public int ActivityCount
        {
            get { return _Activities.Count; }
        }

        #region IActivityLifecycleCallbacks

        public void OnActivityCreated(Activity activity, global::Android.OS.Bundle savedInstanceState)
        {
            var activityName = GetActivityName(activity);
            _Activities.GetOrAdd(activityName, new ActivityInfo() { Activity = activity, IsCurrent = true });
            FireLifetimeChanged(activity, LifecycleEventType.Created);
            //  Mvx.Trace(MvxTraceLevel.Diagnostic, "CREATED Activity: {0} - CURRENT", activityName);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            ActivityInfo removed;
            var activityName = GetActivityName(activity);
            _Activities.TryRemove(activityName, out removed);
            FireLifetimeChanged(activity, LifecycleEventType.Destroyed);
            // Mvx.Trace(MvxTraceLevel.Diagnostic, "DESTROYED Activity: {0} - REMOVED", activityName);
        }

        public void OnActivityPaused(Activity activity)
        {
            ActivityInfo forAdd = new ActivityInfo { Activity = activity, IsCurrent = false };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, forAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = false;
                return existing;
            });
            FireLifetimeChanged(activity, LifecycleEventType.Paused);
            //  Mvx.Trace(MvxTraceLevel.Diagnostic, "PAUSED Activity: {0} - CURRENT", activityName);
        }

        public void OnActivityResumed(Activity activity)
        {
            ActivityInfo forAdd = new ActivityInfo { Activity = activity, IsCurrent = true };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, forAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = true;
                return existing;
            });
            FireLifetimeChanged(activity, LifecycleEventType.Resumed);
            // Mvx.Trace(MvxTraceLevel.Diagnostic, "RESUMED Activity: {0} - CURRENT", activityName);
        }

        public void OnActivitySaveInstanceState(Activity activity, global::Android.OS.Bundle outState)
        {
            var activityName = GetActivityName(activity);
            // Mvx.Trace(MvxTraceLevel.Diagnostic, "SAVEDINSTANCESTATE Activity: {0} - CURRENT", activityName);
            // throw new NotImplementedException();
        }

        public void OnActivityStarted(Activity activity)
        {
            ActivityInfo forAdd = new ActivityInfo { Activity = activity, IsCurrent = true };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, forAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = true;
                return existing;
            });
            FireLifetimeChanged(activity, LifecycleEventType.Started);
            // Mvx.Trace(MvxTraceLevel.Diagnostic, "STARTED Activity: {0} - CURRENT", activityName);
        }

        public void OnActivityStopped(Activity activity)
        {
            ActivityInfo forAdd = new ActivityInfo { Activity = activity, IsCurrent = false };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, forAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = false;
                return existing;
            });
            FireLifetimeChanged(activity, LifecycleEventType.Stopped);
            // Mvx.Trace(MvxTraceLevel.Diagnostic, "STOPPED Activity: {0} - NOT CURRENT", activityName);
        }

        #endregion

        protected virtual void FireLifetimeChanged(Activity activity, LifecycleEventType eventType)
        {
            if (LifetimeChanged != null)
            {
                var args = new ViewLifetimeEventArgs(activity, eventType);
                LifetimeChanged(this, args);
            }
        }

        #region Nested Helper Class

        /// <summary>
        /// Used to store additional info along with an activity.
        /// </summary>
        private class ActivityInfo
        {
            public bool IsCurrent { get; set; }
            public Activity Activity { get; set; }
        }

        #endregion


    }
}