using Android.OS;
using Android.Accounts;
using System;
using Android.Content;
using Gluon.Client.Jwt;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Todo
{

    public class AccountAuthenticator : AbstractAccountAuthenticator
    {

        private Context mContext;

        private ITokenManager _tokenManager;

        public AccountAuthenticator(Context context)
            : base(context)
        {
            Console.WriteLine("AccountAuthenticator");
            mContext = context;
            _tokenManager = MainApp.Current.ServiceProvider.GetService<ITokenManager>();
        }

        public override Bundle AddAccount(Android.Accounts.AccountAuthenticatorResponse response, string accountType, string authTokenType, string[] requiredFeatures, Bundle options)
        {
            try
            {
                Console.WriteLine("Add Account");

                Intent intent = new Intent(this.mContext, typeof(AuthenticatorActivity));

                intent.PutExtra(AuthenticatorActivity.ARG_ACCOUNT_TYPE, accountType);
                intent.PutExtra(AuthenticatorActivity.ARG_AUTH_TYPE, authTokenType);
                intent.PutExtra(AuthenticatorActivity.ARG_IS_ADDING_NEW_ACCOUNT, true);

                intent.PutExtra(AccountManager.KeyAccountAuthenticatorResponse, response);

                Bundle result = new Bundle();
                result.PutParcelable(AccountManager.KeyIntent, intent);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override Bundle GetAuthToken(AccountAuthenticatorResponse response, Account account, string authTokenType, Bundle bundle)
        {
            Console.WriteLine("Get Auth Token");

            // If the caller requested an authToken type we don't support, then
            // return an error
            AuthTokenType authType;
            if (!string.IsNullOrWhiteSpace(authTokenType) && !Enum.TryParse<AuthTokenType>(authTokenType, out authType))
            {
                Bundle result = new Bundle();
                result.PutString(AccountManager.KeyErrorMessage, "invalid authTokenType");
                return result;
            }

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var usernamePasswordProvider = MainApp.Current.ServiceProvider.GetRequiredService<AndroidUsernamePasswordProvider>();
            // usernamePasswordProvider.Account = account;
            usernamePasswordProvider.Context = mContext;
            usernamePasswordProvider.Response = response;


            try
            {
                // problem comes is the token manager needs to display a UI.
                var accessToken = _tokenManager.GetAccessToken(ct).Result;

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    var result = new Bundle();
                    result.PutString(AccountManager.KeyAccountName, account.Name);
                    result.PutString(AccountManager.KeyAccountType, account.Type);
                    result.PutString(AccountManager.KeyAuthtoken, accessToken);
                    // In addition AbstractAccountAuthenticator implementations that declare themselves android:customTokens=true may also provide a non-negative KEY_CUSTOM_TOKEN_EXPIRY long value containing the expiration timestamp of the expiration time (in millis since the unix epoch).
                    //  KEY_CUSTOM_TOKEN_EXPIRY 
                    return result;
                }

                // If we get here, then we couldn't access the user's password - so we
                // need to re-prompt them for their credentials. We do that by creating
                // an intent to display our AuthenticatorActivity.
                var intent = new Intent(mContext, typeof(AuthenticatorActivity));
                intent.PutExtra(AccountManager.KeyAccountAuthenticatorResponse, response);
                intent.PutExtra(AuthenticatorActivity.ARG_ACCOUNT_TYPE, account.Type);
                intent.PutExtra(AuthenticatorActivity.ARG_AUTH_TYPE, authTokenType);
                intent.PutExtra(AuthenticatorActivity.ARG_ACCOUNT_NAME, account.Name);

                var resultBundle = new Bundle();
                resultBundle.PutParcelable(AccountManager.KeyIntent, intent);
                return resultBundle;
            }
            catch (Exception e)
            {
                throw;
            }


        }

        public override String GetAuthTokenLabel(String authTokenType)
        {
            Console.WriteLine("Get Auth Token Label");

            AuthTokenType authType;
            if (Enum.TryParse<AuthTokenType>(authTokenType, out authType))
            {
                return "Global Connect";
                //switch (authType)
                //{

                //case AuthTokenType.FullAccess:
                //    return "Full Access";
                //case AuthTokenType.Access:
                //    return "Read Access";
                // }
            }

            return "authType" + " (Label)";

        }


        public override Bundle ConfirmCredentials(Android.Accounts.AccountAuthenticatorResponse response, Android.Accounts.Account account, Android.OS.Bundle options)
        {
            Console.WriteLine("Confirm Credentials");
            return this.ConfirmCredentials(response, account, options);
        }

        public override Bundle UpdateCredentials(AccountAuthenticatorResponse r, Account account, String s, Bundle bundle)
        {
            Console.WriteLine("Update Credentials");
            return null;
        }
        public override Bundle HasFeatures(AccountAuthenticatorResponse r, Account account, String[] strings)
        {
            Console.WriteLine("Has Featurea");
            var result = new Bundle();
            result.PutBoolean(AccountManager.KeyBooleanResult, false);
            return result;
        }
        public override Bundle EditProperties(AccountAuthenticatorResponse r, String s)
        {
            Console.WriteLine("Edit Properties");
            return null;
        }

    }
}


