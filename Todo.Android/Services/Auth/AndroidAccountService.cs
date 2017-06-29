using Android.App;
using Android.OS;
using Android.Accounts;
using System;
using Android.Widget;
using System.Threading.Tasks;
using Todo.Services;
using Android.Content;

namespace Todo
{
    public class AndroidAccountService : IAccountService
    {

        //  private readonly Activity _activity;
        private Handler _handler;

        private readonly IAndroidCurrentTopActivity _currentTop;

        public AndroidAccountService(IAndroidCurrentTopActivity currentTop)
        {
            _currentTop = currentTop;


        }

        public Task AddAccount(string accountType, string authTokenType)
        {

            return Task.Run(() =>
             {
                 // Looper.Prepare();
                 var activity = _currentTop.Activity;
                 Handler handler = null;
                 try
                 {
                     if (activity == null)
                     {
                         throw new Exception("No current activity");
                     }

                   
                     var accountManager = AccountManager.Get(activity);
                  

                     //  var callback = new AccountManagerCallback();
                     //  var addOptions = new Bundle();
                     //  var handler = new Handler(new HandlerCallback());

                     var future = accountManager.AddAccount(accountType, authTokenType, null, null, null, null, null);

                     var bundle = future.Result as Bundle;
                     if (bundle != null)
                     {
                         var intent = bundle.GetParcelable(AccountManager.KeyIntent) as Intent;
                         activity.StartActivity(intent);
                     }

                     // var result = future.Result;
                 }
                 catch (Exception e)
                 {
                     if (activity != null)
                     {
                         handler = new Handler(activity.MainLooper);                        
                         ShowMessage(handler, activity.BaseContext, e.Message);
                         handler.Dispose();
                         handler = null;
                     }

                 }

             });
        }

        private void ShowMessage(Handler handler, Context context, String msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }
            _handler.Post(() =>
            {
                Toast.MakeText(context, msg, ToastLength.Short).Show();
            });
        }


    }


    public class HandlerCallback : Java.Lang.Object, Handler.ICallback
    {

        public bool HandleMessage(Message msg)
        {
            throw new NotImplementedException();
        }
    }

}



