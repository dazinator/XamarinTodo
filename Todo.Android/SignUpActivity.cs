using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Accounts;
using System;
using Android.Content;
using Android.Widget;
using System.Threading.Tasks;

namespace Todo
{
    [Activity(Label = "Todo", Icon = "@drawable/icon", Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SignUpActivity : Activity
    {

        //private AccountManager _accountManager;

        private string _accountType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var authTokenType = Intent.GetStringExtra(AuthenticatorActivity.ARG_ACCOUNT_TYPE);

            AuthTokenType authType;
            if (!Enum.TryParse<AuthTokenType>(authTokenType, out authType))
            {
                authType = AuthTokenType.AccessToken;
            }
            _accountType = AuthContstants.AccountType;


            SetContentView(Resource.Layout.Register);

            FindViewById<Button>(Resource.Id.alreadyMember).Click += AllreadyMemberButton_Click;

            FindViewById<Button>(Resource.Id.submit).Click += async delegate
            {
                await Submit();
            };

        }

        private async Task Submit()
        {
            var name = FindViewById<TextView>(Resource.Id.name).Text.Trim();
            var userName = FindViewById<TextView>(Resource.Id.accountName).Text.Trim();
            var userPassword = FindViewById<TextView>(Resource.Id.accountPassword).Text.Trim();

            var result = await Task.Run<Intent>(() =>
            {

                String authtoken = null;
                Bundle data = new Bundle();

                try
                {
                    // call server to sign up user..
                    //  authtoken = sServerAuthenticate.userSignUp(name, accountName, accountPassword, AccountGeneral.AUTHTOKEN_TYPE_FULL_ACCESS);

                    authtoken = Guid.NewGuid().ToString();

                    data.PutString(AccountManager.KeyAccountName, userName);
                    data.PutString(AccountManager.KeyAccountType, _accountType.ToString());
                    data.PutString(AccountManager.KeyAuthtoken, authtoken);

                   // data.PutString(AuthenticatorActivity.PARAM_USER_PASS, userPassword);

                }
                catch (Exception e)
                {
                    data.PutString(AuthenticatorActivity.KEY_ERROR_MESSAGE, e.Message);
                    //throw;
                }

                Intent res = new Intent();
                res.PutExtras(data);
                return res;

            });


            if (result.HasExtra(AuthenticatorActivity.KEY_ERROR_MESSAGE))
            {
                Toast.MakeText(BaseContext, Intent.GetStringExtra(AuthenticatorActivity.KEY_ERROR_MESSAGE), ToastLength.Short).Show();
            }
            else
            {
                SetResult(Result.Ok, result);
                Finish();
            }




        }


        private void AllreadyMemberButton_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }

        public override void OnBackPressed()
        {
            SetResult(Result.Canceled);
            base.OnBackPressed();
        }
    }
}


