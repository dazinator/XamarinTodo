using Android.Accounts;
using System;

namespace Todo
{
    public class AccountManagerCallback : Java.Lang.Object, IAccountManagerCallback
    {

        public void Run(IAccountManagerFuture future)
        {
            try
            {
                var bundle = future.GetResult(1, Java.Util.Concurrent.TimeUnit.Minutes);
                Success = true;
            }
            catch (Exception e)
            {
                Success = false;
                Exception = e;
            }


        }

        public bool Success { get; set; }

        public Exception Exception { get; set; }

    }


}


