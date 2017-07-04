using Android.OS;
using Android.Accounts;
using Android.Content;
using Gluon.Client.Jwt;

namespace Todo
{
    public class AndroidUsernamePasswordProvider : IUsernameAndPasswordProvider
    {
        //  private Context _context;
        private IAccountService _accountService;

        public AndroidUsernamePasswordProvider(IAccountService accountService)
        {
            _accountService = accountService;
            // context = context;
        }

        // public Account Account { get; set; }

        public Context Context { get; set; }

        public AccountAuthenticatorResponse Response { get; set; }

        public Bundle ResultBundle { get; set; }

        public UsernameAndPassword GetUsernameAndPassword()
        {
            var accountManager = AccountManager.Get(Context);
            var account = _accountService.CurrentUserAccount;
            //  string authToken = accountManager.PeekAuthToken(, null);

            var intent = new Intent(Context, typeof(AuthenticatorActivity));
            intent.PutExtra(AccountManager.KeyAccountAuthenticatorResponse, Response);
            intent.PutExtra(AuthenticatorActivity.ARG_ACCOUNT_TYPE, account.Type);
            intent.PutExtra(AuthenticatorActivity.ARG_AUTH_TYPE, "");
            intent.PutExtra(AuthenticatorActivity.ARG_ACCOUNT_NAME, account.Name);

            ResultBundle = new Bundle();
            ResultBundle.PutParcelable(AccountManager.KeyIntent, intent);
            return new AndroidUsernameAndPassword() { ResultBundle = ResultBundle };

        }
    }
}


