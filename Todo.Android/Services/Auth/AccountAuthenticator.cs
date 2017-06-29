using Android.OS;
using Android.Accounts;
using System;
using Android.Content;

namespace Todo
{
    public class AccountAuthenticator : AbstractAccountAuthenticator
    {

        private Context mContext;

        public AccountAuthenticator(Context context)
            : base(context)
        {
            Console.WriteLine("AccountAuthenticator");
            mContext = context;
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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public override Bundle GetAuthToken(AccountAuthenticatorResponse response, Account account, String authTokenType, Bundle bundle)
        {
            Console.WriteLine("Get Auth Token");

            // If the caller requested an authToken type we don't support, then
            // return an error
            AuthTokenType authType;
            if (!Enum.TryParse<AuthTokenType>(authTokenType, out authType))
            {
                Bundle result = new Bundle();
                result.PutString(AccountManager.KeyErrorMessage, "invalid authTokenType");
                return result;
            }

            // Extract the username and password from the Account Manager, and ask
            // the server for an appropriate AuthToken.
            var accountManager = AccountManager.Get(mContext);

            String authToken = accountManager.PeekAuthToken(account, authTokenType);

            Console.WriteLine("peekAuthToken returned - " + authToken);

            // Lets give another try to authenticate the user
            if (string.IsNullOrWhiteSpace(authToken))
            {
                String password = accountManager.GetPassword(account);
                if (!string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("re-authenticating with the existing password");
                    try
                    {
                        // todo: call server.
                        //  authToken = sServerAuthenticate.userSignIn(account.name, password, authTokenType);
                        authToken = Guid.NewGuid().ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        // e.printStackTrace();
                    }
                }

            }

            // If we get an authToken - we return it
            if (!string.IsNullOrWhiteSpace(authToken))
            {
                var result = new Bundle();
                result.PutString(AccountManager.KeyAccountName, account.Name);
                result.PutString(AccountManager.KeyAccountType, account.Type);
                result.PutString(AccountManager.KeyAuthtoken, authToken);
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

        public override String GetAuthTokenLabel(String authTokenType)
        {
            Console.WriteLine("Get Auth Token Label");

            AuthTokenType authType;
            if (Enum.TryParse<AuthTokenType>(authTokenType, out authType))
            {
                switch (authType)
                {
                    case AuthTokenType.FullAccess:
                        return "Full Access";
                    case AuthTokenType.ReadOnly:
                        return "Read Access";
                }
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


