using System;
using Android.Accounts;

namespace Todo
{
    public class AndroidUserAccount : IUserAccount
    {
        private readonly Account _account;

        public AndroidUserAccount(Account account)
        {
            _account = account;
        }
        public string Name { get { return _account.Name; } set { _account.Name = value; } }

        public Account Account { get { return _account; } }

        public string Type { get { return _account.Type; } set { _account.Type = value; } }
    }

}





