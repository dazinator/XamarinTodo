using Android.App;
using Android.OS;
using System;
using Android.Content;

namespace Todo
{
    [Service]
    [IntentFilter(new[] { "android.accounts.AccountAuthenticator" })]
    [MetaData("android.accounts.AccountAuthenticator", Resource = "@xml/authenticator")]
    [MetaData("android.accounts.AccountAuthenticator.customTokens", Value = "1")] // see https://stackoverflow.com/questions/15348105/checking-permissions-in-custom-account-authenticator
    public class AccountAuthenticatorService : Service
    {

        private static AccountAuthenticator sAccountAuthenticator = null;

       // public static string ACCOUNT_TYPE = "com.todo.auth_example";
        public static string ACCOUNT_NAME = "MyApp";
        //  private Context _context;

        public AccountAuthenticatorService() : base()
        {
            Console.WriteLine("Default Contructor");
        }

        public AccountAuthenticatorService(Context _context) : base()
        {
            Console.WriteLine("Secondary Constructor");
            //  this._context = _context;
        }

        public override IBinder OnBind(Intent intent)
        {
            Console.WriteLine("OnBind");
            IBinder ret = null;
            if (intent.Action == Android.Accounts.AccountManager.ActionAuthenticatorIntent)
                ret = getAuthenticator().IBinder;
            return ret;
        }

        private AccountAuthenticator getAuthenticator()
        {
            Console.WriteLine("getAuthenticator");
            if (sAccountAuthenticator == null)
                sAccountAuthenticator = new AccountAuthenticator(this);
            return sAccountAuthenticator;
        }
    }
}


