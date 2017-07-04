using Android.App;
using Android.OS;
using Android.Accounts;
using System;
using Android.Widget;
using System.Threading.Tasks;
using Todo.Services;
using Android.Content;
using System.Linq;
using System.Collections.Generic;

namespace Todo
{
    public class AndroidAccountService : IAccountService
    {

        //  private readonly Activity _activity;
        private Handler _handler;

        private readonly IAndroidCurrentTopActivity _currentTop;
        private const int Create_Account_Request_Code = 1;  // The request code

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


                     //  var addOptions = new Bundle();
                     //  var handler = new Handler(new HandlerCallback());
                     //  var accounts = accountManager.GetAccountsByType(accountType);
                     //  var firstAccount = accounts.First();

                     var callback = new AccountManagerCallback();
                     //  var authToken = accountManager.GetAuthToken(firstAccount, authTokenType, null, activity, callback, null);

                     var future = accountManager.AddAccount(accountType, authTokenType, null, null, activity, callback, null);
                     var bundle = future.Result as Bundle;
                     //if (bundle != null)
                     //{
                     //    var intent = bundle.GetParcelable(AccountManager.KeyIntent) as Intent;
                     //    activity.StartActivityForResult(intent, Create_Account_Request_Code);
                     //}

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

        public Task SetAccount(IUserAccount account)
        {
            //CurrentAndroidUserAccount = account as AndroidUserAccount;
            CurrentUserAccount = account;
            return Task.FromResult(true);
        }

        public IUserAccount CurrentUserAccount { get; private set; }

        public Task<IEnumerable<IUserAccount>> GetAccounts(string accountType)
        {
            return Task.Run<IEnumerable<IUserAccount>>(() =>
            {
                // Looper.Prepare();
                var results = new List<IUserAccount>();
                var activity = _currentTop.Activity;


                Handler handler = null;
                try
                {
                    if (activity == null)
                    {
                        throw new Exception("No current activity");
                    }

                    var accountManager = AccountManager.Get(activity);
                    var accounts = accountManager.GetAccountsByType(accountType);

                    foreach (var item in accounts)
                    {
                        var account = new AndroidUserAccount(item);
                        results.Add(account);
                    }
                    return results;
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
                return results;

            });

        }

        public Task<string> GetRefreshToken(IUserAccount account)
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
                    var androidAccount = account as AndroidUserAccount;
                    //var callback = new AccountManagerCallback();
                    //  var authToken = accountManager.GetAuthToken(firstAccount, authTokenType, null, activity, callback, null);
                    var refreshToken = accountManager.GetPassword(androidAccount.Account);

                    return refreshToken;
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

                    return string.Empty;
                }

            });
        }

        public Task SetRefreshToken(IUserAccount account, string refreshToken)
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
                    var androidAccount = account as AndroidUserAccount;                 
                    accountManager.SetPassword(androidAccount.Account, refreshToken);
                   
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
    }

}





