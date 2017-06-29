using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Accounts;
using System;
using Android.Content;
using Android.Widget;
using Android.Runtime;
using System.Threading.Tasks;

namespace Todo
{


    [Activity(Label = "Todo", Icon = "@drawable/icon", Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class AuthenticatorActivity : AccountAuthenticatorActivity
    {

        public const String ARG_ACCOUNT_TYPE = "ACCOUNT_TYPE";
        public const String ARG_AUTH_TYPE = "AUTH_TYPE";
        public const String ARG_ACCOUNT_NAME = "ACCOUNT_NAME";
        public const String ARG_IS_ADDING_NEW_ACCOUNT = "IS_ADDING_ACCOUNT";
        public const String PARAM_USER_PASS = "USER_PASS";
        public const String KEY_ERROR_MESSAGE = "ERR_MSG";

        private const int REQ_SIGNUP = 1;

        private AccountManager _AccountManager;
        private AuthTokenType _AuthType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            _AccountManager = AccountManager.Get(BaseContext);


            string accountName = Intent.GetStringExtra(ARG_ACCOUNT_NAME);
            var authType = Intent.GetStringExtra(ARG_AUTH_TYPE);

            if (!string.IsNullOrWhiteSpace(authType))
            {
                _AuthType = (AuthTokenType)Enum.Parse(typeof(AuthTokenType), authType);
            }
            else
            {
                _AuthType = AuthTokenType.FullAccess;
            }

            if (!string.IsNullOrWhiteSpace(accountName))
            {
                ((TextView)FindViewById(Resource.Id.accountName)).Text = accountName;
            }

            FindViewById<Button>(Resource.Id.submit).Click += async delegate
            {
                await Submit();
            };

            FindViewById<TextView>(Resource.Id.signUp).Click += SignUpButton_Click;


        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            SignUp();
        }

        private void SignUp()
        {
            Intent intent = new Intent(this.BaseContext, typeof(SignUpActivity));
            intent.PutExtras(Intent.Extras);

            StartActivityForResult(intent, REQ_SIGNUP);

        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == REQ_SIGNUP && resultCode == Result.Ok)
            {
                FinishLogin(data);
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }

        private void FinishLogin(Intent data)
        {
            var accountName = data.GetStringExtra(AccountManager.KeyAccountName);
            var accountPassword = data.GetStringExtra(PARAM_USER_PASS);
            var accountType = data.GetStringExtra(AccountManager.KeyAccountType);
            var account = new Account(accountName, accountType);

            bool isAddingNewAccount = Intent.GetBooleanExtra(ARG_IS_ADDING_NEW_ACCOUNT, false);
            if (isAddingNewAccount)
            {
                var authToken = data.GetStringExtra(AccountManager.KeyAuthtoken);
                var authtokenType = _AuthType;

                _AccountManager.AddAccountExplicitly(account, accountPassword, null);
                _AccountManager.SetAuthToken(account, authtokenType.ToString(), authToken);
            }
            else
            {
                _AccountManager.SetPassword(account, accountPassword);
            }

            SetAccountAuthenticatorResult(data.Extras);
            SetResult(Result.Ok, data);
            Finish();
        }

        private async Task Submit()
        {
            var userName = FindViewById<TextView>(Resource.Id.accountName).Text;
            var userPassword = FindViewById<TextView>(Resource.Id.accountPassword).Text;

            var accountType = Intent.GetStringExtra(ARG_ACCOUNT_TYPE);

            var intent = await Task.Run<Intent>(() =>
              {
                  String authtoken = null;
                  Bundle data = new Bundle();
                  try
                  {
                      // call server!
                      // authtoken = sServerAuthenticate.userSignIn(userName, userPass, mAuthTokenType);

                      data.PutString(AccountManager.KeyAccountName, userName);
                      data.PutString(AccountManager.KeyAccountType, accountType);
                      data.PutString(AccountManager.KeyAuthtoken, authtoken);
                      data.PutString(PARAM_USER_PASS, userPassword);

                  }
                  catch (Exception e)
                  {
                      data.PutString(KEY_ERROR_MESSAGE, e.Message);
                      throw;
                  }

                  var res = new Intent();
                  res.PutExtras(data);
                  return res;
              });


            if (intent.HasExtra(KEY_ERROR_MESSAGE))
            {
                Toast.MakeText(BaseContext, intent.GetStringExtra(KEY_ERROR_MESSAGE), ToastLength.Short).Show();
            }
            else
            {
                FinishLogin(intent);
            }


        }

    }
}



