using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Todo
{
   

    public interface IAccountService
    {
        Task AddAccount(string accountType, string authTokenType);

        Task SetAccount(IUserAccount account);

        Task<string> GetRefreshToken(IUserAccount account);

        Task SetRefreshToken(IUserAccount account, string token);

        IUserAccount CurrentUserAccount { get; }

        Task<IEnumerable<IUserAccount>> GetAccounts(string accountType);

        // Task AddAccount(string accountType, string authTokenType);
    }

    public interface IUserAccount
    {
        string Name { get; set; }

        string Type { get; set; }

    }
}
